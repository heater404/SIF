using Services.Interfaces;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Services
{
    public class StateMachine : Stateless.StateMachine<States, Triggers>, IStateMachine, INotifyPropertyChanged
    {
        public StateMachine() : base(States.Disconnected)
        {
            OnTransitioned(t =>
            {
                OnPropertyChanged("State");
                CommandManager.InvalidateRequerySuggested();
            });

            OnTransitioned(t =>
            {
                Debug.WriteLine("State Machine transitioned from {0} -> {1} [{2}]", t.Source, t.Destination, t.Trigger);
            });
        }

        public void ConfigureConnectedState(Func<Task> entryAction, Action exitAction)
        {
            this.Configure(States.Connected)
                .OnEntryAsync(entryAction)
                .OnExit(exitAction)
                .Permit(Triggers.Disconnect, States.Disconnected)
                .Permit(Triggers.StreamingOn, States.Streaming);
        }

        public void ConfigureDisconnectAction(Action entryAction, Action exitAction)
        {
            this.Configure(States.Disconnected)
                .OnEntry(entryAction)
                .OnExit(exitAction)
                .Permit(Triggers.Connect, States.Connected);
        }

        public void ConfigureStreamingAction(Action entryAction, Action exitAction)
        {
            this.Configure(States.Streaming)
                .OnEntry(entryAction)
                .OnExit(exitAction)
                .Permit(Triggers.StreamingOff, States.Connected)
                .Permit(Triggers.Disconnect, States.Disconnected)
                .Permit(Triggers.Capture, States.Capturing);
        }

        public void ConfigureCapturingAction(Action entryAction, Action exitAction)
        {
            this.Configure(States.Capturing)
                .OnEntry(entryAction)
                .OnExit(exitAction)
                .Permit(Triggers.CancelCapture, States.Streaming);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public Task Connect()
        {
           return this.FireAsync(Triggers.Connect);
        }

        public void Disconnect()
        {
            this.Fire(Triggers.Disconnect);
        }

        public void StreamingOn()
        {
            this.Fire(Triggers.StreamingOn);
        }

        public void StreamingOff()
        {
            this.Fire(Triggers.StreamingOff);
        }

        public void Capture()
        {
            this.Fire(Triggers.Capture);
        }

        public void CancelCapture()
        {
            this.Fire(Triggers.CancelCapture);
        }

        public bool CanConnect()
        {
            return this.CanFire(Triggers.Connect);
        }

        public bool CanDisconnect()
        {
            return this.CanFire(Triggers.Disconnect);
        }

        public bool CanStreamingOn()
        {
            return this.CanFire(Triggers.StreamingOff);
        }

        public bool CanStreamingOff()
        {
            return this.CanFire(Triggers.StreamingOff);
        }

        public bool CanCapture()
        {
            return this.CanFire(Triggers.Capture);
        }

        public bool CanCancelCapture()
        {
            return this.CanFire(Triggers.CancelCapture);
        }
    }
}
