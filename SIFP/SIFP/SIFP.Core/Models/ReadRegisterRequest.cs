using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    [MsgType(MsgTypeE.ReadRegisterRequestType)]
    public class ReadRegisterRequest : MsgHeader
    {
        public override uint GetMsgLen()
        {
            return 4 + 4 + ConfigRegister.NumRegs * 8;
        }

        [FieldOrder(1)]
        public ConfigRegisterModel ConfigRegister { get; set; }
    }
}
