using BinarySerialization;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    [MsgType(MsgTypeE.ConfigArithParamsRequestType)]
    public class ConfigArithParamsRequest : MsgHeader
    {
        public override uint GetMsgLen()
        {
            return 160;
        }

        [FieldOrder(1)]
        public CorrectionParams Correction { get; set; }

        [FieldOrder(2)]
        public PostProcParams PostProc { get; set; }

        // The following flags can be both 1 at the same time
        // use(1) above procParams to configure correction algorithm or not(0)
        [FieldOrder(3)]
        public UInt32 UseCorrParams { get; set; }
        // use(1) above ppParams to configure post processing algorithm or not(0)
        [FieldOrder(4)]
        public UInt32 UsePostProcParams { get; set; }

    }
}
