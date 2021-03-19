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
            return (UInt32)Marshal.SizeOf(this.GetType()) - HeaderLen;
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
        public uint PktSN { get; set; }

        //更改为总的包个数
        [FieldOrder(2)]
        public uint TotalMsgNum { get; set; }

        [FieldOrder(3)]
        public uint MsgSn { get; set; }

        [FieldOrder(4)]
        public MsgTypeE MsgType { get; set; }

        [FieldOrder(5)]
        public uint MsgLen { get; set; }//msgLen后面实际数据域字段长度

        [FieldOrder(6)]
        public uint Timeout { get; set; }
    }
}
