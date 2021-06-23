using BinarySerialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class ConfigVcselDriverReply : MsgHeader
    {
        [FieldOrder(1)]// 1: Vdriver config failed, 0: success
        public UInt32 Ack { get; set; }

        [FieldOrder(2)]
        public UInt32 IBiasMicroAmp { get; set; }

        [FieldOrder(3)]
        public UInt32 ISwitchMicroAmp { get; set; }

        [FieldOrder(4)]
        public UInt32 MaxIBiasMicroAmp { get; set; }

        [FieldOrder(5)]
        public UInt32 MaxISwitchMicroAmp { get; set; }
    }
}
