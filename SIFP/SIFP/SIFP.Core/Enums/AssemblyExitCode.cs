using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Enums
{
    public enum AssemblyExitCode
    {
        USB_2 = 1,
        Success = 0,
        Init_Buffer_Fail = -1,
        Init_USB_Fail = -2,
        Init_DataProcessor_Fail = -3,
        Init_DataCapture_Fail = -4,
        Init_Algorithm_Fail = -5,
        Init_CaliParam_Fail = -6,
        Assembly_Not_Exist = -7,
    }
}
