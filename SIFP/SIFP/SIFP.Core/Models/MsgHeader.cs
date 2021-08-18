using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SIFP.Core.Models
{
    public abstract class MsgHeader
    {
        [Ignore]
        public const UInt32 HeaderLen = 24;
        public virtual UInt32 GetMsgLen()
        {
            return GetMsgLen(this.GetType()) - HeaderLen;
        }

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

        public virtual MsgTypeE GetMsgType()
        {
            Type t = this.GetType();
            foreach (var att in t.GetCustomAttributes(true))
            {
                if (att is MsgTypeAttribute)
                {
                    return (att as MsgTypeAttribute).MsgType;
                }
            }
            return 0x00;
        }

        // pkt Header
        [FieldOrder(1)]
        [FieldLength(4)]
        public uint PktSN { get; set; }

        //更改为总的包个数
        [FieldOrder(2)]
        [FieldLength(4)]
        public uint TotalMsgLen { get; set; }

        [FieldOrder(3)]
        [FieldLength(4)]
        public uint MsgSn { get; set; }

        [FieldOrder(4)]
        [FieldLength(4)]
        public MsgTypeE MsgType { get; set; }

        [FieldOrder(5)]
        [FieldLength(4)]
        public uint MsgLen { get; set; }//msgLen后面实际数据域字段长度

        [FieldOrder(6)]
        [FieldLength(4)]
        public uint Timeout { get; set; }
    }
}
