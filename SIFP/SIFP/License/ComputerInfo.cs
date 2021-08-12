using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                var vsn = obj["VolumeSerialNumber"];
                if (null != vsn)
                    result = vsn.ToString();
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
                var pid = obj["Processorid"];
                if (null != pid)
                    result = pid.ToString();
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
                var pn = obj["PartNumber"];
                if (null != pn)
                    result = pn.ToString();
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
                var sn = obj["SerialNumber"];
                if (null != sn)
                    result = sn.ToString();
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
