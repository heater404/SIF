using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Xunit;

namespace DummyServer
{
    public class ServerReplyTest
    {
        UdpClient client;
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8901);
        public ServerReplyTest()
        {
            Random random = new Random();
            client = new UdpClient(random.Next(1111, 9999));
        }

        [Fact]
        public void DisconnectCameraReplyTest()
        {
            DisconnectCameraReply reply = new DisconnectCameraReply
            {
                Ack = 0,
            };

            Send(reply);
        }

        private int Send(MsgHeader msg)
        {
            if (null == client)
                return 0;

            msg.PktSN = 0x12345678;
            msg.TotalMsgLen = 1;
            msg.MsgSn = 0;
            msg.MsgType = msg.GetMsgType();
            msg.MsgLen = msg.GetMsgLen();
            msg.Timeout = 0xFFFFFFFF;

            var data = BinarySerialize(msg);

            return client.Send(data, data.Length, remoteEP);
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
