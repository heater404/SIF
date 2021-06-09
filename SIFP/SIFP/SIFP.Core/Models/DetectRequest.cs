using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    [MsgType(MsgTypeE.DetectRequestType)]
    public class DetectRequest:MsgHeader
    {
        public override uint GetMsgLen()
        {
            return 14;
        }

        [FieldOrder(1)]
        public Detect DetectMsg { get; set; }
    }
}
