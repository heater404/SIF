using BinarySerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIFP.Core.Models
{
    public abstract class MsgHeader
    {
        // pkt Header
        [FieldOrder(1)]
        public uint PktSN { get; set; }

        //更改为总的包个数
        [FieldOrder(2)]
        public uint TotalMsgNum { get; set; }

        [FieldOrder(3)]
        public uint MsgSn { get; set; }

        [FieldOrder(4)]
        public uint MsgType { get; set; }

        [FieldOrder(5)]
        public uint MsgLen { get; set; }//msgLen后面实际数据域字段长度

        [FieldOrder(6)]
        public uint Timeout { get; set; }
    }
}
