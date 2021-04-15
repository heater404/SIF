using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace License
{
    public static class HMACSHA256Extend
    {
        public static string ComputeHash(string data, string key)
        {
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hashBytes = hmac.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes).Trim();
        }
    }
}
