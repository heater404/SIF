using SIFP.Core.Enums;
using SIFP.Core.Models;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IInitCamera
    {
        Tuple<double, double> InitIntegrationTimesRange();

        List<ComboBoxViewMode<UInt32>> InitFrequencies();

        ConfigCameraModel InitConfigCamera(SubWorkModeE subWorkMode);
    }
}
