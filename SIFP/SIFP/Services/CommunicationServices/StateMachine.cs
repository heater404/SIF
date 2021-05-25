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
        Stateless.StateMachine<State, Trigger> machine = new Stateless.StateMachine<State, Trigger>(State.Disconnected);

        public State CurrentState
        {
            get { return machine.State; }
        }

        public StateMachine()
        {
            machine.Configure(State.Disconnected)
                .Permit(Trigger.Connect, State.Connected);

            machine.Configure(State.Connected)
                .Permit(Trigger.Disconnect, State.Disconnected)
                .Permit(Trigger.StreamingOn, State.Streaming);

            machine.Configure(State.Streaming)
                .Permit(Trigger.StreamingOff, State.Connected)
                .Permit(Trigger.Disconnect, State.Disconnected)
                .Permit(Trigger.Capture, State.Capturing);

            machine.Configure(State.Capturing)
                .Permit(Trigger.CancelCapture, State.Streaming);

            string graph = Stateless.Graph.UmlDotGraph.Format(machine.GetInfo());
            Debug.WriteLine(graph);
        }

        public void Connect()
        {
            machine.Fire(Trigger.Connect);
        }

        public void Disconnect()
        {
            machine.Fire(Trigger.Disconnect);
        }

        public void StreamingOn()
        {
            machine.Fire(Trigger.StreamingOn);
        }

        public void StreamingOff()
        {
            machine.Fire(Trigger.StreamingOff);
        }

        public void Capture()
        {
            machine.Fire(Trigger.Capture);
        }

        public void CancelCapture()
        {
            machine.Fire(Trigger.CancelCapture);
        }
    }
}
