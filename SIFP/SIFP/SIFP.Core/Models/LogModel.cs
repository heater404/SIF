using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class LogModel
    {
        public LogModel(string msg, LogLevel lev)
        {
            this.Time = DateTime.Now;
            this.Message = msg;
            this.Lev = lev;
        }

        public DateTime Time { get; private set; }
        
        public string Message { get;private set; }

        public LogLevel Lev { get;private set; }

        public override string ToString()
        {
            return $"{this.Time:HH:mm:ss} > {this.Message}";
        }
    }
}
