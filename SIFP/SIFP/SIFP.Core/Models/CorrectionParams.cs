using BinarySerialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class CorrectionParams
    {
        [FieldOrder(1)]
        public CorrOutPut OutPutParams { get; set; } = new CorrOutPut();

        [FieldOrder(2)]
        public Correction CorrParams { get; set; } = new Correction();

        [FieldOrder(3)]
        public Fusion FusionParams { get; set; } = new Fusion();

        [FieldOrder(4)]
        public Others OthersParams { get; set; } = new Others();
    }

    public class CorrOutPut
    {
        [FieldOrder(1)]
        [FieldLength(4)]
        public bool OutPointCloud { get; set; } = false;//bool类型默认为1字节 true为1 false为0

        [FieldOrder(2)]
        [FieldLength(4)]
        public bool OutConfidence { get; set; } = false;

        [FieldOrder(3)]
        public PointCloudTypeE OutPointCloudType { get; set; } = PointCloudTypeE.PC32F;

        [FieldOrder(4)]
        public DepthValueTypeE OutDepthValueType { get; set; } = DepthValueTypeE.XYR;

        [FieldOrder(5)]
        public DepthDataTypeE OutDepthDataType { get; set; } = DepthDataTypeE.UINT16;
    }
    public enum PointCloudTypeE
    {
        PC32F = 0,
        Depth_PointCloud = 1,
    }
    public enum DepthValueTypeE
    {
        XYR = 0,
        XYZ = 1,
    }
    public enum DepthDataTypeE
    {
        UINT16 = 0,
        DEPTH16 = 1,
    }

    public class Correction
    {
        [FieldOrder(1)]
        [FieldLength(4)]
        public bool CorrBP { get; set; } = true;

        [FieldOrder(2)]
        [FieldLength(4)]
        public bool CorrLens { get; set; } = true;

        [FieldOrder(3)]
        [FieldLength(4)]
        public bool CorrTemp { get; set; } = true;

        [FieldOrder(4)]
        [FieldLength(4)]
        public bool CorrOffsetAuto { get; set; } = true;

        [FieldOrder(5)]
        [FieldLength(4)]
        public bool CorrFPPN { get; set; } = true;

        [FieldOrder(6)]
        [FieldLength(4)]
        public bool CorrWig { get; set; } = true;

        [FieldOrder(7)]
        [FieldLength(4)]
        public bool CorrFPN { get; set; } = true;

        [FieldOrder(8)]
        [FieldLength(4)]
        public bool FillInvalidPixels { get; set; } = true;

        [FieldOrder(9)]
        [FieldLength(4)]
        public bool CutInvalidPixels { get; set; } = true;

        [FieldOrder(10)]
        [FieldLength(4)]
        public bool CorrOffsetManual { get; set; } = false;

        [FieldOrder(11)]
        [FieldLength(4)]
        public Int32 F1CorrOffset { get; set; } = 0;//F1 手动 Offset 校正量，单位 mm -32768~32767, step: 1

        [FieldOrder(12)]
        [FieldLength(4)]
        public Int32 F2CorrOffset { get; set; } = 0;//F1 手动 Offset 校正量，单位 mm -32768~32767, step: 1

        [FieldOrder(13)]
        [FieldLength(4)]
        public Int32 F3CorrOffset { get; set; } = 0;//F1 手动 Offset 校正量，单位 mm -32768~32767, step: 1

        [FieldOrder(14)]
        [FieldLength(4)]
        public Int32 F4CorrOffset { get; set; } = 0;//F1 手动 Offset 校正量，单位 mm -32768~32767, step: 1
    }

    public class Fusion
    {
        [FieldOrder(1)]
        [FieldLength(4)]
        public bool SFDeAliasing { get; set; } = true;//单频深度去混叠开关

        [FieldOrder(2)]
        public UInt32 PresetMaxDist { get; set; } = 7000;//最大预置距离单位mm 0~65535, step: 1 7000

        //Detect white board and fix abnormal points
        [FieldOrder(3)]
        [FieldLength(4)]
        public bool DetectWB { get; set; } = false;
    }

    public class Others
    {
        [FieldOrder(1)]
        [FieldLength(4)]
        public bool AE { get; set; } = false;

        [FieldOrder(2)]
        [FieldLength(4)]
        public bool AntiAliCorr { get; set; } = false;
    }
}
