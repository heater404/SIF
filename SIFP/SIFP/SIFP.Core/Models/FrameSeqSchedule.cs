using BinarySerialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Models
{
    public class FrameSeqSchedule
    {
        [FieldOrder(1)]
        //the first frame num in a depth map, value starts from 0 to 3
        public byte Slot0FrameNum { get; set; }

        [FieldOrder(2)]
        //the second frame num in a depth map, value starts from 0 to 3
        public byte Slot1FrameNum { get; set; }

        [FieldOrder(3)]
        //the third frame num in a depth map, value starts from 0 to 3
        public byte Slot2FrameNum { get; set; }

        [FieldOrder(4)]
        //the fourth frame num in a depth map, value starts from 0 to 3
        public byte Slot3FrameNum { get; set; }
    }
}
