using BinarySerialization;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class ConfigCameraRequest : MsgHeader
    {
        [FieldOrder(1)]
        public ConfigCameraModel ConfigCamera { get; set; }
    }
}
