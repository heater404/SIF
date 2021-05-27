using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Services.Interfaces;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RegMap.ViewModels
{
    public class RegMapViewModel : RegionViewModelBase
    {
        private ICommunication comm;
        public RegMapViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, ICommunication communication) : base(regionManager, eventAggregator)
        {
            this.comm = communication;
            InitRegisters();
            InitCmds();
            Messenger.Default.Register<RegStruct[]>(this, "RecvRegs", RecvRegs);
        }
        #region Field

        private readonly RegMapServer rms = new RegMapServer();
        private List<RegisterModel> registers = new List<RegisterModel>();
        private string configFilePath = "ConifgFilePath";
        private string path = @"Config/RegMap2610.xml";
        private bool canUserOperate = true;
        private bool isLoading = false;
        private List<UInt32> selectedRegsAddr = new List<UInt32>();
        private List<RegOperateStruct> configRegs = null;
        #endregion Field

        #region Property

        public string ConfigFilePath
        {
            get { return configFilePath; }
            set { configFilePath = value; RaisePropertyChanged(); }
        }

        public List<RegisterModel> Registers
        {
            get { return registers; }
            set { registers = value; RaisePropertyChanged(); }
        }

        public bool IsLoading
        {
            get { return isLoading; }
            set { isLoading = value; RaisePropertyChanged(); }
        }

        public IEnumerable<int> Title
        {
            get { return Enumerable.Range(0, 32).Reverse(); }
        }

        public List<RegMapPage> MapPages { get; set; } = new List<RegMapPage>();

        #endregion Property

        #region Commands

        public DelegateCommand<string> GroupCmd { get; set; }
        public DelegateCommand<TextChangedEventArgs> QueryCmd { get; set; }
        public DelegateCommand<object> ModifyBitsCmd { get; set; }
        public DelegateCommand ReadAllRegCmd { get; set; }
        public DelegateCommand WriteAllRegCmd { get; set; }
        public DelegateCommand SaveAllRegCmd { get; set; }
        public DelegateCommand ReadSelectedRegsCmd { get; set; }
        public DelegateCommand WriteSelectedRegsCmd { get; set; }
        public DelegateCommand OpenConfigRegCmd { get; set; }
        public DelegateCommand WriteConfigRegCmd { get; set; }
        public DelegateCommand<IList<RegisterModel>> SelectionChangedCmd { get; set; }

        #endregion Commands

        #region Methods

        private void InitCmds()
        {
            GroupCmd = new DelegateCommand<string>(GroupChoose);
            QueryCmd = new DelegateCommand<TextChangedEventArgs>(Query);
            ModifyBitsCmd = new DelegateCommand<object>(ModifyBits);
            ReadAllRegCmd = new DelegateCommand(ReadAllReg, CanUserOperate);
            WriteAllRegCmd = new DelegateCommand(WriteAllReg, CanUserOperate);
            SaveAllRegCmd = new DelegateCommand(SaveAllReg, CanUserOperate);
            ReadSelectedRegsCmd = new DelegateCommand(ReadSelectedRegs);
            WriteSelectedRegsCmd = new DelegateCommand(WriteSelectedRegs);
            OpenConfigRegCmd = new DelegateCommand(LoadConfigReg, CanUserOperate);
            WriteConfigRegCmd = new DelegateCommand(WriteConfigReg, CanUserOperate);
            SelectionChangedCmd = new DelegateCommand<IList<RegisterModel>>(SelectionChanged);
        }

        private void SelectionChanged(IList<RegisterModel> args)
        {
            //todo  addItems into a list and RemoveItems removed from list
            //foreach (RegisterModel remove in args.RemovedItems)
            //{
            //    selectedRegsAddr.Remove(remove.Address);
            //}
            //foreach (RegisterModel add in args.AddedItems)
            //{
            //    selectedRegsAddr.Add(add.Address);
            //}
            selectedRegsAddr.Clear();
            foreach (RegisterModel item in args)
            {
                selectedRegsAddr.Add(item.Address);
            }

        }

        private void GroupChoose(string pageName)
        {
            FetchOnePage(pageName);
        }

        private bool CanUserOperate()
        {
            return canUserOperate;
        }

        private async void WriteConfigReg()
        {
            if (!File.Exists(ConfigFilePath))
                return;
            canUserOperate = false;
            await Task.Run(() =>
            {
                foreach (var reg in configRegs)
                {
                    //只有有写操作的时候才会将寄存器的值在界面上更新。只读的时候不更新
                    //更新regval的操作放在每一个操作之前。
                    if (reg.OperateType == RegOperateType.Write || reg.OperateType == RegOperateType.WriteRead)
                    {
                        rms[reg.Register.RegAddr] = new Tuple<uint, uint>(rms[reg.Register.RegAddr].Item1, rms[reg.Register.RegAddr].Item1); //需要更新LastVal
                        rms[reg.Register.RegAddr] = new Tuple<uint, uint>(reg.Register.RegValue, rms[reg.Register.RegAddr].Item2); //再更新RegVal 
                    }

                    switch (reg.OperateType)
                    {
                        case RegOperateType.Write:
                            rms.WriteRegs(new UInt32[] { reg.Register.RegAddr }); break;

                        case RegOperateType.Read:
                            rms.ReadRegs(new UInt32[] { reg.Register.RegAddr }); break;

                        case RegOperateType.WriteRead:
                            rms.WriteRegs(new UInt32[] { reg.Register.RegAddr });
                            Thread.Sleep(100);
                            rms.ReadRegs(new UInt32[] { reg.Register.RegAddr });
                            break;
                    }
                    Thread.Sleep(100);
                }
            });
            canUserOperate = true;
            WriteAllRegCmd.RaiseCanExecuteChanged();
        }

        private void LoadConfigReg()
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "json files|*.json"
            };
            bool? result = ofd.ShowDialog();

            if (result.Value != true)
                return;

            ConfigFilePath = ofd.FileName;

            Task.Run(() =>
            {
                string script = File.ReadAllText(ConfigFilePath);

                try
                {
                    configRegs = JsonSerializer.Deserialize<List<RegOperateStruct>>(script);
                    /*将此处的更新regval放在写每一个寄存器之前，
                     * 因为在这里更新regval，如果脚本中有相同地址的reg，那么上一个相同地址的regval会被覆盖
                     */
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"An error occurs on line {ex.LineNumber} in {Path.GetFileName(ConfigFilePath)}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        private void WriteSelectedRegs()
        {
            if (rms == null)
                return;

            rms.WriteRegs(selectedRegsAddr.ToArray<UInt32>());

            rms.ReadRegs(selectedRegsAddr.ToArray<UInt32>());
        }

        private void ReadSelectedRegs()
        {
            if (null == rms)
                return;

            rms.ReadRegs(selectedRegsAddr.ToArray<UInt32>());
        }

        private async void SaveAllReg()
        {
            if (null == rms)
                return;

            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "json files|*.json"
            };
            bool? result = sfd.ShowDialog();

            if (result.Value != true)
                return;

            await Task.Run(() => rms.SaveAllRegs(sfd.FileName));

            MessageBox.Show("Save All Regs Completed");
        }

        private async void WriteAllReg()
        {
            canUserOperate = false;
            await Task.Run(() =>
            {
                var regs = rms.GetAllCanWriteAddrs();
                foreach (var reg in regs)
                {
                    rms.WriteRegs(reg.ToArray());
                }
            });
            canUserOperate = true;//设置为True后 canExecute不会立刻执行，所以按钮还是Disable状态
            WriteAllRegCmd.RaiseCanExecuteChanged();//触发CanExecuteChanged()后会立马刷新，但是好像其他命令不需要执行，执行其中一个就可以了
        }

        private async void ReadAllReg()
        {
            canUserOperate = false;
            await Task.Run(() =>
            {
                var regs = rms.GetAllCanReadAddrs();
                foreach (var reg in regs)
                {
                    rms.ReadRegs(reg.ToArray());
                }
            });
            canUserOperate = true;
            ReadAllRegCmd.RaiseCanExecuteChanged();
        }

        /// <summary>
        //  从Chip中读到值或者从配置文件中Load到值后 需要更新LastVal
        /// </summary>
        /// <param name="regs"></param>
        private void RecvRegs(RegStruct[] regs)
        {
            if (null == rms)
                return;

            foreach (var reg in regs)
            {
                rms[reg.RegAddr] = new Tuple<uint, uint>(rms[reg.RegAddr].Item1, rms[reg.RegAddr].Item1);//更新LastVal
                rms[reg.RegAddr] = new Tuple<uint, uint>(reg.RegValue, rms[reg.RegAddr].Item2);//更新RegVal
                                                                                             //因为socket send之后会立马recv。但是Send的日志时间不是在send之后立马执行的，recv是立马执行的。
                                                                                             //只要从日志的打印来看感觉是先Recv的
                                                                                             //为了避免该情况 在recv的日志这里做了延时，具体延时多少随缘
                Thread.Sleep(60);
                //WatchLog.PrintWatchLog($"Recv One Reg=>[0x{reg.RegAddr:X4}:0x{rms[reg.RegAddr].Item1:X8}]", LogType.Warning);
            }
        }

        private void ModifyBits(object param)
        {
            Type type = param.GetType();
            UInt16 addr = (UInt16)type.GetProperty("Addr").GetValue(param);
            UInt16 index = (UInt16)type.GetProperty("Index").GetValue(param);

            rms.UpdateRegValue(addr, index);
        }

        private void Query(TextChangedEventArgs args)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(Registers);
            TextBox textBox = args.OriginalSource as TextBox;

            if (view != null)
            {
                view.Filter =
                    obj =>
                    {
                        if (obj is RegisterModel reg)
                        {
                            string addrUpper = "0X" + reg.Address.ToString("X4");
                            if (addrUpper.Contains(textBox.Text) || addrUpper.ToLower().Contains(textBox.Text))
                                return true;
                        }

                        return false;
                    };
            }
        }

        private async void InitRegisters()
        {
            if (null == rms)
                return;
            IsLoading = true;
            await Task.Run(
               () => rms.GetData(path));

            InitMapPages();

            FetchOnePage(MapPages[0].PageName);
            IsLoading = false;
        }

        private void InitMapPages()
        {
            if (null == rms)
                return;

            string[] pages = rms.GetPages();

            for (int i = 0; i < pages.Length; i++)
            {
                MapPages.Add(new RegMapPage { IsChecked = i == 0, PageName = pages[i] });
            }
        }

        private async void FetchOnePage(string page)
        {
            if (null == rms)
                return;
            IsLoading = true;
            await Task.Run(() => Registers = rms.GetOnePageRegMap(page));
            IsLoading = false;
        }

        #endregion Methods
    }
}
