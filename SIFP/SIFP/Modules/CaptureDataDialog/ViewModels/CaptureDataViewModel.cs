using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Services.Interfaces;
using SIFP.Core.Enums;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CaptureDataDialog.ViewModels
{
    public class CaptureDataViewModel : RegionViewModelBase, IDialogAware
    {
        private CaptureDataTypeE captureDataType = CaptureDataTypeE.Raw;
        public CaptureDataTypeE CaptureDataType
        {
            get { return captureDataType; }
            set { captureDataType = value; RaisePropertyChanged(); }
        }

        private UInt32 captureDataNum = 10;
        public UInt32 CaptureDataNum
        {
            get { return captureDataNum; }
            set { captureDataNum = value; RaisePropertyChanged(); }
        }

        public List<ComboBoxItemMode<CaptureDataTypeE>> CaptureDataTypes { get; private set; } = new List<ComboBoxItemMode<CaptureDataTypeE>>();
        public List<ComboBoxItemMode<UInt32>> CaptureDataNums { get; private set; } = new List<ComboBoxItemMode<UInt32>>();


        private ICommunication comm;
        public CaptureDataViewModel(ICommunication communication, IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            this.comm = communication;
            foreach (CaptureDataTypeE item in Enum.GetValues(typeof(CaptureDataTypeE)))
            {
                CaptureDataTypes.Add(new ComboBoxItemMode<CaptureDataTypeE> { Description = item.ToString(), SelectedModel = item, IsShow = Visibility.Visible });
            }

            foreach (UInt32 item in new UInt32[] { 1, 5, 10, 20, 30, 50, 100, 200, 300, 0xFFFFFFFF })
            {
                if (item == 0xffffffff)
                    CaptureDataNums.Add(new ComboBoxItemMode<UInt32> { Description = "continue", SelectedModel = item, IsShow = Visibility.Visible });
                else
                    CaptureDataNums.Add(new ComboBoxItemMode<UInt32> { Description = item.ToString(), SelectedModel = item, IsShow = Visibility.Visible });
            }

            CaptureOkCmd = new DelegateCommand(CaptureOK);
            CaptureCancelCmd = new DelegateCommand(CaptureCancel);
        }

        private void CaptureCancel()
        {
            this.RequestClose.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        private async void CaptureOK()
        {
            this.RequestClose.Invoke(new DialogResult(ButtonResult.OK));

            UInt32 type = (UInt32)(0 << 10)
                                | (UInt32)((1 << 9)
                                | (UInt32)((CaptureDataType == CaptureDataTypeE.Raw ? 0 : 1) << 8)
                                | (UInt32)CaptureDataType);

            var res = await Task.Run(() => comm.AlgoAddCapture((UInt32)CaptureOpt.AddToLoacl, (UInt32)CapturePosition.Pos, (UInt32)CaptureID.ID, type, captureDataNum, 1));

            if (res.HasValue)
            {
                if (res.Value)
                {
                    this.PrintNoticeLog("Capture Success", LogLevel.Warning);
                    this.PrintWatchLog("Capture Success", LogLevel.Warning);
                }
                else
                {
                    this.PrintNoticeLog("Capture Fail", LogLevel.Error);
                    this.PrintWatchLog("Capture Fail", LogLevel.Error);
                }
            }
            else
            {
                this.PrintNoticeLog("Capture Timeout", LogLevel.Error);
                this.PrintWatchLog("Capture Timeout", LogLevel.Error);

                this.EventAggregator.GetEvent<CaptureReplyEvent>().Publish(null);
            }
        }

        #region IDialogAware
        public event Action<IDialogResult> RequestClose;
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
        public string Title { get; set; } = "CaptureData";
        #endregion

        public DelegateCommand CaptureOkCmd { get; private set; }
        public DelegateCommand CaptureCancelCmd { get; private set; }


    }
}
