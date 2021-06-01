using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    [MsgType(MsgTypeE.LensArgsRequestType)]
    public class LensArgsRequest : MsgHeader
    {
        public override uint GetMsgLen()
        {
            return 20;
        }

        [FieldOrder(1)]
        public UInt32 GainFx { get; set; } = 100;
        [FieldOrder(2)]
        public UInt32 GainFy { get; set; } = 100;
        [FieldOrder(3)]
        public UInt32 GainCx { get; set; } = 100;
        [FieldOrder(4)]
        public UInt32 GainCy { get; set; } = 100;
        [FieldOrder(5)]
        public UInt32 GainFovZoom { get; set; } = 100;
    }
}
