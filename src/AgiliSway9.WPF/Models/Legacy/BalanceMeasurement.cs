using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgiliSway9.WPF.Models.Legacy
{
    public class BalanceMeasurement
    {
        public long Ticks;
        public float X;
        public float Y;

        public BalanceValues RawValues;
        public BalanceValues CalibrationValuesKg0;
        public BalanceValues CalibrationValuesKg17;
        public BalanceValues CalibrationValuesKg34;
    }
}
