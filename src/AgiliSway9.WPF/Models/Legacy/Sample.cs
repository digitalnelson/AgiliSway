using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgiliSway9.WPF.Models.Legacy
{
    public class Sample
    {
        public DateTime Timestamp;
        public long Ticks;
        public List<WiiBalanceBoardMeasurement> WiiBoards = new List<WiiBalanceBoardMeasurement>();
	}
}
