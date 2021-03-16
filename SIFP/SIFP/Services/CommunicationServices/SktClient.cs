using Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BinarySerialization;
using System.IO;
using Serilog;
using SIFP.Core.Models;
using SIFP.Core.Attributes;

namespace Services
{
    public class SktClient : ICommClient
    {
        public const UInt32 MAX_PKT_LEN = 65000;

        private readonly Dictionary<UInt32,Action<MsgHeader>> procMap = new Dictionary<uint, Action<MsgHeader>>();
        private readonly object socketLock = new object();

        private readonly BlockingCollection<byte[]> recvDatas = new BlockingCollection<byte[]>(3000);

        private UdpClient client = null;
        private IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);

        private void InitProcessMap()
        {
            Type t = typeof(SktClient);
            var ms = t.GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            foreach (var m in ms)
            {
                foreach (var attr in m.GetCustomAttributes(false))
                {
                    if (attr is MsgTypeAttribute)
                    {
                        procMap.Add((UInt32)(attr as MsgTypeAttribute).MsgType, (Action<MsgHeader>)m.CreateDelegate(typeof(Action<MsgHeader>), this));
                        break;
                    }
                }
            }
        }

        public SktClient()
        {
            InitProcessMap();
        }

        public bool Close()
        {
            if (client != null)
                client.Close();
            client = null;
            return true;
        }

        public bool Open()
        {
            if (null == client)
            {
                try
                {
                    client = new UdpClient(8901);
                    client.Client.ReceiveBufferSize = 1 * 1024 * 1024;
                    Task.Run(AnalyseOnePacket);

                    client.BeginReceive(RecvDataCallBack, null);
                }
                catch (SocketException se)
                {
                    Log.Error(se, "SktClient Open Error");
                    return false;
                }
                return true;
            }
            return true;
        }

        private void RecvDataCallBack(IAsyncResult ar)
        {
            try
            {
                byte[] recvData = null;
                if (client != null)
                {
                    recvData = client.EndReceive(ar, ref remoteEP);

                    recvDatas.Add(recvData);
                }
            }
            catch (Exception)
            {
                //异常处理
            }
            finally
            {
                if (client != null)
                    client.BeginReceive(RecvDataCallBack, null);
            }
        }

        private void AnalyseOnePacket()
        {
            while (true)
            {
                if (!recvDatas.TryTake(out byte[] recvData, -1))
                    continue;

                if (0 == RecvOnePkt(recvData, out MsgHeader msg))
                {
                    foreach (var proc in procMap)
                        if (msg.MsgType == proc.Key)
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
            //Console.WriteLine("RecvOnePkt:");
            //Console.WriteLine("PktSN = " + pkt.Header.PktSN.ToString());
            //Console.WriteLine("MsgSn = " + pkt.Header.MsgSn.ToString());
            //Console.WriteLine("MsgType = 0x" + pkt.Header.MsgType.ToString("x"));
            //Console.WriteLine("MsgLen = " + pkt.Header.MsgLen.ToString());
            //Console.WriteLine("TotalMsgLen = " + pkt.Header.TotalMsgLen.ToString());

            msg = new OpenCameraReply();
            return 0;   // rcv done
        }

        public async Task<int> SendAsync(MsgHeader msg)
        {
            if (null == client)
                return 0;

            BinarySerializer serializer = new BinarySerializer();
            using MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, msg);

            byte[] datagram = ms.ToArray();

            return await client.SendAsync(datagram, datagram.Length);
        }

        //[MsgType(0x122)]
        //private void CmdProConfigCameraReply(MsgHeader pkt)
        //{
        //    if (!(pkt is ConfigCameraReply msg))
        //        return;

        //    WatchLog.PrintWatchLog($"ConfigCameraReply => {msg.ConfigAck}  ImageWidth:{msg.OutImageWidth} ImageHeight:{msg.OutImageHeight} " +
        //        $"PhaseNum:{msg.NumPhaseFramePerMIPIFrame} AddInfoLines:{msg.AddInfoLines}", LogType.Warning);
        //    if (msg.ConfigAck)
        //    {
        //        Messenger.Default.Send<Size>(new Size(msg.OutImageWidth, msg.OutImageHeight - msg.AddInfoLines), "Resolution");
        //        waitHandle.Set();
        //    }
        //}

        [MsgType(0x120)]
        private void CmdProConnectCameraReply(MsgHeader pkt)
        {
            if (!(pkt is OpenCameraReply msg))
                return;

            //this.CamChipID = msg.CamChipID;
            //if (waitHandle.Set())
            //{
            //    WatchLog.PrintWatchLog($"OpenCameraReply => CamChipID:0x{msg.CamChipID:x4} ; CamName:{msg.CamName} ; MaxImageHeight:{msg.MaxImageHeight} ; MaxImageWidth:{msg.MaxImageWidth}", LogType.Warning);
            //}
        }
    }
}
