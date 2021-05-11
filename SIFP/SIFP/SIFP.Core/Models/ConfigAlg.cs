using BinarySerialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class ConfigAlg
    {
        [FieldLength(4)]//bool类型默认为1字节 true为1 false为0
        [FieldOrder(1)]
        public bool ByPassSocAlgorithm { get; set; }

        [FieldLength(4)]
        [FieldOrder(2)]
        public bool ReturnRawData { get; set; }

        [FieldLength(4)]
        [FieldOrder(3)]
        public bool ReturnDepthIamge { get; set; }

        [FieldLength(4)]
        [FieldOrder(4)]
        public bool ReturnGrayImage { get; set; }

        [FieldLength(4)]
        [FieldOrder(5)]
        public bool ReturnBGImage { get; set; }

        [FieldLength(4)]
        [FieldOrder(6)]
        public bool ReturnAmplitudeImage { get; set; }

        [FieldLength(4)]
        [FieldOrder(7)]
        public bool ReturnConfidence { get; set; }

        [FieldLength(4)]
        [FieldOrder(8)]
        public bool ReturnFlagMap { get; set; }

        [FieldLength(4)]
        [FieldOrder(9)]
        public bool ReturnPointcloud { get; set; }
    }
}
