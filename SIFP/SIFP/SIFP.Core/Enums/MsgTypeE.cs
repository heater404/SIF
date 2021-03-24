using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIFP.Core.Enums
{
    public enum MsgTypeE : UInt32
    {
        HelloRequestMsgType=0x10,
        CaptureRequestType=0x11b,
        CaptureReplyType=0x11c,
        
        ConfigAlgRequestType=0x119,
        ConfigAlgReplyType=0x11A,

        ConnectCameraRequestType =0x11f,
        ConnectCameraReplyType=0x120,

        ConfigCameraRequestType=0x121,
        ConfigCameraReplyType=0x122,

        DisconnectCameraRequestType =0x12E,

        StartStreamingRequestType=0x12f,

        StopStreamingRequestType=0x130,
        StopStreamingReplyType=0x136,
    }
}
