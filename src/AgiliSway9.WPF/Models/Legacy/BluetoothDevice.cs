using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgiliSway9.WPF.Models.Legacy
{
    public enum BluetoothDeviceType
    {
        WiiDevice,
        AccuswayPlus
    }

    public class BluetoothDevice
    {
        public string BluetoothAddress;
        public string HIDPath;
        public string HIDSerial;
        public string COMPort;
        public string Name;
        public bool Connected = false;
        public BluetoothDeviceType Type; // Nintendo vs ASP vs OTHER
        public object Device;
    }
}
