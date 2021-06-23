using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class ConfigVcselDriver
    {
        [FieldOrder(1)]
        public VDriverWorkModeE Mode { get; set; } = VDriverWorkModeE.Fix;

        [FieldOrder(2)]
        public UInt32 IBias { get; set; }//threshold current uA

        [FieldOrder(3)]
        public UInt32 ISw { get; set; } = 3;//switch current uA
    }

    public enum VDriverWorkModeE
    {
        Fix = 0,
        Fake_Apc = 1,
        Apc = 2,
    }
}
