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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegMap.ViewModels
{
    public class RegMapViewModel : RegionViewModelBase
    {
        private ICommunication comm;
        public RegMapViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, ICommunication communication) : base(regionManager, eventAggregator)
        {
            this.comm = communication;

            ReadRegisterCmd = new DelegateCommand(ReadRegister);
            WriteRegisterCmd = new DelegateCommand(WriteRegister);

            this.EventAggregator.GetEvent<ReadRegisterReplyEvent>().Subscribe(reply =>
            {
                foreach (var item in reply.ConfigRegister.Regs)
                {
                    if (item.RegAddr == this.regAddr)
                        this.RegValue = item.RegValue.ToString("X2");
                }
            },true);
        }

        private void WriteRegister()
        {
            comm.WriteRegs(new Register[] { new Register { RegAddr = this.regAddr,RegValue=this.regValue } },DevTypeE.TOF);
        }

        private void ReadRegister()
        {
            comm.ReadRegs(new Register[] { new Register { RegAddr = this.regAddr, RegValue = this.regValue } }, DevTypeE.TOF);
        }

        private UInt32 regAddr=0xc02;
        public string RegAddr
        {
            get { return regAddr.ToString("X2"); }
            set
            {
                try
                {
                    regAddr = Convert.ToUInt32(value, 16);

                    RaisePropertyChanged();
                }
                catch (ArgumentException)
                {
                    throw;
                }
            }
        }

        private UInt32 regValue;
        public string RegValue
        {
            get { return regValue.ToString("X2"); }
            set
            {
                try
                {
                    regValue = Convert.ToUInt32(value, 16);
                    RaisePropertyChanged();
                }
                catch (ArgumentException)
                {
                    throw;
                }
            }
        }

        public DelegateCommand ReadRegisterCmd { get; set; }
        public DelegateCommand WriteRegisterCmd { get; set; }
    }
}
