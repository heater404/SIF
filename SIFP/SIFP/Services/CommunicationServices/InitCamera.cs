using Serilog;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class InitCamera : IInitCamera
    {
        public Tuple<uint, uint> InitIntegrationTimesRange()
        {
            try
            {
                var json = File.ReadAllText(@"Configs\Camera.json");
                return JsonSerializer.Deserialize<Tuple<uint, uint>>(json);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }
            return null;
        }
    }
}
