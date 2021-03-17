using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RecvMsgAttribute : Attribute
    {
        public UInt32 MsgType;
        public Type DataType;
        public RecvMsgAttribute(UInt32 msgType,Type type)
        {
            this.MsgType = msgType;
            this.DataType = type;
        }
    }
}
