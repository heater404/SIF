using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace License
{
    public static class VerityLicense
    {
        public static bool VerifyLicense(string filename, string shakey)
        {
            string licenseFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @$"\{filename}";
            string licenseStr = "";
            try
            {
                using (StreamReader sr = new StreamReader(licenseFilePath))
                {
                    licenseStr = sr.ReadToEnd();
                }
            }
            catch (FileNotFoundException ex)
            {
                Debug.WriteLine($"{ex}");
                return false;
            }
            string keyStr = ComputeHash(ComputerInfo.GetComputerInfo(), shakey);

            return licenseStr == keyStr;
        }

        public static string ComputeHash(string data, string key)
        {
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hashBytes = hmac.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes).Trim();
        }
    }
}
