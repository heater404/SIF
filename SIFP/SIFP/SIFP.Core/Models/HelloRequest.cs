using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    [MsgType(MsgTypeE.HelloRequestMsgType)]
    public class HelloRequest : MsgHeader
    {
        [FieldLength(6)]
        [FieldOrder(1)]
        public string LocID { get; set; }//本地设备唯一标识符（暂未使用) 6字节

        [FieldOrder(2)]
        public UInt32 MsgNum { get; set; }//订阅Msg个数

        [FieldCount(nameof(MsgNum))]
        [FieldOrder(3)]
        public UInt32[] MsgTable { get; set; }

        [FieldCount(1500)]
        public byte[] Reserve { get; set; }
    }
}
