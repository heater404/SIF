using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class LensCaliArgs
    {
        public float Fx { get; set; }
        public float Fy { get; set; }
        public float Cx { get; set; }
        public float Cy { get; set; }
        public float FOVZoom { get; set; }

        public override string ToString()
        {
            return Fx + "_" + Fy + "_" + Cx + "_" + Cy + "_" + FOVZoom;
        }
    }
}
