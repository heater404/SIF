using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    [MsgType(MsgTypeE.CaptureRequestType)]
    public class CaptureRequest : MsgHeader
    {
        public override uint GetMsgLen()
        {
            return 7 * 4 + 4 * CaptureNum;
        }

        [FieldOrder(1)]
        public uint CaptureOpt { get; set; }

        [FieldOrder(2)]
        public uint CapturePos { get; set; }

        [FieldOrder(3)]
        public uint CaptureID { get; set; }

        [FieldOrder(4)]
        public uint CaptureType { get; set; }

        [FieldOrder(5)]
        public uint CaptureCnt { get; set; }

        [FieldOrder(6)]
        public uint CaptureCycle { get; set; }

        [FieldOrder(7)]
        public uint CaptureNum { get; set; }

        [FieldCount(nameof(CaptureNum))]
        [FieldOrder(8)]
        public Int32[] CaptureSN { get; set; }
    }
}
