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
                .Ignore(Triggers.Disconnect)
                .Permit(Triggers.Connect, States.Connected);

            this.Configure(States.Connected)
                .PermitReentry(Triggers.ConfigVcselDriver)
                .Permit(Triggers.ConfigCamera, States.CameraConfiged)
                .Permit(Triggers.Disconnect, States.Disconnected);

            this.Configure(States.CameraConfiged)
                .PermitReentry(Triggers.ConfigVcselDriver)
                .PermitReentry(Triggers.ConfigCamera)
                .Permit(Triggers.Disconnect,States.Disconnected)
                .Permit(Triggers.StreamingOn, States.Streaming);

            this.Configure(States.Streaming)
                .PermitReentry(Triggers.ConfigVcselDriver)
                .PermitReentry(Triggers.ConfigCamera)
                .Ignore(Triggers.CancelCapture)
                .Permit(Triggers.StreamingOff, States.CameraConfiged)
                .Permit(Triggers.Disconnect, States.Disconnected)
                .Permit(Triggers.Capture, States.Capturing);

            this.Configure(States.Capturing)
                .PermitReentry(Triggers.ConfigCamera)
                .PermitReentry(Triggers.ConfigVcselDriver)
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
