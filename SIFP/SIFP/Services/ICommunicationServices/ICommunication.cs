using SIFP.Core.Models;
using System;
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

        bool? ConfigCamera(ConfigCameraRequest configCamera, int millisecondsTimeout);
    }
}
