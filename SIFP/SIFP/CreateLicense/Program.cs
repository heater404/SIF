using System;
using System.Diagnostics;

namespace CreateLicense
{
    class Program
    {
        static void Main(string[] args)
        {
            string data = "";
            string key = "ToF_2610_2021";
            string filename = "ToFDemo_License.lic";

            try
            {
                data = args[0];
                key = args[1];
                filename = args[2];
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

           Console.WriteLine( CreateLicense.Create(data, key, filename));
        }
    }
}
