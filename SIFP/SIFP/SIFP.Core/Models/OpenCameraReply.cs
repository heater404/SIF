using BinarySerialization;
using SIFP.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIFP.Core.Models
{
    public class OpenCameraReply:MsgHeader
    {
        [FieldOrder(1)]
        public UInt32 CamChipID { get; private set; }

        [FieldLength(64)]
        [FieldOrder(2)]
        public string CamName { get; private set; }

        [FieldOrder(3)]
        public UInt32 MaxImageWidth { get; private set; }

        [FieldOrder(4)]
        public UInt32 MaxImageHeight { get; private set; }
    }
}
