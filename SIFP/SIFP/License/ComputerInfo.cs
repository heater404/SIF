using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace License
{
    public class ComputerInfo
    {
        static Dictionary<string, ManagementObjectCollection> managementObjectDict = new Dictionary<string, ManagementObjectCollection>();

        static ComputerInfo()
        {
            var names = Enum.GetNames(typeof(WmiType));
            foreach (string name in names)
            {
                managementObjectDict.Add(name, new ManagementObjectSearcher("SELECT * FROM " + name).Get());
            }
        }

        public static string GetComputerInfo()
        {
            string info = string.Empty;
            string hardDisk = GetHardDiskNumber().Trim();
            string cpu = GetCPUNumber().Trim();
            string memory = GetMemoryNumber().Trim();
            string board = GetBaseBoardInfo().Trim();

            info = string.Concat(hardDisk, cpu, memory, board);
            return info;
        }
        private static string GetHardDiskNumber()
        {
            var query = managementObjectDict[WmiType.Win32_LogicalDisk.ToString()];

            string result = string.Empty;
            foreach (var obj in query)
            {
                result = obj["VolumeSerialNumber"].ToString();
                break;
            }

            return result;
        }

        private static string GetCPUNumber()
        {
            var query = managementObjectDict[WmiType.Win32_Processor.ToString()];

            string result = string.Empty;
            foreach (var obj in query)
            {
                result = obj["Processorid"].ToString();
                break;
            }

            return result;
        }

        private static string GetMemoryNumber()
        {
            var query = managementObjectDict[WmiType.Win32_PhysicalMemory.ToString()];

            string result = string.Empty;
            foreach (var obj in query)
            {
                result = obj["PartNumber"].ToString();
                break;
            }
            return result;
        }

        private static string GetBaseBoardInfo()
        {
            var query = managementObjectDict[WmiType.Win32_BaseBoard.ToString()];

            string result = string.Empty;
            foreach (var obj in query)
            {
                result = obj["SerialNumber"].ToString();
                break;
            }
            return result;
        }
    }

    internal enum WmiType
    {
        Win32_Processor,
        Win32_PerfFormattedData_PerfOS_Memory,
        Win32_PhysicalMemory,
        Win32_NetworkAdapterConfiguration,
        Win32_LogicalDisk,
        Win32_BaseBoard
    }
}
