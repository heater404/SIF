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
        public SubFrameModeE[] SubFrameModes { get; set; } = new SubFrameModeE[4];

        [FieldOrder(7)]
        public SpecialFrameModeE[] SpecialFrameModes { get; set; } = new SpecialFrameModeE[4];

        [FieldOrder(8)]
        public UInt32 DifferentialBG { get; set; }

        [FieldOrder(9)]
        public FrameSeqSchedule FrameSeqSchedule { get; set; }

        [FieldCount(4)]
        [FieldOrder(10)]
        public IntegrationTime[] IntegrationTimes { get; set; }

        [FieldLength(11)]
        [FieldCount(4)]
        public PLLDLLDiv[] PLLDLLDivs { get; set; }

        [FieldLength(12)]
        [FieldCount(4)]
        public UInt32[] NumSubFramePerFrame { get; set; }

        [FieldLength(13)]
        public UInt32 NumDepthSequencePerDepthMap { get; set; }

        [FieldLength(14)]
        public MIPI_FS_FE_PosE MIPI_FS_FE_Pos { get; set; }

        [FieldLength(15)]
        public UInt32 MIPIFrameRate { get; set; }

        [FieldLength(16)]
        public SequencerRepeatModeE SequencerRepeatMode { get; set; }

        [FieldLength(17)]
        public TriggerModeE TriggerMode { get; set; }

        [FieldLength(18)]
        public ROISetting ROISetting { get; set; }

        [FieldLength(19)]
        public BinningModeE BinningMode { get; set; }

        [FieldLength(20)]
        public MirrorModeE MirrorMode { get; set; }

        [FieldLength(21)]
        public TSensorModeE TSensorMode { get; set; }

        [FieldLength(22)]
        public uint PerformClkChanges { get; set; }

        [FieldLength(23)]
        public ClkDIvOverride ClkDivOverride { get; set; }
    }
}
