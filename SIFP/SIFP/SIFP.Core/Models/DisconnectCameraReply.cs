using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    [MsgType(MsgTypeE.DisconnectCameraReplyType)]
    public class DisconnectCameraReply : MsgHeader
    {
        public UInt32 Ack { get; set; }//0表示success  1表示fail
    }
}
