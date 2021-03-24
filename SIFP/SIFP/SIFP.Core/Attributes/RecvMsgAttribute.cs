using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RecvMsgAttribute : MsgTypeAttribute
    {
        public Type DataType;
        public RecvMsgAttribute(MsgTypeE msgType, Type type) : base(msgType)
        {
            this.DataType = type;
        }
    }
}
