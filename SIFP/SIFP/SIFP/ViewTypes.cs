using ConfigAlg.Views;
using ConfigCamera.Views;
using PointCloud.Views;
using RegMap.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIFP
{
    public static class ViewTypes
    {
        public readonly static Type ConfigCameraType = typeof(ConfigCameraView);
        public readonly static Type ConfigAlgType = typeof(ConfigAlgView);
        public readonly static string PointCloudType = typeof(PointCloudView).Name;
        public readonly static string RegMapType = typeof(RegMapView).Name;

    }
}
