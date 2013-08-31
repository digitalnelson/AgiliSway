using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgiliSway9.WPF.Models.Legacy
{
    public class Range
    {
        public float Max = float.MinValue;
        public float Min = float.MaxValue;
        public float Avg = 0;

        private float _total = 0;
        private float _count = 0;

        public void Update(float val)
        {
            if (val > Max)
                Max = val;
            if (val < Min)
                Min = val;

            _total += val;
            _count++;

            Avg = _total / _count;
        }
    }
}
