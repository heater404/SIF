using BinarySerialization;
using SIFP.Core.Converters;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SIFP.Core.Models
{
    public class RegStruct
    {
        [FieldOrder(1)]
        [JsonConverter(typeof(DecimalHexConverter))]
        public UInt32 RegAddr { get; set; }

        [FieldOrder(2)]
        [JsonConverter(typeof(DecimalHexConverter))]
        public UInt32 RegVal { get; set; }
    }
    public struct RegOperateStruct
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RegOperateType OperateType { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DevTypeE RegType { get; set; }
        public RegStruct Register { get; set; }
    }
}
