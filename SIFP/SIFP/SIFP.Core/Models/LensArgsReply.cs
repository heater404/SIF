using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    [MsgType(MsgTypeE.LensArgsReplyType)]
    public class LensArgsReply : MsgHeader
    {
        public override uint GetMsgLen()
        {
            return 40;
        }

        [FieldOrder(1)]
        public LensCaliArgs LensArgs { get; set; }
    }
}
