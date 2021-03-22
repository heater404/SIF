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
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class Communication : ICommunication
    {
        private ICommClient client;
        private IEventAggregator eventAggregator;
        private readonly AutoResetEvent waitHandle = new AutoResetEvent(false);
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
                Subscribe();
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

        private void Subscribe()
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
                        if (msg.MsgType == proc.Key.MsgType)
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
        public bool? ConfigCamera(ConfigCameraRequest configCamera, int millisecondsTimeout)
        {
            if (client == null)
                return false;

            if (this.client.Send(configCamera) > 0)
            {
                if (waitHandle.WaitOne(millisecondsTimeout))
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
                if (waitHandle.WaitOne(millisecondsTimeout))
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
                if (waitHandle.WaitOne(millisecondsTimeout))
                    return true;
                else
                   return null;
            }

            return false;
        }

        public bool? DisconnectCamera(int millisecondsTimeout)
        {
            if (client == null)
                return false;

            DisconnectCameraRequest msg = new DisconnectCameraRequest
            {

            };

            if (this.client.Send(msg) > 0)
            {
                //同步等待
                return true;
            }

            return false;
        }

        [RecvMsg(MsgTypeE.ConfigCameraReplyType, typeof(ConfigCameraReply))]
        private void CmdProConfigCameraReply(MsgHeader pkt)
        {
            if (!(pkt is ConfigCameraReply msg))
                return;

            configCameraSuccess = msg.ConfigAck == 0;
            if (waitHandle.Set())
            {
                eventAggregator.GetEvent<ConfigCameraReplyEvent>().Publish(msg);
            }
        }

        [RecvMsg(MsgTypeE.ConnectCameraReplyType, typeof(ConnectCameraReply))]
        private void CmdProConnectCameraReply(MsgHeader pkt)
        {
            if (pkt is not ConnectCameraReply msg)
                return;

            this.CamChipID = msg.CamChipID;
            if (waitHandle.Set())
            {
                eventAggregator.GetEvent<ConnectCameraReplyEvent>().Publish(msg);
            }
        }

        [RecvMsg(MsgTypeE.StopStreamingReplyType,typeof(StopStreamingReply))]
        private void CmdProcStopStreamingReply(MsgHeader pkt)
        {
            if (pkt is not StopStreamingReply msg)
                return;

            if (waitHandle.Set())
            {
                eventAggregator.GetEvent<StopStreamingReplyEvent>().Publish(msg);
            }
        }
    }
}
