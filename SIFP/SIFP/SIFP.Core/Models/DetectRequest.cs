using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SIFP.Core.Models
{
    [MsgType(MsgTypeE.DetectRequestType)]
    public class DetectRequest:MsgHeader
    {
        public override uint GetMsgLen()
        {
            //return 14;
            var count = this.GetMsgLen(this.GetType()); //base.GetMsgLen();
            Debug.WriteLine(count);
            return count;
        }

        [FieldOrder(1)]
        public Detect DetectMsg { get; set; }

        private uint GetMsgLen(Type type)
        {
            uint count = 0;
            var properties = type.GetProperties();
            if (properties.Length == 0)
                return count;
            foreach (var pro in properties)
            {
                foreach (var attr in pro.GetCustomAttributes(false))
                {
                    if (attr.GetType() == typeof(FieldLengthAttribute))
                    {
                        count += (uint)((FieldLengthAttribute)attr).ConstLength;
                    }
                    else
                        count += GetMsgLen(pro.GetType());
                }
            }
            return count;
        }
    }
}
