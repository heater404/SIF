using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    [MsgType(MsgTypeE.UserAccessRequestType)]
    public class UserAccessRequest : MsgHeader
    {
        public override uint GetMsgLen()
        {
            return 8;
        }
        [FieldOrder(1)]
        public UserAccessType AccessType { get; set; }

        [FieldOrder(2)]
        public UInt32 PassWord { get; set; }
    }
}
