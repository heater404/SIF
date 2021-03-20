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
    }
}
