using BinarySerialization;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class UserCaseModel
    {
        [FieldOrder(1)]
        public WorkModeE WorkMode { get; set; }

        [FieldOrder(2)]
        public SubWorkModeE SubWorkMode { get; set; }

        [FieldOrder(3)]
        [FieldCount(4)]
        public SubFrameModeE[] SubFrameModes { get; set; }

        [FieldOrder(4)]
        [FieldCount(4)]
        public SpecialFrameModeE[] SpecialFrameModes { get; set; }
        [FieldOrder(5)]
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

        [FieldOrder(10)]
        public UInt32 NumDepthSequencePerDepthMap { get; set; }

        [FieldOrder(11)]
        public MIPI_FS_FE_PosE MIPI_FS_FE_Pos { get; set; }
    }
}
