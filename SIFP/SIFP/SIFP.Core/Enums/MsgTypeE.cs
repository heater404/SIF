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

        WriteRegisterRequestType=0x123,
        WriteRegisterReplyType=0x124,

        ReadRegisterRequestType=0x125,
        ReadRegisterReplyType=0x126,

        LensArgsRequestType=0x12A,
        LensArgsReplyType=0x12B,

        DisconnectCameraRequestType =0x12E,

        StartStreamingRequestType=0x12f,

        StopStreamingRequestType=0x130,
        StopStreamingReplyType=0x136,

        UserAccessRequestType=0x131,

        GetSysStatusRequestType=0x132,
        GetSysStatusReplyType=0x133,

        ConfigArithParamsRequestType=0x137,//用于下发算法参数
        ConfigArithParamsReplyType = 0x138,//主要用与主动上报AE的结果

        ConfigVcselDriverRequestType=0x139,//用于配置VcselDriver,里面的内容还需定义
        ConfigVcselDriverReplyType =0x140,//暂时没有用到

        DisconnectCameraReplyType=0x141,
    }
}
