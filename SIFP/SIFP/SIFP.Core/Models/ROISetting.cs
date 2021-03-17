using BinarySerialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class ROISetting
    {
        [FieldOrder(1)]
        //x start position
        public UInt16 XStart { get; set; }

        [FieldOrder(2)]
        //x final output size (must be multiple of 4)
        public UInt16 XSize { get; set; }

        [FieldOrder(3)]
        //x step size (1-32)
        public UInt16 XStep { get; set; }

        [FieldOrder(4)]
        //y start position (must start on even number position)
        public UInt16 YStart { get; set; }

        [FieldOrder(5)]
        //y final output size (no limit on multiples)
        public UInt16 YSize { get; set; }

        [FieldOrder(6)]
        //y step size (1-32)
        public UInt16 YStep { get; set; }
    }
}
