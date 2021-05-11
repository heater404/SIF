using Serilog;
using Services.Interfaces;
using SIFP.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class InitConfigAlg : IInitConfigAlg
    {
        public ConfigAlg Init()
        {
            try
            {
                string json = File.ReadAllText(@"Configs\ConfigAlg.json");
                return JsonSerializer.Deserialize<ConfigAlg>(json);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }

            return null;
        }
    }
}
