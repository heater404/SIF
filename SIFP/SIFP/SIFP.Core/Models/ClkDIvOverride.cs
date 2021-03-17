using BinarySerialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class ClkDIvOverride
    {
        [FieldOrder(1)]
        //PLL pre-divider, 1(default), 2, 3, 4
        public uint PLLPreDiv { get; set; }

        //PLL internal feedback divider 2,4,6...254, default:50
        [FieldOrder(2)]
        public uint PLLFBDiv { get; set; }

        [FieldOrder(3)]
        //PLL to slow digital clock divider, 1, 2(default), 3, 4
        public uint ClkDigSlowDiv { get; set; }
    }
}
