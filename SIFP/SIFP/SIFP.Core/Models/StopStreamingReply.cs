using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class StopStreamingReply:MsgHeader
    {
        public override uint GetMsgLen()
        {
            return 0;
        }
    }
}
