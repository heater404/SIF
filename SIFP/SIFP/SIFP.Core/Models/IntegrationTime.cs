using BinarySerialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class IntegrationTime
    {
        [FieldOrder(1)]
        //integration time for every FRAME.
        //PHASE 1-4 integration time in nano-second
        public UInt32 Phase1_4Int { get; set; }

        [FieldOrder(2)]
        //PHASE 5-8 integration time in nano-second
        public UInt32 Phase5_8Int { get; set; }

        [FieldOrder(3)]
        //special phase integration time in nano-second
        public UInt32 SpecialPhaseInt { get; set; }
    }
}
