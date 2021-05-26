using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IStateMachine
    {
        public void ConfigureConnectedState(Func<Task> entryAction, Action exitAction);
        public void ConfigureDisconnectAction(Action entryAction, Action exitAction);
        public void ConfigureStreamingAction(Action entryAction, Action exitAction);
        public void ConfigureCapturingAction(Action entryAction, Action exitAction);
        public Task Connect();

        public bool CanConnect();

        public void Disconnect();

        public bool CanDisconnect();

        public void StreamingOn();

        public bool CanStreamingOn();

        public void StreamingOff();

        public bool CanStreamingOff();

        public void Capture();

        public bool CanCapture();

        public void CancelCapture();

        public bool CanCancelCapture();
    }
}
