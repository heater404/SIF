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

    public enum CaptureOpt : UInt32
    {
        Add=0,
        Delete=1,
        AddToLoacl=2,
    }

    public enum CapturePosition : UInt32
    {
        Pos= 0x8001,
    }

    public enum CaptureID : UInt32
    {
        ID= 0x9997,
    }
}
