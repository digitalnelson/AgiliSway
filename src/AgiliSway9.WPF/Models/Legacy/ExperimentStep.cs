using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace AgiliSway9.WPF.Models.Legacy
{
    public class ExperiementStep
    {
        public String Name;
        public String Direction;
        public int Duration;
        public Brush Brush;

        public override string ToString()
        {
            return Name;
        }
    }
}
