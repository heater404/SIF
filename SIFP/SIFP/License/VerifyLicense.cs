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
            string keyStr = HMACSHA256Extend.ComputeHash(ComputerInfo.GetComputerInfo(), shakey);

            return licenseStr == keyStr;
        }
    }
}
