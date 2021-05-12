using BinarySerialization;
using SIFP.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIFP.Core.Models
{
    public class ConnectCameraReply : MsgHeader
    {
        [FieldOrder(1)]
        public UInt32 ToFChipID { get; set; }

        [FieldOrder(2)]
        public UInt32 RGBChipID { get; set; }

        [FieldLength(64)]
        [FieldOrder(3)]
        public string ToFCamName { get; set; }

        [FieldLength(64)]
        [FieldOrder(4)]
        public string RGBCamName { get; set; }

        [FieldOrder(5)]
        public UInt32 ToFMaxImageWidth { get; set; }

        [FieldOrder(6)]
        public UInt32 ToFMaxImageHeight { get; set; }

        [FieldOrder(7)]
        public UInt32 RGBMaxImageWidth { get; set; }

        [FieldOrder(8)]
        public UInt32 RGBMaxImageHeight { get; set; }

        [FieldOrder(9)]
        public UInt32 LotNumber { get; set; }

        [FieldOrder(10)]
        public UInt32 WaferId { get; set; }
    }
}
