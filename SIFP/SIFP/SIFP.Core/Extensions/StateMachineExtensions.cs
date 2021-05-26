using Prism.Commands;
using SIFP.Core.Enums;
using Stateless;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Extensions
{
    public static class StateMachineExtensions
    {
        public static DelegateCommand CreateCommand(this StateMachine<States, Triggers> machine, Triggers trigger)
        {
            return new DelegateCommand(() => machine.Fire(trigger), () => machine.CanFire(trigger));
        }
    }
}
