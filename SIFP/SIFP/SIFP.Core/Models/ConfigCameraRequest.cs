using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    [MsgType(MsgTypeE.ConfigCameraRequestType)]
    public class ConfigCameraRequest : MsgHeader
    {
        [FieldOrder(1)]
        public ConfigCameraModel ConfigCamera { get; set; }

        public override uint GetMsgLen()
        {
            return 244;
        }
    }
}
