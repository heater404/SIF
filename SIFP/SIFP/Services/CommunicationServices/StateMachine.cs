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
        public event PropertyChangedEventHandler PropertyChanged;

        public StateMachine() : base(States.Disconnected)
        {
            this.Configure(States.Disconnected)
                .Permit(Triggers.Connect, States.Connected);

            this.Configure(States.Connected)
                .Permit(Triggers.Disconnect, States.Disconnected)
                .Permit(Triggers.StreamingOn, States.Streaming);

            this.Configure(States.Streaming)
                .Permit(Triggers.StreamingOff, States.Connected)
                .Permit(Triggers.Disconnect, States.Disconnected)
                .Permit(Triggers.Capture, States.Capturing);

            this.Configure(States.Capturing)
                .Permit(Triggers.CancelCapture, States.Streaming);

            OnTransitioned(t =>
            {
                OnPropertyChanged("State");
                CommandManager.InvalidateRequerySuggested();
                Debug.WriteLine($"State Machine transitioned from {t.Source} -> { t.Destination} [{t.Trigger}]");
            });
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public new void Fire(Triggers trigger)
        {
            base.Fire(trigger);
        }

        public new bool CanFire(Triggers trigger)
        {
            return base.CanFire(trigger);
        }
    }
}
