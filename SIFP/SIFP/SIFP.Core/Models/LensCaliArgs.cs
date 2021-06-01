using BinarySerialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class LensCaliArgs
    {
        [FieldOrder(1)]
        public UInt32 GainFx { get; set; } = 100;
        [FieldOrder(2)]
        public UInt32 Fx { get; set; } = 528 * 100;
        [FieldOrder(3)]
        public UInt32 GainFy { get; set; } = 100;
        [FieldOrder(4)]
        public UInt32 Fy { get; set; } = 528 * 100;
        [FieldOrder(5)]
        public UInt32 GainCx { get; set; } = 100;
        [FieldOrder(6)]
        public UInt32 Cx { get; set; } = 320 * 100;
        [FieldOrder(7)]
        public UInt32 GainCy { get; set; } = 100;
        [FieldOrder(8)]
        public UInt32 Cy { get; set; } = 240 * 100;
        [FieldOrder(9)]
        public UInt32 GainFovZoom { get; set; } = 100;
        [FieldOrder(10)]
        public UInt32 FovZoom { get; set; } = 100;

        public override string ToString()
        {
            return Fx / (GainFx * 1.0f) + "_" + Fy / (GainFy * 1.0f) + 
                "_" + Cx / (GainCx * 1.0f) + "_" + Cy / (GainCy * 1.0f) + 
                "_" + FovZoom / (GainFovZoom * 1.0f);
        }
    }
}
