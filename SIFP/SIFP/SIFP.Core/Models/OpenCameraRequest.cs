using BinarySerialization;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIFP.Core.Models
{
    public class OpenCameraRequest:MsgHeader
    {
        [FieldOrder(0)]
        public DevTypeE CameraType { set; private get; }

        [FieldLength(4)]//bool类型默认为1字节 true为1 false为0
        [FieldOrder(1)]
        public bool Reset { set; private get; }
    }
}
