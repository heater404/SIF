using BinarySerialization;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class ConfigRegisterModel
    {
        [FieldOrder(1)]
        public DevTypeE DevType { get; set; }

        [FieldOrder(2)]
        public UInt32 NumRegs { get; set; }

        [FieldOrder(3)]
        [FieldCount(nameof(NumRegs))]
        public Register[] Regs { get; set; }
    }
}
