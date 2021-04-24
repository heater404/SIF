using BinarySerialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class PostProcParams
    {
        [FieldOrder(1)]
        public PostProcOutPut OutPutParams { get; set; } = new PostProcOutPut();

        [FieldOrder(2)]
        public Denoising DenoisingParams { get; set; } = new Denoising();

        [FieldOrder(3)]
        public Repair RepairParams { get; set; } = new Repair();

        [FieldOrder(4)]
        public AntiInterference AntiInterferenceParams { get; set; } = new AntiInterference();

        [FieldOrder(5)]
        public Confidence ConfidenceParams { get; set; } = new Confidence();
    }

    public class PostProcOutPut
    {
        [FieldOrder(1)]
        [FieldLength(4)]
        public bool OutPointCloud { get; set; } = false;//bool类型默认为1字节 true为1 false为0

        [FieldOrder(2)]
        [FieldLength(4)]
        public bool OutConfidence { get; set; } = false;

        [FieldOrder(3)]
        [FieldLength(4)]
        public bool OutFlag { get; set; } = true;

        [FieldOrder(4)]
        public PointCloudTypeE OutPointCloudType { get; set; } = PointCloudTypeE.PC32F;

        [FieldOrder(5)]
        public DepthValueTypeE OutDepthValueType { get; set; } = DepthValueTypeE.XYR;

        [FieldOrder(6)]
        public DepthDataTypeE OutDepthDataType { get; set; } = DepthDataTypeE.UINT16;
    }

    public enum DenoiseLevelE
    {
        Level0 = 0,//off
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
        Level4 = 4,
        Level5 = 5,
    }

    public enum SDenoiseMethodE
    {
        MF=0,
        BF=1,
        NLM=2,
    }

    public class Denoising
    {
        [FieldOrder(1)]
        public DenoiseLevelE TDenoiseLevel { get; set; } = DenoiseLevelE.Level3;

        [FieldOrder(2)]
        public DenoiseLevelE SDenoiseLevel { get; set; } = DenoiseLevelE.Level3;

        [FieldOrder(3)]
        public SDenoiseMethodE SDenoiseMethod { get; set; } = SDenoiseMethodE.NLM;
    }

    public class Repair
    {
        [FieldOrder(1)]
        [FieldLength(4)]
        public bool DeFlyPixel { get; set; } = true;

        [FieldOrder(2)]
        [FieldLength(4)]
        public bool DeHoles { get; set; } = false;
    }

    public class AntiInterference
    {
        [FieldOrder(1)]
        [FieldLength(4)]
        public bool AntiALI { get; set; } = false;

        [FieldOrder(2)]
        [FieldLength(4)]
        public bool AntiMCI { get; set; } = false;
    }

    public class Confidence
    {
        [FieldOrder(1)]
        public UInt32 ValidDistMin { get; set; } = 150;//最小有效距离，单位 mm  150 0~65535, step: 1

        [FieldOrder(2)]
        public UInt32 ValidDistMax { get; set; } = 4000;//4000 0~65535, step: 1 (注：应保证ValidDistMinMM ValidDistMaxMM)
    }
}
