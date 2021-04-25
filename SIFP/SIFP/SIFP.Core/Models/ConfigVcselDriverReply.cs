using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class ConfigVcselDriverReply : MsgHeader
    {
        public UInt32 Ack { get; set; }//0-success  1-fail
    }
}
