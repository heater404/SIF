using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class GetSysStatusReply : MsgHeader
    {
        [FieldOrder(1)]
        public UInt32 Reserve1 { get; set; }
        [FieldOrder(2)]
        public UInt32 Reserve2 { get; set; }

        [FieldOrder(3)]
        [FieldScale(1000)]
        [SerializeAs(serializedType: SerializedType.UInt4)]
        public double VcselTemp { get; set; }//扩大1000倍的摄氏度

    }
}
