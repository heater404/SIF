using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MsgTypeAttribute : Attribute
    {
        public UInt32 MsgType;

        public MsgTypeAttribute(UInt32 msgType)
        {
            this.MsgType = msgType;
        }
    }
}
