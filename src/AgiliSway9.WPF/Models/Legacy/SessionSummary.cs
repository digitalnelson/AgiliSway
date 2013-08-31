using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace AgiliSway9.WPF.Models.Legacy
{
    public class SessionSummary : INotifyPropertyChanged
    {
        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                NotifyProperty("FullName");
            }
        }
        private string _fullName = "";

        public string Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                NotifyProperty("Birthday");
            }
        }
        private string _birthday = "";

        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                NotifyProperty("Gender");
            }
        }
        private string _gender = "";

        public ObservableCollection<DataPoint> DataPoints
        {
            get { return _dataPoints; }
            set
            {
                _dataPoints = value;
                NotifyProperty("DataPoints");
            }
        }
        private ObservableCollection<DataPoint> _dataPoints = new ObservableCollection<DataPoint>();

        public double WeightKgs
        {
            get { return _weightKgs; }
            set { _weightKgs = value; NotifyProperty("WeightKgs"); }
        }
        private double _weightKgs;

        public string CurrentWeight
        {
            get { return String.Format("{0:0}", WeightKgs); }
            set { }
        }

        public int TotalMilliseconds { 
            get
            {
                return _totalMillieconds;
            } 
            set
            {
                _totalMillieconds = value;
                NotifyProperty("TotalMilliseconds");
            }
        }
        private int _totalMillieconds = 0;

        public string TotalTime
        {
            get
            {
                var minutes = (int)TotalMilliseconds / 60000;
                var seconds = (TotalMilliseconds - minutes * 60000) / 1000;
                var milliseconds = TotalMilliseconds - (minutes*60000) - (seconds*1000);
                return String.Format("{0:00}", minutes) + "m" + String.Format("{0:00}", seconds) + "." + String.Format("{0:0}", milliseconds/100) + "s";
            }
            set { }
        }

        public double ApDist 
        { 
            get { return _apDist; }
            set
            {
                _apDist = value;
                NotifyProperty("ApDist");
            }
        }
        private double _apDist = 0.0;

        public double MlDist
        {
            get { return _mlDist; }
            set
            {
                _mlDist = value;
                NotifyProperty("MlDist");
            }
        }
        private double _mlDist = 0.0;


        public string ApDistString
        {
            get { return String.Format("{0:0.000}", ApDist); }
            set { }
        }
        public string MlDistString
        {
            get { return String.Format("{0:0.000}", MlDist); }
            set { }
        }
        public string TotalDistString
        {
            get { return String.Format("{0:0.000}", ApDist+MlDist); }
            set { }
        }
        public string AvgDistString
        {
            get
            {
                var count = _dataPoints.Count;
                if (count > 0)
                    return String.Format("{0:0.000}", (ApDist + MlDist) / count);
                else
                    return "0.000";
            }
            set { }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyProperty(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public string Notes
        {
            get { return _notes; }
            set { _notes = value;
                NotifyProperty("Notes"); }
        }

        private string _notes = "";
    }
}
