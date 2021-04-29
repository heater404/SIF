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
        public UserCaseModel UserCase { get; set; } 

        [FieldOrder(5)]
        public UInt32 MIPIFrameRate { get; set; }

        [FieldOrder(6)]
        public SequencerRepeatModeE SequencerRepeatMode { get; set; }

        [FieldOrder(7)]
        public TriggerModeE TriggerMode { get; set; }

        [FieldOrder(8)]
        public ROISetting ROISetting { get; set; }

        [FieldOrder(9)]
        public BinningModeE BinningMode { get; set; }

        [FieldOrder(10)]
        public MirrorModeE MirrorMode { get; set; }

        [FieldOrder(11)]
        public TSensorModeE TSensorMode { get; set; }

        [FieldOrder(12)]
        public uint PerformClkChanges { get; set; }

        [FieldOrder(13)]
        public ClkDIvOverride ClkDivOverride { get; set; }
    }
}
