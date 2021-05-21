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
        public bool IsAsync { get; set; }//4BG和前一个SubFrame的积分同步
        public SubWorkModeAttribute(WorkModeE type)
        {
            this.WorkModeType = type;
        }
    }
}
