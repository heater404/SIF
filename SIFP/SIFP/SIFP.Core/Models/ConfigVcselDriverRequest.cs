using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    [MsgType(MsgTypeE.ConfigVcselDriverRequestType)]
    public class ConfigVcselDriverRequest : MsgHeader
    {
        public override uint GetMsgLen()
        {
            return 12;
        }

        [FieldOrder(1)]
        public ConfigVcselDriver VcselDriver { get; set; }
    }
}
