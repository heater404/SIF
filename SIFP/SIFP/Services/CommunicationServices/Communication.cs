using BinarySerialization;
using Prism.Events;
using Services.Interfaces;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class Communication : ICommunication
    {
        private ICommClient client;
        private IEventAggregator eventAggregator;
        private readonly AutoResetEvent configCameraWaitHandle = new AutoResetEvent(false);
        private readonly AutoResetEvent connectCameraWaitHandle = new AutoResetEvent(false);
        private readonly AutoResetEvent stopStreamingWaitHandle = new AutoResetEvent(false);
        private readonly AutoResetEvent disconnectCameraWaitHandle = new AutoResetEvent(false);
        private readonly AutoResetEvent configAlgWaitHandle = new AutoResetEvent(false);
        private readonly AutoResetEvent captureWaitHandle = new AutoResetEvent(false);
        private readonly AutoResetEvent lenArgsWaitHandle = new AutoResetEvent(false);
        private readonly AutoResetEvent detectWaitHandle = new AutoResetEvent(false);
        private readonly Dictionary<RecvMsgAttribute, Action<MsgHeader>> procMap = new Dictionary<RecvMsgAttribute, Action<MsgHeader>>();
        private void InitProcessMap()
        {
            Type t = typeof(Communication);
            var ms = t.GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            foreach (var m in ms)
            {
                foreach (var attr in m.GetCustomAttributes(false))
                {
                    if (attr is RecvMsgAttribute)
                    {
                        procMap.Add((attr as RecvMsgAttribute), (Action<MsgHeader>)m.CreateDelegate(typeof(Action<MsgHeader>), this));
                        break;
                    }
                }
            }
        }

        public Communication(ICommClient client, IEventAggregator eventAggregator)
        {
            this.client = client;
            this.eventAggregator = eventAggregator;
            InitProcessMap();
            Task.Run(AnalyseOnePacket);
        }

        public bool Open()
        {
            if (null == client)
                return false;

            if (client.Open())
            {
                return true;
            }

            return false;
        }

        public bool Close()
        {
            if (null == client)
                return false;

            return client.Close();
        }

        public void Subscribe()
        {
            List<UInt32> msgTable = new List<uint>();
            foreach (var item in procMap)
            {
                msgTable.Add((UInt32)item.Key.MsgType);
            }

            HelloRequest msg = new HelloRequest
            {
                MsgNum = (UInt32)msgTable.Count,
                MsgTable = msgTable.ToArray(),
            };

            this.client.Send(msg);
        }

        private void AnalyseOnePacket()
        {
            while (true)
            {
                if (!client.RecvDatas.TryTake(out byte[] recvData, -1))
                    continue;

                if (0 == RecvOnePkt(recvData, out MsgHeader msg))
                {
                    foreach (var proc in procMap)
                        if (msg?.MsgType == proc.Key.MsgType)
                            proc.Value?.Invoke(msg);
                }
            }
        }

        /// <summary>
        /// 收包这里不做分包合并，每一包都触发委托处理
        /// 在委托里面根据需要进行合包，分包信息在Header里面有，所以委托参数从Msg改为Packet，
        /// </summary>
        /// <param name="data">skt收到的报文字节</param>
        /// <param name="pkt">返回的自定义Packet类型对象</param>
        /// <returns>是否接收正确 0表示正常， 非0表示异常</returns>
        private int RecvOnePkt(byte[] data, out MsgHeader msg)
        {
            msg = null;
            var msgType = BitConverter.ToUInt32(data, 12);
            foreach (var proc in procMap)
            {
                if (msgType == (UInt32)proc.Key.MsgType)
                {
                    msg = BinaryDeserialize(data, proc.Key.DataType);
                    break;
                }
            }

            //Debug.WriteLine("RecvOnePkt:");
            //Debug.WriteLine("PktSN = " + msg.PktSN.ToString());
            //Debug.WriteLine("MsgSn = " + msg.MsgSn.ToString());
            //Debug.WriteLine("MsgType = 0x" + msg.MsgType.ToString("x"));
            //Debug.WriteLine("MsgLen = " + msg.MsgLen.ToString());
            //Debug.WriteLine("TotalMsgLen = " + msg.TotalMsgNum.ToString());

            return 0;   // rcv done
        }

        private MsgHeader BinaryDeserialize(byte[] data, Type type)
        {
            BinarySerializer serializer = new BinarySerializer();
            return (MsgHeader)serializer.Deserialize(data, type);
        }

        private bool configCameraSuccess;
        public bool? ConfigCamera(ConfigCameraModel configCamera, int millisecondsTimeout)
        {
            if (client == null)
                return false;

            ConfigCameraRequest request = new ConfigCameraRequest
            {
                ConfigCamera = configCamera,
            };

            if (this.client.Send(request) > 0)
            {
                if (configCameraWaitHandle.WaitOne(millisecondsTimeout))
                    return configCameraSuccess;
                else
                    return null;
            }
            return false;
        }

        private UInt32 CamChipID;
        /// <summary>
        /// OpenCamera的请求函数
        /// </summary>
        /// <param name="millisecondsTimeout">请求后等待Reply的时间</param>
        /// <returns>可为null的bool类型，为true则表示OpenCamera成功，为false则表示OpenCamera失败，为Null则表示在指定的时间内没有收到Reply</returns>
        public bool? ConnectCamera(int millisecondsTimeout)
        {
            if (client == null)
                return false;

            ConnectCameraRequest msg = new ConnectCameraRequest
            {
                CameraType = DevTypeE.TOF,
                Reset = false,
            };

            if (this.client.Send(msg) > 0)
            {
                if (connectCameraWaitHandle.WaitOne(millisecondsTimeout))
                    return CamChipID != 0xdeadbeef;
                else
                    return null;
            }

            return false;
        }

        public bool? StartStreaming(int millisecondsTimeout)
        {
            if (client == null)
                return false;

            StartStreamingRequest msg = new StartStreamingRequest
            {

            };

            if (this.client.Send(msg) > 0)
            {
                //todo:同步等待
                return true;
            }

            return false;
        }

        public bool? StopStreaming(int millisecondsTimeout)
        {
            if (client == null)
                return false;

            StopStreamingRequest msg = new StopStreamingRequest
            {

            };

            if (this.client.Send(msg) > 0)
            {
                if (stopStreamingWaitHandle.WaitOne(millisecondsTimeout))
                    return true;
                else
                    return null;
            }

            return false;
        }

        private bool disconnectCameraAck;
        public bool? DisconnectCamera(int millisecondsTimeout)
        {
            if (client == null)
                return false;

            DisconnectCameraRequest msg = new DisconnectCameraRequest
            {

            };

            if (this.client.Send(msg) > 0)
            {
                if (disconnectCameraWaitHandle.WaitOne(millisecondsTimeout))
                    return disconnectCameraAck;
                else
                    return null;
            }

            return false;
        }

        public bool? ConfigAlg(ConfigAlgRequest configAlg, int millisecondsTimeout)
        {
            if (client == null)
                return false;

            if (this.client.Send(configAlg) > 0)
            {
                if (configAlgWaitHandle.WaitOne(millisecondsTimeout))
                    return configAlgAck;
                else
                    return null;
            }
            return false;
        }

        private bool captureAck;
        public bool? AlgoAddCapture(UInt32 opt, UInt32 pos, UInt32 ID, UInt32 type,
               UInt32 frameNum, UInt32 cycle)
        {
            if (null == client)
                return false;

            UInt32 num = MaxDivisor(frameNum, cycle);
            List<Int32> sn = Enumerable.Range(0, (int)num).ToList<Int32>();

            UInt32 cnt = frameNum / num;

            CaptureRequest msg = new CaptureRequest()
            {
                CaptureOpt = opt,
                CapturePos = pos,
                CaptureID = ID,
                CaptureType = type,
                CaptureCnt = cnt,
                CaptureCycle = cycle,
                CaptureNum = num,
                CaptureSN = sn.ToArray<Int32>(),
            };

            if (0 < client.Send(msg))
            {
                if (captureWaitHandle.WaitOne((int)frameNum * 1000))
                    return this.captureAck;
                else
                    return null;
            }
            return false;
        }

        public bool AlgoDelCapture(UInt32 pos, UInt32 ID)
        {
            if (null == client)
                return false;

            CaptureRequest msg = new CaptureRequest()
            {
                CaptureOpt = 1,
                CapturePos = pos,
                CaptureID = ID,
                CaptureType = 0,
                CaptureCnt = 0,
                CaptureCycle = 0,
                CaptureNum = 0,
            };

            return client.Send(msg) > 0;
        }

        /// <summary>
        /// 求不超过cycle的frameNum的最大因数
        /// </summary>
        /// <param name="frameNun"></param>
        /// <param name="cycle"></param>
        /// <returns></returns>
        private static UInt32 MaxDivisor(UInt32 frameNun, UInt32 cycle)
        {
            UInt32 ret = 0;

            var min = Math.Min(frameNun, cycle);

            for (UInt32 i = min; i > 0; i--)
            {
                if (0 == frameNun % i)
                {
                    ret = i;
                    break;
                }
            }

            return ret;
        }

        #region ReadRegs
        /// <summary>
        /// 读寄存器的方法，里面组装了协议然后调用socket的接口
        /// </summary>
        /// <param name="regs">协议中需要的数据</param>
        /// <returns></returns>
        public bool ReadRegs(RegStruct[] regs, DevTypeE devType)
        {
            bool success = false;
            MsgHeader msg = new ReadRegisterRequest()
            {
                ConfigRegister = new ConfigRegisterModel
                {
                    DevType = devType,
                    NumRegs = (UInt32)regs.Length,
                    Regs = regs,
                }
            };
            success = client.Send(msg) > 0;

            return success;
        }
        #endregion ReadRegs

        #region WriteRegs
        /// <summary>
        /// 写寄存器的方法，里面组装了协议然后调用socket的接口
        /// </summary>
        /// <param name="regs">协议中需要的数据</param>
        /// <returns></returns>
        public bool WriteRegs(RegStruct[] regs, DevTypeE devType)
        {
            bool success = false;
            MsgHeader msg = new WriteRegisterRequest()
            {
                ConfigRegister = new ConfigRegisterModel
                {
                    DevType = devType,
                    NumRegs = (UInt32)regs.Length,
                    Regs = regs,
                }
            };
            success = client.Send(msg) > 0;

            return success;
        }
        #endregion

        public bool SwitchUserAccess(UserAccessType accessType)
        {
            if (null == client)
                return false;

            UserAccessRequest msg = new UserAccessRequest()
            {
                AccessType = accessType,
                PassWord = 0xcafe2610,
            };

            return client.Send(msg) > 0;
        }

        public bool? GetLensArgs(int millisecondsTimeout)
        {
            if (null == client)
                return false;

            LensArgsRequest msg = new LensArgsRequest()
            {

            };

            if (0 < client.Send(msg))
            {
                if (lenArgsWaitHandle.WaitOne(millisecondsTimeout))
                    return true;
                else
                    return null;
            }
            return false;
        }

        public async Task GetSysStatusAsync(CancellationToken cancellationToken, int interval)
        {
            if (null == client)
                return;

            GetSysStatusRequest msg = new GetSysStatusRequest()
            {
            };
            Task task = new Task(() =>
             {
                 while (!cancellationToken.IsCancellationRequested)
                 {
                     client.Send(msg);
                     Thread.Sleep(interval);
                 }
             }, cancellationToken, TaskCreationOptions.LongRunning);
            task.Start();
            await task;
        }

        public bool? ConfigArithParams(CorrectionParams correction, PostProcParams postProc)
        {
            if (null == client)
                return false;

            ConfigArithParamsRequest msg = new ConfigArithParamsRequest()
            {
                Correction = correction,
                PostProc = postProc,
                UseCorrParams = 1,
                UsePostProcParams = 1,
            };

            return client.Send(msg) > 0;
        }

        public bool? ConfigVcselDriver(ConfigVcselDriver vcselDriver)
        {
            if (null == client)
                return false;

            ConfigVcselDriverRequest msg = new ConfigVcselDriverRequest()
            {
                VcselDriver = vcselDriver,
            };

            return client.Send(msg) > 0;
        }

        public bool Detect(int times)
        {
            if (null == client)
                return false;

            for (int i = 0; i < times; i++)
            {
                DetectRequest msg = new DetectRequest()
                {
                    DetectMsg = new Detect { SN = 0xffffffff },
                };

                detectAck = false;
                if (client.Send(msg) > 0)
                {
                    if (detectWaitHandle.WaitOne(2000))
                    {
                        if (detectAck)
                            return true;
                        else
                            continue;
                    }
                    else
                        continue;
                }
                return false;
            }
            return false;
        }

        [RecvMsg(MsgTypeE.ConfigCameraReplyType, typeof(ConfigCameraReply))]
        private void CmdProConfigCameraReply(MsgHeader pkt)
        {
            if (!(pkt is ConfigCameraReply msg))
                return;

            configCameraSuccess = msg.ConfigAck == 0;
            if (configCameraWaitHandle.Set())
            {
                eventAggregator.GetEvent<ConfigCameraReplyEvent>().Publish(msg);
            }
        }

        [RecvMsg(MsgTypeE.ConnectCameraReplyType, typeof(ConnectCameraReply))]
        private void CmdProConnectCameraReply(MsgHeader pkt)
        {
            if (pkt is not ConnectCameraReply msg)
                return;

            this.CamChipID = msg.ToFChipID;
            if (connectCameraWaitHandle.Set())
            {
                eventAggregator.GetEvent<ConnectCameraReplyEvent>().Publish(msg);
            }
        }

        [RecvMsg(MsgTypeE.StopStreamingReplyType, typeof(StopStreamingReply))]
        private void CmdProcStopStreamingReply(MsgHeader pkt)
        {
            if (pkt is not StopStreamingReply msg)
                return;

            if (stopStreamingWaitHandle.Set())
            {
                eventAggregator.GetEvent<StopStreamingReplyEvent>().Publish(msg);
            }
        }

        private bool configAlgAck = false;
        [RecvMsg(MsgTypeE.ConfigAlgReplyType, typeof(ConfigAlgReply))]
        private void CmdProcConfigAlgReply(MsgHeader pkt)
        {
            if (pkt is not ConfigAlgReply msg)
                return;

            configAlgAck = msg.ConfigAck == 0;
            if (configAlgWaitHandle.Set())
            {
                eventAggregator.GetEvent<ConfigAlgReplyEvent>().Publish(msg);
            }
        }

        [RecvMsg(MsgTypeE.CaptureReplyType, typeof(CaptureReply))]
        private void CmdProcCaptureReply(MsgHeader pkt)
        {
            if (pkt is not CaptureReply msg)
                return;

            this.captureAck = msg.ACK;
            if (captureWaitHandle.Set())
            {
                eventAggregator.GetEvent<CaptureReplyEvent>().Publish(msg);
            }
        }

        [RecvMsg(MsgTypeE.ReadRegisterReplyType, typeof(ReadRegisterReply))]
        private void CmdProcReadRegisterReply(MsgHeader pkt)
        {
            if (pkt is not ReadRegisterReply msg)
                return;

            eventAggregator.GetEvent<ReadRegisterReplyEvent>().Publish(msg);
        }

        [RecvMsg(MsgTypeE.GetSysStatusReplyType, typeof(GetSysStatusReply))]
        private void CmdProcGetSysStatusReply(MsgHeader pkt)
        {
            if (pkt is not GetSysStatusReply msg)
                return;

            eventAggregator.GetEvent<GetSysStatusReplyEvent>().Publish(msg);
        }

        [RecvMsg(MsgTypeE.ConfigArithParamsReplyType, typeof(ConfigArithParamsReply))]
        private void CmdProcConfigArithParamsReply(MsgHeader pkt)
        {
            if (pkt is not ConfigArithParamsReply msg)
                return;

            eventAggregator.GetEvent<ConfigArithParamsReplyEvent>().Publish(msg);
        }

        [RecvMsg(MsgTypeE.ConfigVcselDriverReplyType, typeof(ConfigVcselDriverReply))]
        private void CmdProcConfigVcselDriverReply(MsgHeader pkt)
        {
            if (pkt is not ConfigVcselDriverReply msg)
                return;

            eventAggregator.GetEvent<ConfigVcselDriverReplyEvent>().Publish(msg);
        }

        [RecvMsg(MsgTypeE.DisconnectCameraReplyType, typeof(DisconnectCameraReply))]
        private void CmdProcDisconnectCameraReply(MsgHeader pkt)
        {
            if (pkt is not DisconnectCameraReply msg)
                return;

            disconnectCameraAck = msg.Ack == 0;
            if (disconnectCameraWaitHandle.Set())
            {
                eventAggregator.GetEvent<DisconnectCameraReplyEvent>().Publish(msg);
            }
        }

        [RecvMsg(MsgTypeE.LensArgsReplyType, typeof(LensArgsReply))]
        private void CmdProcLensArgsReply(MsgHeader pkt)
        {
            if (pkt is not LensArgsReply msg)
                return;

            if (lenArgsWaitHandle.Set())
                this.eventAggregator.GetEvent<LensArgsReplyEvent>().Publish(msg);
        }

        bool detectAck = false;
        [RecvMsg(MsgTypeE.DetectReplyType, typeof(DetectReply))]
        private void CmdProcDetectReply(MsgHeader pkt)
        {
            if (pkt is not DetectReply msg)
                return;

            detectAck = msg.DetectMsg.ACK == 1;
            detectWaitHandle.Set();
        }
    }
}
