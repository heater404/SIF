using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateLicense
{
    public class CreateLicense
    {
        public static bool Create(string data, string key, string filename)
        {
            try
            {
                string hash = License.VerityLicense.ComputeHash(data, key);
                Console.WriteLine(hash);
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    sw.Write(hash);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return true;
        }
    }
}
