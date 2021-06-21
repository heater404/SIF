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
        public void Subscribe();
        bool? ConnectCamera(int millisecondsTimeout);

        bool? DisconnectCamera(int millisecondsTimeout);

        bool? StartStreaming(int millisecondsTimeout);

        bool? StopStreaming(int millisecondsTimeout);

        ConfigCameraReplyE? ConfigCamera(ConfigCameraModel configCamera, int millisecondsTimeout);

        bool? ConfigAlg(ConfigAlgRequest configAlg, int millisecondsTimeout);

        bool? AlgoAddCapture(UInt32 opt, UInt32 pos, UInt32 ID, UInt32 type,
               UInt32 frameNum, UInt32 cycle);

        bool AlgoDelCapture(UInt32 pos, UInt32 ID);

        bool WriteRegs(RegStruct[] regs, DevTypeE devType);

        bool ReadRegs(RegStruct[] regs, DevTypeE devType);

        bool SwitchUserAccess(UserAccessType accessType);

        Task GetSysStatusAsync(CancellationToken cancellationToken, int interval);

        bool? ConfigArithParams(CorrectionParams correction,PostProcParams postProc);

        bool? ConfigVcselDriver(ConfigVcselDriver vcselDriver);

        bool? GetLensArgs(int millisecondsTimeout);

        public bool Detect(int times);
    }
}
