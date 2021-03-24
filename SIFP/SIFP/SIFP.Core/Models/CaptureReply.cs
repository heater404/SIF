using BinarySerialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class CaptureReply : MsgHeader
    {
        [FieldOrder(1)]
        [FieldLength(4)]
        public bool ACK { get; set; }
    }
}
