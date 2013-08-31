using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgiliSway9.WPF.Models.Legacy
{
    public class SubjectStep
    {
        public string Name;

        public double L;
        public double d;
        public double FD;
        public double dL;
        public double AvgVec0;
        public double Expanse;

        public double AvgX;
        public double AvgY;
        public CartesianRange Range;

        public List<BalanceMeasurement> BalanceMeasurements = new List<BalanceMeasurement>();
    }
}
