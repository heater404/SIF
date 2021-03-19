﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
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
    public class CaptureDataViewModel : ViewModelBase, IDialogAware
    {
        private CaptureDataTypeE captureDataType=CaptureDataTypeE.Raw;
        public CaptureDataTypeE CaptureDataType
        {
            get { return captureDataType; }
            set { captureDataType = value; RaisePropertyChanged(); }
        }

        private UInt32 captureDataNum=10;
        public UInt32 CaptureDataNum
        {
            get { return captureDataNum; }
            set { captureDataNum = value;RaisePropertyChanged(); }
        }

        public List<ComboBoxItemMode<CaptureDataTypeE>> CaptureDataTypes { get; private set; } = new List<ComboBoxItemMode<CaptureDataTypeE>>();
        public List<ComboBoxItemMode<UInt32>> CaptureDataNums { get; private set; } = new List<ComboBoxItemMode<UInt32>>();
        public CaptureDataViewModel()
        {
            foreach (CaptureDataTypeE item in Enum.GetValues(typeof(CaptureDataTypeE)))
            {
                CaptureDataTypes.Add(new ComboBoxItemMode<CaptureDataTypeE> { Description = item.ToString(), SelectedModel = item, IsShow = Visibility.Visible });
            }

            foreach (UInt32 item in new UInt32[] { 1, 5, 10, 20, 30, 50, 100, 150, 200, 300 })
            {
                CaptureDataNums.Add(new ComboBoxItemMode<UInt32> { Description = item.ToString(), SelectedModel = item, IsShow = Visibility.Visible });
            }

            CaptureOkCmd = new DelegateCommand(CaptureOK);
            CaptureCancelCmd = new DelegateCommand(CaptureCancel);
        }

        private void CaptureCancel()
        {
            this.RequestClose.Invoke(null);
        }

        private void CaptureOK()
        {
            //todo:capture

            this.RequestClose.Invoke(null);
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