﻿using SIFP.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIFP.Core.Enums
{
    public enum UserAccessType
    {
        Normal,
        Expert
    }

    public enum RegOperateType
    {
        Write = 0,
        Read,
        WriteRead
    }

    public enum DevTypeE : UInt32
    {
        TOF = 0u,
        RGB,
        TOF_RGB,
        VDRIVER,
        ISP1,
        ISP2,
    }

    public enum StandByModeE : UInt32
    {
        None = 0,
    }

    public enum WorkModeE : UInt32
    {
        SINGLE_FREQ = 0,

        DOUBLE_FREQ = 1,

        TRIPLE_FREQ = 2,

        QUAD_FREQ=3,

        BG_OUTDOOR=7,

        GRAY_ONLY =8,
    }

    public enum SubWorkModeE : UInt32
    {
        [SubWorkMode(WorkModeE.SINGLE_FREQ)]
        _4PHASE = 0u,

        [SubWorkMode(WorkModeE.SINGLE_FREQ)]
        _4PHASE_GRAY,

        [SubWorkMode(WorkModeE.SINGLE_FREQ)]
        _4PHASE_BG,

        [SubWorkMode(WorkModeE.SINGLE_FREQ)]
        _4PHASE_4BG,

        [SubWorkMode(WorkModeE.SINGLE_FREQ)]
        _4PHASE_GRAY_5BG,

        [SubWorkMode(WorkModeE.SINGLE_FREQ)]
        GRAY_BG_4PHASE_GRAY_BG,

        [SubWorkMode(WorkModeE.DOUBLE_FREQ)]
        _4PHASE_4PHASE,

        [SubWorkMode(WorkModeE.DOUBLE_FREQ)]
        _4PHASE_GRAY_4PHASE_BG,//for calibration usecase only

        [SubWorkMode(WorkModeE.DOUBLE_FREQ)]
        _4PHASE_4BG_4PHASE_4BG,

        [SubWorkMode(WorkModeE.DOUBLE_FREQ)]
        _4PHASE_GRAY_5BG_4PHASE_GRAY_5BG,

        [SubWorkMode(WorkModeE.TRIPLE_FREQ)]
        _4PHASE_4PHASE_4PHASE,

        [SubWorkMode(WorkModeE.TRIPLE_FREQ)]
        _4PHASE_GRAY_4PHASE_GRAY_4PHASE_BG,

        [SubWorkMode(WorkModeE.QUAD_FREQ)]
        _4PHASE_4PHASE_4PHASE_4PHASE,

        [SubWorkMode(WorkModeE.QUAD_FREQ)]
        _4PHASE_GRAY_4PHASE_BG_4PHASE_GRAY_4PHASE_BG,

        [SubWorkMode(WorkModeE.BG_OUTDOOR)]
        BG_OUTDOOR=97,

        [SubWorkMode(WorkModeE.GRAY_ONLY)]
        GRAY_ONLY=98,
    }

    public enum SubFrameModeE : UInt32
    {
        //subframeMode, must be set to value that satisfies workMode's needs.
        /// normal 4 phase per subframe (0,90,180,270)
        Mode_4Phase = 0,

        /// 2 phase per subframe (0,180)
        Mode_2Phase,

        /// 2x 4phase, with 2 sets of parameters
        Mode_8Phase,

        /// 2x 2phase, with 2 sets of parameters
        Mode_2Phase_2Phase,

        /// only 1 special phase
        Mode_1SpecialPhase,

        /// SUBFRAME_MODE_4 + 1 special phase
        Mode_5Phase,

        /// SUBFRAME_MODE_8 + 1 special phase
        Mode_9Phase,

        /// SUBFRAME_MODE_2 + 1 special phase
        Mode_3Phase,
    }

    public enum SpecialFrameModeE : UInt32
    {
        Normal = 0,
        Gray,
        Bg,
    }

    public enum MIPI_FS_FE_PosE : UInt32
    {
        SubFrame = 0,
        Phase,
        Frame,
        DepthMap,
    }

    public enum PhaseModeE:UInt32
    {
        PhaseMode1,
        PhaseMode2,
        PhaseMode4,
        PhaseMode8,
        PhaseMode16,
    }

public enum SequencerRepeatModeE : UInt32
    {
        //not auto repeating, after triggering, only perform 1 depth map
        Non_Auto = 0,

        //auto repeat, after triggering, repeat forever
        Auto_Repeat,
    }

    public enum TriggerModeE : UInt32
    {
        //slave triggering mode, rely on XVS signal to triger a sequence
        Slave_Mode = 0,

        //master mode, rely on sequence_go bit to trigger a sequence (SI_PLF_MSG_START_STREAMING will do it)
        Master_Mode,
    }

    public enum BinningModeE : UInt32
    {
        None = 0,
        //average two adjacent pixels in one column
        Analog,

        //average two adjacent pixels in one row
        Digital,

        //average 4 pixels
        _2X2,

        _4X4,
    }

    public enum TSensorModeE : UInt32
    {
        None = 0,
        EveryPhase,
        EverySubFrame,
    }

    public enum MirrorModeE : UInt32
    {
        None = 0,
        Vertical,
        Horizontal,
        Both,
    }

    public enum ConfigCameraReplyE : UInt32
    {
        Success=0,
        InvalidArgs,
        InvalidResolution,
        InvalidWokeMode,
        InternalError,
        DisconnectStatus=99,
    }
}
