using ConfigCamera.Views;
using ConfigCorrection.Views;
using ConfigPostProc.Views;
using System;

namespace SIFP
{
    public static class ConfigViewTypes
    {
        public static readonly Type ConfigCameraView = typeof(ConfigCameraView);
        public static readonly Type ConfigCorrectionView = typeof(ConfigCorrectionView);
        public static readonly Type ConfigPostProcView = typeof(ConfigPostProcView);
    }
}
