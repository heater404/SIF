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
        public States CurrentState { get; }

        public void Connect();

        public void Disconnect();

        public void StreamingOn();

        public void StreamingOff();

        public void Capture();

        public void CancelCapture();
    }
}
