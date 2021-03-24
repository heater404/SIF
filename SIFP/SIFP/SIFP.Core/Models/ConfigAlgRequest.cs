using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    [MsgType(MsgTypeE.ConfigAlgRequestType)]
    public class ConfigAlgRequest : MsgHeader
    {
        public override uint GetMsgLen()
        {
            return 36; 
        }
        [FieldOrder(1)]
        [FieldLength(4)]//默认占一个字节，true为1，false为0
        public bool ByPassSocAlgorithm { get; set; }

        [FieldOrder(2)]
        [FieldLength(4)]
        public bool ReturnRawData { get; set; }

        [FieldOrder(3)]
        [FieldLength(4)]
        public bool ReturnDepthIamge { get; set; }

        [FieldOrder(4)]
        [FieldLength(4)]
        public bool ReturnGrayImage { get; set; }

        [FieldOrder(5)]
        [FieldLength(4)]
        public bool ReturnBGImage { get; set; }

        [FieldOrder(6)]
        [FieldLength(4)]
        public bool ReturnAmplitudeImage { get; set; }

        [FieldOrder(7)]
        [FieldLength(4)]
        public bool ReturnConfidence { get; set; }

        [FieldOrder(8)]
        [FieldLength(4)]
        public bool ReturnFlagMap { get; set; }

        [FieldOrder(9)]
        [FieldLength(4)]
        public bool ReturnPointcloud { get; set; }
    }
}
