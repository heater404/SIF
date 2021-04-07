using BinarySerialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class Register
    {
        [FieldOrder(1)]
        public UInt32 RegAddr { get; set; }

        [FieldOrder(2)]
        public UInt32 RegValue { get; set; }
    }
}
