using BinarySerialization;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class ConfigCameraReply : MsgHeader
    {
        [FieldOrder(1)]
        public ConfigCameraReplyE ConfigAck { get; set; }

        [FieldOrder(2)]
        /// number of MIPI frames in a single SI_PLF_MSG_IMAGE_DATA_REPLY_t or UVC transaction
        public UInt32 NumMIPIFrames { get; set; }

        [FieldOrder(3)]
        //number of phase frames in a MIPI frame (determines the expected data size per MIPI frame)
        public UInt32 NumPhaseFramePerMIPIFrame { get; set; }

        [FieldOrder(4)]
        //output image width (with consideration of ROI and Binning)
        public UInt32 OutImageWidth { get; set; }

        [FieldOrder(5)]
        //output image height (with consideration of ROI and Binning)
        public UInt32 OutImageHeight { get; set; }

        [FieldOrder(6)]
        public UInt32 AddInfoLines { get; set; }
    }
}
