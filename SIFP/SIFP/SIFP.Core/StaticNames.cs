using SIFP.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core
{
    public static class RegionNames
    {
        public const string MenuRegion = "MenuRegion";
        public const string ToolRegion = "ToolRegion";
        public const string MainRegion = "MainRegion";
        public const string WatchLogRegion = "WatchLogRegion";
        public const string StatusBarRegion = "StatusBarRegion";
    }

    public static class DialogNames
    {
        public const string CaptureDataDialog = "CaptureDataView";
        public const string WaitingDialog = "WaitingView";
        public const string NotificationDialog = "NotificationView";
        public const string VcselDriverDialog = "VcselDriverDialog";
        public const string PasswordDialog = "PasswordDialog";
    }

    public static class ViewNames
    {
        public const string PointCloudView = "PointCloudView";
        public const string RegMapView = "RegMapView";
    }

    public static class EnumsTypes
    {
        public static Array PointCloudTypes { get => Enum.GetValues(typeof(PointCloudTypeE)); }
        public static Array DepthValueTypes { get => Enum.GetValues(typeof(DepthValueTypeE)); }
        public static Array DepthDataTypes { get => Enum.GetValues(typeof(DepthDataTypeE)); }
        public static Array DenoiseLevels { get => Enum.GetValues(typeof(DenoiseLevelE)); }
        public static Array SDenoiseMethods { get => Enum.GetValues(typeof(SDenoiseMethodE)); }
    }
}
