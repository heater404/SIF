using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SubWorkModeAttribute : Attribute
    {
        public WorkModeE WorkModeType { get; set; }
        public UInt32 NumPhasePerDepthMap { get; set; }

        public UInt32 NumDepthMapPerDepth { get; set; }
        public bool IsAsync { get; set; }//4BG和前一个SubFrame的积分同步
        public SubWorkModeAttribute(WorkModeE type, UInt32 numPhasePerDepthMap, UInt32 numDepthMapPerDepth, bool isAsync = false)
        {
            this.WorkModeType = type;
            this.NumPhasePerDepthMap = numPhasePerDepthMap;
            this.NumDepthMapPerDepth = numDepthMapPerDepth;
            this.IsAsync = isAsync;
        }
    }
}
