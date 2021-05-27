using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class RegisterModel : BindableBase
    {
        private bool isWriteable;
        private bool isReadable;
        private bool isRealTime;
        private string name;
        private UInt16 address;
        private UInt32 regVal;
        private UInt32 lastVal;
        private List<RegBitModel> bits;
        public List<RegBitsRange> Range { get; set; }

        public bool IsWriteable
        {
            get { return isWriteable; }
            private set { isWriteable = value; RaisePropertyChanged(); }
        }

        public bool IsReadable
        {
            get { return isReadable; }
            private set { isReadable = value; RaisePropertyChanged(); }
        }

        public bool IsRealTime
        {
            get { return isRealTime; }
            private set { isRealTime = value; RaisePropertyChanged(); }
        }

        public string Name
        {
            get { return name; }
            private set { name = value; RaisePropertyChanged(); }
        }

        public UInt16 Address
        {
            get { return address; }
            private set { address = value; RaisePropertyChanged(); }
        }

        public UInt32 RegVal
        {
            get { return regVal; }
            set
            {
                //RegValue的值有非法的
                //1、比特位使能为false的值必须为0，并且不需要错误提示
                //2、比特位使能位true的，但是值超范围了的情况，需要错误提示
                foreach (var range in Range)
                {
                    if (range.Min == range.Max)
                        value = MakeInValidBits2Zero(value, range);
                }

                regVal = value;
                RaisePropertyChanged();

                Bits = UpdataBits();
            }
        }

        /// <summary>
        /// 将比特位使为false的值置为0
        /// </summary>
        /// <param name="val">置零前的值</param>
        /// <param name="range">置零范围</param>
        /// <returns></returns>
        private UInt32 MakeInValidBits2Zero(UInt32 val, RegBitsRange range)
        {
            if (range.Max != range.Min)
                return val;

            UInt32 mask = (UInt32)(Math.Pow((double)2, (double)(range.End - range.Start + 1)) - 1) << range.Start;

            return val & ~mask;
        }

        //从chip读到值后或者从配置文件中Load后就需要UpdataLastRegValue
        public UInt32 LastVal
        {
            get { return lastVal; }
            set { lastVal = value; RaisePropertyChanged(); }
        }

        public List<RegBitModel> Bits
        {
            get { return bits; }
            set
            {
                bits = value;
                RaisePropertyChanged();
            }
        }

        public RegisterModel(string name, UInt16 addr, UInt32 defaultVal, List<RegBitsRange> valids, bool isRealTime = true, bool isWriteable = true, bool isReadable = true)
        {
            this.name = name;
            this.address = addr;
            this.regVal = defaultVal;
            this.lastVal = defaultVal;
            this.Range = valids;
            this.isRealTime = isRealTime;
            this.isWriteable = isWriteable;
            this.isReadable = isReadable;
            this.bits = UpdataBits();
        }

        /// <summary>
        /// UpdataBits
        /// </summary>
        /// <returns></returns>
        private List<RegBitModel> UpdataBits()
        {
            List<RegBitModel> bits = new List<RegBitModel>();
            BitArray array = new BitArray(BitConverter.GetBytes(this.regVal));
            for (UInt16 i = 0; i < array.Count; i++)
            {
                bits.Add(new RegBitModel(array[i], i));
            }
            return bits;
        }

        private bool IsRegValueValid(RegBitsRange item, UInt32 val)
        {
            UInt32 mask = (UInt32)(Math.Pow((double)2, (double)(item.End - item.Start + 1)) - 1) << item.Start;
            UInt32 temp = (mask & val) >> item.Start;
            return temp >= item.Min && temp <= item.Max;
        }

        private bool IsRegBitsValid(RegBitsRange item, List<RegBitModel> regBits)
        {
            UInt32 temp = 0;
            for (UInt32 i = item.Start; i < item.End + 1; i++)
            {
                if (regBits[(int)i].Bit)
                    temp = (UInt32)(1 << (Int32)(i - item.Start)) | temp;
            }
            return temp >= item.Min && temp <= item.Min;
        }
    }

    public class RegBitModel : BindableBase
    {
        public RegBitModel(bool b, UInt16 i)
        {
            this.bit = b;
            this.index = i;
        }

        private bool bit;

        public bool Bit
        {
            get { return bit; }
            set
            {
                this.GetType();
                bit = value;
                RaisePropertyChanged();
            }
        }

        private UInt16 index;

        public UInt16 Index
        {
            get { return index; }
            private set { index = value; RaisePropertyChanged(); }
        }
    }

    public class RegBitsRange
    {
        public Byte Start { get; set; }
        public Byte End { get; set; }
        public UInt32 Min { get; set; }
        public UInt32 Max { get; set; }

        public RegBitsRange(Byte start, Byte end, UInt32 min, UInt32 max)
        {
            this.Start = start;
            this.End = end;
            this.Min = min;
            this.Max = max;
        }
    }

    public class RegMapPage : BindableBase
    {
        public string PageName { get; set; }

        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; RaisePropertyChanged(); }
        }
    }
}
