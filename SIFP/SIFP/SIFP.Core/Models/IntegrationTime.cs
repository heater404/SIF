using BinarySerialization;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class IntegrationTime: ViewModelBase
    {
       
        //integration time for every FRAME.
        //PHASE 1-4 integration time in nano-second
        [Ignore]
        private UInt32 phase1_4Int;
        [FieldOrder(1)]
        public UInt32 Phase1_4Int 
        {
            get { return phase1_4Int; }
            set { phase1_4Int = value;RaisePropertyChanged(); }
        }

        [Ignore]
        //PHASE 5-8 integration time in nano-second
        private UInt32 phase5_8Int;
        [FieldOrder(2)]
        public UInt32 Phase5_8Int 
        {
            get { return phase5_8Int; }
            set { phase5_8Int = value;RaisePropertyChanged(); }
        }

        [Ignore]
        //special phase integration time in nano-second
        private UInt32 specialPhaseInt;
        [FieldOrder(3)]
        public UInt32 SpecialPhaseInt 
        {
            get { return specialPhaseInt; }
            set { specialPhaseInt = value;RaisePropertyChanged(); }
        }
    }
}
