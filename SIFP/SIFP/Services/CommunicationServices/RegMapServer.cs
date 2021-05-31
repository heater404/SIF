using Serilog;
using Services.Interfaces;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Services
{
    public class RegMapServer
    {
        private readonly Dictionary<string, List<RegisterModel>> registers = new Dictionary<string, List<RegisterModel>>();
        private readonly ICommunication comm;
        public const Int32 MaxRWLength = 10;
        public RegMapServer(ICommunication communication)
        {
            this.comm = communication;
        }
        public Tuple<UInt32, UInt32> this[UInt32 addr]//[value,LastValue]
        {
            get
            {
                RegisterModel model = GetRegisterByAddr(addr);
                if (null != model)
                    return new Tuple<uint, uint>(model.RegVal, model.LastVal);
                else
                {
                    Log.Error($"Error Address:0x{addr:X8}");
                    return new Tuple<uint, uint>(0, 0);
                }
            }
            set
            {
                List<RegisterModel> models = GetRegistersByAddr(addr);
                foreach (var model in models)
                {
                    if (null != model)
                    {
                        model.RegVal = value.Item1;
                        model.LastVal = value.Item2;
                    }
                    else
                    {
                        Log.Error($"Error Address:0x{addr:X8}");
                    }
                }
            }
        }

        public List<List<UInt32>> GetAllCanReadAddrs()
        {
            List<List<UInt32>> addrs = new List<List<UInt32>>();
            foreach (var register in registers.Values)
            {
                List<UInt32> addr = new List<uint>();
                foreach (var reg in register)
                {
                    if (reg.IsReadable)
                        addr.Add(reg.Address);
                }
                addrs.Add(addr);
            }
            return addrs;
        }

        public List<List<UInt32>> GetAllCanWriteAddrs()
        {
            List<List<UInt32>> addrs = new List<List<UInt32>>();
            foreach (var register in registers.Values)
            {
                List<UInt32> addr = new List<uint>();
                foreach (var reg in register)
                {
                    if (reg.IsWriteable)
                        addr.Add(reg.Address);
                }
                addrs.Add(addr);
            }
            return addrs;
        }

        public string[] GetPages()
        {
            return registers.Keys.ToArray<string>();
        }

        public List<RegisterModel> GetOnePageRegMap(string page)
        {
            return registers[page];
        }

        public RegisterModel GetRegisterByAddr(UInt32 addr)
        {
            RegisterModel model = null;

            foreach (var item in registers.Values)
            {
                model = item.Find(t => t.Address == addr);
                if (model != null)
                    return model;
            }

            return model;
        }

        public List<RegisterModel> GetRegistersByAddr(UInt32 addr)
        {
            List<RegisterModel> models = new List<RegisterModel>();

            foreach (var item in registers.Values)
            {
                models.AddRange(item.FindAll(t => t.Address == addr));
            }

            return models;
        }

        public bool IsWriteableAddr(UInt32 addr)
        {
            RegisterModel model = GetRegisterByAddr(addr);
            if (null != model)
                return model.IsWriteable;
            return false;
        }

        public List<RegBitModel> GetRegBitsByAddr(UInt32 addr)
        {
            RegisterModel model = GetRegisterByAddr(addr);
            if (null != model)
                return model.Bits;
            return null;
        }

        public void UpdateRegValue(UInt32 addr, UInt16 offset)
        {
            List<RegisterModel> models = GetRegistersByAddr(addr);
            foreach (var model in models)
            {
                BitArray bitArray = new BitArray(BitConverter.GetBytes(model.RegVal));
                bitArray.Set(offset, !bitArray[offset]);

                byte[] val = new byte[4];
                bitArray.CopyTo(val, 0);

                model.RegVal = BitConverter.ToUInt32(val, 0);
            }
        }

        public bool IsReadableAddr(UInt32 addr)
        {
            RegisterModel model = GetRegisterByAddr(addr);
            if (null != model)
                return model.IsReadable;
            return false;
        }

        /// <summary>
        /// 多个寄存器一起读写的时候，他们肯定属于同一page
        /// 使用脚本读写的时候，都是单个的。
        /// 所以可以通过这一组寄存器地址判断DeviceType
        /// </summary>
        /// <param name="regs"></param>
        /// <returns></returns>
        public DevTypeE GetRegDeviceType(RegStruct[] regs)
        {
            foreach (var register in registers)
            {
                if (register.Value.Exists(r => r.Address == regs[0].RegAddr))
                {
                    if (register.Key == "VDriver")
                        return DevTypeE.VDRIVER;
                    else
                        return DevTypeE.TOF;
                }
            }
            return DevTypeE.TOF;
        }

        public bool ReadRegs(UInt32[] addrs, int maxRWLength = MaxRWLength)
        {
            List<RegStruct> regs = new List<RegStruct>();
            for (int i = 0; i < addrs.Length; i++)
            {
                if (IsReadableAddr(addrs[i]))
                    regs.Add(new RegStruct { RegAddr = addrs[i], RegValue = 0 });
            }

            while (regs.Count > 0)
            {
                int len = regs.Count <= maxRWLength ? regs.Count : maxRWLength;

                if (this.ReadRegs(regs.Take(len).ToArray()))
                    regs = regs.Skip(len).ToList();
                else
                    return false;
            }

            return false;
        }

        private bool ReadRegs(RegStruct[] regs)
        {
            if (comm.ReadRegs(regs, GetRegDeviceType(regs)))
            {
                {
                    string msg = string.Empty;
                    foreach (var reg in regs)
                    {
                        msg += $"[0x{reg.RegAddr:x4}] ";
                    }

                    //WatchLog.PrintWatchLog($"Read Regs : {msg}", LogType.Info);
                }
                return true;
            }

            return false;
        }

        public bool WriteRegs(UInt32[] addrs, int maxRWLength = MaxRWLength)
        {
            List<RegStruct> regs = new List<RegStruct>();
            for (int i = 0; i < addrs.Length; i++)
            {
                if (IsWriteableAddr(addrs[i]))
                    regs.Add(new RegStruct { RegAddr = addrs[i], RegValue = this[(UInt16)(addrs[i])].Item1 });
            }

            while (regs.Count > 0)
            {
                int len = regs.Count <= maxRWLength ? regs.Count : maxRWLength;

                if (this.WriteRegs(regs.Take(len).ToArray<RegStruct>()))
                    regs = regs.Skip(len).ToList();
                else
                    return false;
            }

            return false;
        }

        private bool WriteRegs(RegStruct[] regs)
        {
            if (comm.WriteRegs(regs, GetRegDeviceType(regs)))
            {
                {
                    string msg = string.Empty;
                    foreach (var reg in regs)
                    {
                        msg += $"[0x{reg.RegAddr:X4}:0x{reg.RegValue:X8}] ";
                    }

                    //WatchLog.PrintWatchLog($"Write Regs : {msg}", LogType.Info);
                }

                return true;
            }

            return false;
        }

        public void SaveAllRegs(string path)
        {
            List<RegOperateStruct> script = new List<RegOperateStruct>();
            foreach (var register in registers.Values)
            {
                foreach (var reg in register)
                {
                    RegOperateStruct regOperateStruct = new RegOperateStruct
                    {
                        OperateType = reg.IsWriteable ? RegOperateType.Write : RegOperateType.Read,
                        Register = new RegStruct { RegAddr = reg.Address, RegValue = reg.RegVal },
                    };
                    script.Add(regOperateStruct);
                }
            }

            File.WriteAllText(path, JsonSerializer.Serialize(script));
        }

        public void GetData(string path)
        {
            try
            {
                IEnumerable<XElement> root = XElement.Load(path).Elements();

                foreach (var rt in root)
                {
                    List<RegisterModel> list = new List<RegisterModel>();
                    var elements = from ele in rt.Elements("Reg")
                                   select ele;

                    foreach (var item in elements)
                    {
                        List<RegBitsRange> validations = new List<RegBitsRange>();
                        foreach (var ele in item.Element("Ranges").Elements("Range"))
                        {
                            RegBitsRange valid = new RegBitsRange
                                (
                                  Convert.ToByte(ele.Element("Start").Value),
                                  Convert.ToByte(ele.Element("End").Value),
                                  Convert.ToUInt32(ele.Element("Min").Value, 16),
                                  Convert.ToUInt32(ele.Element("Max").Value, 16)
                                );
                            validations.Add(valid);
                        }

                        RegisterModel model = new RegisterModel
                            (
                              item.Attribute("Name").Value,
                              Convert.ToUInt16(item.Element("Address").Value, 16),
                              Convert.ToUInt32(item.Element("RegVal").Value, 16),
                              validations,
                              Convert.ToBoolean(item.Element("IsRealTime").Value),
                              Convert.ToBoolean(item.Element("IsWriteable").Value),
                              Convert.ToBoolean(item.Element("IsReadable").Value)
                            );
                        list.Add(model);
                    }
                    registers.Add(rt.Name.ToString(), list);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex,"GetRegMap");
            }
        }
    }
}
