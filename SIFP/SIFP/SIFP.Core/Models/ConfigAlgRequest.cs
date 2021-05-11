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
        public ConfigAlg Config { get; set; }
    }
}
