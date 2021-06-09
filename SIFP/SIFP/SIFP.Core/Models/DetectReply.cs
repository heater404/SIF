using BinarySerialization;
using SIFP.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    [MsgType(Enums.MsgTypeE.DetectReplyType)]
    public class DetectReply : MsgHeader
    {
        [FieldOrder(1)]
        public Detect DetectMsg { get; set; }
    }
}
