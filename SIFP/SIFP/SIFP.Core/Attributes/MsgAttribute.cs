using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MsgTypeAttribute : Attribute
    {
        public MsgTypeE MsgType;

        public MsgTypeAttribute(MsgTypeE msgType)
        {
            this.MsgType = msgType;
        }
    }
}
