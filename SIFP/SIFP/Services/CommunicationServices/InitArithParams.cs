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
    public class InitArithParams : IInitArithParams
    {
        public CorrectionParams InitCorrection()
        {
            try
            {
                string json =  File.ReadAllText(@"Configs\CorrectionParamters.json");
                return JsonSerializer.Deserialize<CorrectionParams>(json);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }

            return null;
        }

        public PostProcParams InitPostProc()
        {
            try
            {
                string json = File.ReadAllText(@"Configs\PostProcParameters.json");
                return JsonSerializer.Deserialize<PostProcParams>(json);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }

            return null;
        }
    }
}
