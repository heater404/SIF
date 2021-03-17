using BinarySerialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class PLLDLLDiv 
    {
        [FieldOrder(1)]
        //PHASE 1-4 modulation freqeuncy in kHz
        public UInt32 Phase1_4Div { get; set; }

        [FieldOrder(2)]
        //PHASE 5-8 modulation freqeuncy in kHz
        public UInt32 Phase5_8Div { get; set; }

        [FieldOrder(3)]
        //special phase  modulation freqeuncy in kHz
        public UInt32 SpecialPhaseDiv { get; set; }
    }
}
