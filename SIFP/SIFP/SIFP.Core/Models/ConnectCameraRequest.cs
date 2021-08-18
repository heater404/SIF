using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIFP.Core.Models
{
    [MsgType(MsgTypeE.ConnectCameraRequestType)]
    public class ConnectCameraRequest : MsgHeader
    {
        [FieldOrder(0)]
        public DevTypeE CameraType { set; get; }

        [FieldLength(4)]//bool类型默认为1字节 true为1 false为0
        [FieldOrder(1)]
        public bool Reset { set; get; }

        //public override uint GetMsgLen()
        //{
        //    return 8;
        //}
    }
}
