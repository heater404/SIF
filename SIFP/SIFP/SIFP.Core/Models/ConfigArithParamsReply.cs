using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    [MsgType(MsgTypeE.ConfigArithParamsReplyType)]
    public class ConfigArithParamsReply:MsgHeader
    {
        [FieldOrder(1)]
        public UInt32 Ack { get; set; }//0表示success  1表示fail

        [FieldOrder(2)]
        [FieldCount(4)]
        public IntegrationTime[] IntegrationTimes { get; set; }
    }
}
