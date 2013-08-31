using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AgiliSway9.WPF.Models.Legacy
{
    public class DataPoint : INotifyPropertyChanged
    {
        public DataPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X
        {
            get { return _x; }
            set { _x = value;
                NotifyProperty("X"); }
        }

        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                NotifyProperty("Y");
            }
        }

        private double _x;
        private double _y;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyProperty(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
