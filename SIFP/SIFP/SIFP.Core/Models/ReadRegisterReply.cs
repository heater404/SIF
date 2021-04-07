using BinarySerialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class ReadRegisterReply:MsgHeader
    {
        [FieldOrder(1)]
        public ConfigRegisterModel ConfigRegister { get; set; }
    }
}
