using BinarySerialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class Detect
    {
        [FieldOrder(1)]
        [FieldCount(6)]
        [FieldLength(6)]
        public byte[] Reserved { get; set; }

        [FieldOrder(2)]
        [FieldLength(4)]
        public UInt32 SN { get; set; }

        [FieldOrder(3)]
        [FieldLength(4)]
        public UInt32 ACK { get; set; }//为1的时候表示detect正常
    }
}
