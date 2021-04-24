using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IInitCamera
    {
        Tuple<UInt32, UInt32> InitIntegrationTimesRange();
    }
}
