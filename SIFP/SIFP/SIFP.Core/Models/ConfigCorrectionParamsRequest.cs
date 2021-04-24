using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    [MsgType(MsgTypeE.ConfigArithParamsRequestType)]
    public class ConfigCorrectionParamsRequest : MsgHeader
    {
        public override uint GetMsgLen()
        {
            return 23 * 4+8;
        }

        [FieldOrder(1)]
        public UInt32 Reserve1 = 2;

        [FieldOrder(2)]
        public UInt32 Reserve2 = 0;

        [FieldOrder(3)]
        public CorrectionParams Correction { get; set; }
    }
}
