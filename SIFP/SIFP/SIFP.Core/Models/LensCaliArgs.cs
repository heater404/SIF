using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class LensCaliArgs
    {
        public float Fx { get; set; } = 528;
        public float Fy { get; set; } = 528;
        public float Cx { get; set; } = 320;
        public float Cy { get; set; } = 240;
        public float FOVZoom { get; set; } = 1;

        public override string ToString()
        {
            return Fx + "_" + Fy + "_" + Cx + "_" + Cy + "_" + FOVZoom;
        }
    }
}
