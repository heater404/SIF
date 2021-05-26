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
        public void Fire(Triggers trigger);
        public bool CanFire(Triggers trigger);
    }
}
