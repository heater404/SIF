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
        public UInt32 IBias { get; set; } = 400;//threshold current mA

        [FieldOrder(3)]
        [SerializeAs(serializedType: SerializedType.UInt4)]
        [FieldScale(1000)]//协议中需要mA，放大1000倍
        public float Isw { get; set; } = 3;//switch current A
    }

    public enum VDriverWorkModeE
    {
        Fix=0,
        Fake_Apc=1,
        Apc=2,
    }
}
