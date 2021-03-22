using BinarySerialization;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class ConfigCameraModel
    {
        [FieldOrder(1)]
        public UInt32 DoReset { get; set; }

        [FieldOrder(2)]
        public StandByModeE StandByMode { get; set; }

        [FieldOrder(3)]
        public UInt32 SysXtalClkKHz { get; set; }

        [FieldOrder(4)]
        public WorkModeE WorkMode { get; set; }

        [FieldOrder(5)]
        public SubWorkModeE SubWorkMode { get; set; }

        [FieldOrder(6)]
        [FieldCount(4)]
        public SubFrameModeE[] SubFrameModes { get; set; } 

        [FieldOrder(7)]
        [FieldCount(4)]
        public SpecialFrameModeE[] SpecialFrameModes { get; set; }
        [FieldOrder(8)]
        public UInt32 DifferentialBG { get; set; }

        [FieldOrder(9)]
        public FrameSeqSchedule FrameSeqSchedule { get; set; }

        [FieldOrder(10)]
        [FieldCount(4)]
        public IntegrationTime[] IntegrationTimes { get; set; }

        [FieldOrder(11)]
        [FieldCount(4)]
        public PLLDLLDiv[] PLLDLLDivs { get; set; }

        [FieldOrder(12)]
        [FieldCount(4)]
        public UInt32[] NumSubFramePerFrame { get; set; }

        [FieldOrder(13)]
        public UInt32 NumDepthSequencePerDepthMap { get; set; }

        [FieldOrder(14)]
        public MIPI_FS_FE_PosE MIPI_FS_FE_Pos { get; set; }

        [FieldOrder(15)]
        public UInt32 MIPIFrameRate { get; set; }

        [FieldOrder(16)]
        public SequencerRepeatModeE SequencerRepeatMode { get; set; }

        [FieldOrder(17)]
        public TriggerModeE TriggerMode { get; set; }

        [FieldOrder(18)]
        public ROISetting ROISetting { get; set; }

        [FieldOrder(19)]
        public BinningModeE BinningMode { get; set; }

        [FieldOrder(20)]
        public MirrorModeE MirrorMode { get; set; }

        [FieldOrder(21)]
        public TSensorModeE TSensorMode { get; set; }

        [FieldOrder(22)]
        public uint PerformClkChanges { get; set; }

        [FieldOrder(23)]
        public ClkDIvOverride ClkDivOverride { get; set; }
    }
}
