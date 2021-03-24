using SIFP.Core.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICommClient
    {
        BlockingCollection<byte[]> RecvDatas { get; set; }
        bool Open();
        bool Close();

        int Send(MsgHeader msg);
    }
}
