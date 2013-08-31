using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace AgiliSway9.WPF.Models.Legacy
{
    public class SamplingSession
    {
        public DateTime Timestamp = DateTime.Now;
        public String SubjectId = "";
        public String Notes = "";
        public long TicksPerSec = Stopwatch.Frequency;
		public BindableCollection<Sample> Calibration = new BindableCollection<Sample>();
		public BindableCollection<Sample> Samples = new BindableCollection<Sample>();
    }
}
