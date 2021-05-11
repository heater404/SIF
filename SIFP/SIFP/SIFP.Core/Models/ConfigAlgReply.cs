using BinarySerialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class ConfigAlgReply : MsgHeader
    {
        [FieldOrder(1)]
        public UInt32 ConfigAck { get; set; }//0-true 1-false
    }
}
