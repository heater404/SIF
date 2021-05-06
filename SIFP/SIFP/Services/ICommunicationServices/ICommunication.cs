using SIFP.Core.Enums;
using SIFP.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICommunication
    {
        bool Open();
        bool Close();
        bool? ConnectCamera(int millisecondsTimeout);

        bool? DisconnectCamera(int millisecondsTimeout);

        bool? StartStreaming(int millisecondsTimeout);

        bool? StopStreaming(int millisecondsTimeout);

        bool? ConfigCamera(ConfigCameraModel configCamera, int millisecondsTimeout);

        bool? ConfigAlg(ConfigAlgRequest configAlg, int millisecondsTimeout);

        bool? AlgoAddCapture(UInt32 opt, UInt32 pos, UInt32 ID, UInt32 type,
               UInt32 frameNum, UInt32 cycle);

        bool AlgoDelCapture(UInt32 pos, UInt32 ID);

        bool WriteRegs(Register[] regs, DevTypeE devType);

        bool ReadRegs(Register[] regs, DevTypeE devType);

        bool SwitchUserAccess(UserAccessType accessType);

        Task GetSysStatusAsync(CancellationToken cancellationToken, int interval);

        bool? ConfigCorrectionParams(CorrectionParams correction);

        bool? ConfigPostProcParams(PostProcParams postProc);

        bool? ConfigVcselDriver(ConfigVcselDriver vcselDriver);
    }
}
