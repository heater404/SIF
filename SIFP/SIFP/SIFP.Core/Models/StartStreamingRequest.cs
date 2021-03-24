using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    [MsgType(MsgTypeE.StartStreamingRequestType)]
    public class StartStreamingRequest:MsgHeader
    {
        public override uint GetMsgLen()
        {
            return 0;
        }
    }
}
