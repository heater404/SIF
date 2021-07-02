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
        public UInt32 DoReset { get; set; }//0--false 1--true

        [FieldOrder(2)]
        public StandByModeE StandByMode { get; set; }

        [FieldOrder(3)]
        public UInt32 SysXtalClkKHz { get; set; }

        [FieldOrder(4)]
        public UserCaseModel CurrentUserCase { get; set; }//当前使用的UserCase

        [Ignore]//协议中是不需要这个字段的，但是在初始化的时候需要
        public List<UserCaseModel> UserCases { get; set; }//预定义的所有UserCase集合

        //深度帧帧率 但是协议需要MIPI帧率
        [FieldOrder(5)]
        public UInt32 DepthFPS { get; set; }

        [FieldOrder(6)]
        public SequencerRepeatModeE SequencerRepeatMode { get; set; }

        [FieldOrder(7)]
        public TriggerModeE TriggerMode { get; set; }

        //configure XVS trigger polarity (only used when triggerMode is TRIGGER_MODE_SLAVE_MODE). true: being triggerd on positive level signal
        [FieldOrder(8)]
        [FieldLength(4)]
        public bool SlaveTriggeredOnPosLevel { get; set; }

        [FieldOrder(9)]
        public ROISetting ROISetting { get; set; }

        [FieldOrder(10)]
        public BinningModeE BinningMode { get; set; }

        [FieldOrder(11)]
        public MirrorModeE MirrorMode { get; set; }

        [FieldOrder(12)]
        public TSensorModeE TSensorMode { get; set; }

        [FieldOrder(13)]
        public uint PerformClkChanges { get; set; }

        [FieldOrder(14)]
        public ClkDIvOverride ClkDivOverride { get; set; }

        [FieldOrder(15)]
        public PhaseModeE PhaseMode { get; set; }

        [FieldOrder(16)]
        public UInt32 NeedReply { get; set; }
    }
}
