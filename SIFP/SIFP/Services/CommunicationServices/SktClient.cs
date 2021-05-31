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
using System.Threading;

namespace Services
{
    public class SktClient : ICommClient
    {
        public const UInt32 MAX_PKT_LEN = 65000;

        private readonly object socketLock = new object();

        public BlockingCollection<byte[]> RecvDatas { get; set; } = new BlockingCollection<byte[]>(3000);

        private UdpClient client = null;
        private IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);

        public bool Close()
        {
            if (client != null)
            {
                client.Close();
                client.Dispose();
            }
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
                if (client != null)
                {
                    RecvDatas.Add(client.EndReceive(ar, ref remoteEP));
                }
            }
            catch (Exception ex)
            {
                //异常处理
                Log.Error(ex, "RecvDataCallBack Error");
            }
            finally
            {
                if (client != null)
                    client.BeginReceive(RecvDataCallBack, null);
            }
        }

        private int Send(byte[] data)
        {
            try
            {
                lock (socketLock)
                {
                    if (null != client)
                    {
                        IAsyncResult result = client.BeginSend(data, data.Length, remoteEP, null, null);
                        if (result.AsyncWaitHandle.WaitOne(1000))
                            Thread.Sleep(100);

                        return client.EndSend(result);
                    }
                }
            }
            catch (Exception)
            {
                //异常处理
            }
            return 0;
        }

        private void SendDataCallBack(IAsyncResult ar)
        {
            if (ar.IsCompleted)
            {
                try
                {
                    if (null != client)
                    {
                        int ret = client.EndSend(ar);
                    }
                }
                catch (Exception)
                {
                    //异常处理
                }
            }
        }

        UInt32 pktSN = UInt32.MinValue;
        public int Send(MsgHeader msg)
        {
            if (null == client)
                return 0;

            msg.PktSN = pktSN;
            if (pktSN == UInt32.MaxValue)
                pktSN = UInt32.MinValue;
            else
                pktSN++;
            msg.TotalMsgNum = 1;
            msg.MsgSn = 0;
            msg.MsgType = msg.GetMsgType();
            msg.MsgLen = msg.GetMsgLen();
            msg.Timeout = 0xFFFFFFFF;

            return Send(BinarySerialize(msg));
        }

        private byte[] BinarySerialize(object obj)
        {
            BinarySerializer serializer = new BinarySerializer();
            using MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, obj);

            return ms.ToArray();
        }
    }
}
