using System;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICommunication
    {
        bool Open();
        bool Close();
        Task<bool?> ConnectCameraAsync(int millisecondsTimeout);
    }
}
