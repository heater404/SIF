using Services.Interfaces;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class StateMachine : IStateMachine
    {
        Stateless.StateMachine<States, Triggers> machine = new Stateless.StateMachine<States, Triggers>(States.Disconnected);

        public States CurrentState
        {
            get { return machine.State; }
        }

        public StateMachine()
        {
            machine.Configure(States.Disconnected)
                .Permit(Triggers.Connect, States.Connecting);

            machine.Configure(States.Connected)
                .Permit(Triggers.Disconnect, States.Disconnected)
                .Permit(Triggers.StreamingOn, States.Streaming);

            machine.Configure(States.Streaming)
                .Permit(Triggers.StreamingOff, States.Connected)
                .Permit(Triggers.Disconnect, States.Disconnected)
                .Permit(Triggers.Capture, States.Capturing);

            machine.Configure(States.Capturing)
                .Permit(Triggers.CancelCapture, States.Streaming);



            string graph = Stateless.Graph.UmlDotGraph.Format(machine.GetInfo());
            Debug.WriteLine(graph);
        }

        public void Connect()
        {
            machine.Fire(Triggers.Connect);
        }

        public void Disconnect()
        {
            machine.Fire(Triggers.Disconnect);
        }

        public void StreamingOn()
        {
            machine.Fire(Triggers.StreamingOn);
        }

        public void StreamingOff()
        {
            machine.Fire(Triggers.StreamingOff);
        }

        public void Capture()
        {
            machine.Fire(Triggers.Capture);
        }

        public void CancelCapture()
        {
            machine.Fire(Triggers.CancelCapture);
        }
    }
}
