using BinarySerialization;
using SIFP.Core.Enums;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class UserCaseModel
    {
        [FieldOrder(1)]
        [FieldLength(4)]
        public WorkModeE WorkMode { get; set; }

        [FieldOrder(2)]
        [FieldLength(4)]
        public SubWorkModeE SubWorkMode { get; set; }

        [FieldOrder(3)]
        [FieldCount(4)]
        [FieldLength(16)]
        public SubFrameModeE[] SubFrameModes { get; set; }

        [FieldOrder(4)]
        [FieldCount(4)]
        [FieldLength(16)]
        public SpecialFrameModeE[] SpecialFrameModes { get; set; }

        //when specialFrameMode is SPECIAL_FRAME_MODE_BG, use this field to choose between normal BG(0) or differential BG mode(1). 
        //Keep this to 0 when specialFrameMode is other option.
        [FieldOrder(5)]
        [FieldLength(4)]
        public UInt32 DifferentialBG { get; set; }

        [FieldOrder(6)]
        public FrameSeqSchedule FrameSeqSchedule { get; set; }

        [FieldOrder(7)]
        [FieldCount(4)]
        public IntegrationTime[] IntegrationTimes { get; set; }

        [FieldOrder(8)]
        [FieldCount(4)]
        public PLLDLLDiv[] PLLDLLDivs { get; set; }

        [FieldOrder(9)]
        [FieldCount(4)]
        public UInt32[] NumSubFramePerFrame { get; set; }

        //switch between pulse light and always-on light for gray frame
        //1--pulse light  0--always-on light
        [FieldOrder(10)]
        [Ignore]
        public UInt32 EnableLedMod { get; set; }

        [FieldOrder(11)]
        public UInt32 NumDepthSequencePerDepthMap { get; set; }

        [FieldOrder(12)]
        public MIPI_FS_FE_PosE MIPI_FS_FE_Pos { get; set; }

        //number of phase frames in a MIPI frame (determines the expected data size per MIPI frame)
        [Ignore]
        public UInt32 NumPhasePerFrameStruct { get; set; }

        [Ignore]
        public UInt32 MaxFPS { get; set; }
    }
}
