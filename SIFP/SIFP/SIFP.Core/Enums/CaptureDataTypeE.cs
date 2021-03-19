using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Enums
{
    public enum CaptureDataTypeE : UInt32
    {
        Raw = 1,
        Depth = 2,
        Gray = 3,
        Raw_Depth = 4,
        Depth_Gray = 5
    }
}
