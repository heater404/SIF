using SIFP.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICommClient
    {
        bool Open();
        bool Close();

        Task<int> SendAsync(MsgHeader msg);
    }
}
