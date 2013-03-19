using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AgiliSway.vNext.Models.Legacy;
using Caliburn.Micro;
using Ninject;
using OxyPlot;
using AgiliSway.vNext.Services;

namespace AgiliSway.vNext.Collections
{
    class ImportCollectionViewModel : Screen
    {
		readonly IEventAggregator _events;
		readonly IDbStorage _storage;
		readonly IWindowManager _windowManager;

		[Inject]
		public ImportCollectionViewModel(IEventAggregator events, IDbStorage localStorage, IWindowManager windowManager)
		{
			this.DisplayName = "IMPORT COLLECTION";

			_events = events;
			_storage = localStorage;
			_windowManager = windowManager;
		}

        public string FilePath 
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
                NotifyOfPropertyChange(() => FilePath);

                LoadFile(FilePath);
            }
        }
        private string _filePath;

		public SamplingSession Session { get { return _session; } set { _session = value; NotifyOfPropertyChange(() => Session); } } private SamplingSession _session;
		public int CalibrationCount { get { return _calibrationCount; } set { _calibrationCount = value; NotifyOfPropertyChange(() => CalibrationCount); } } private int _calibrationCount;
		public int SampleCount { get { return _sampleCount; } set { _sampleCount = value; NotifyOfPropertyChange(() => SampleCount); } } private int _sampleCount;

		public string ProgressMessage { get { return _progressMessage; } set { _progressMessage = value; NotifyOfPropertyChange(() => ProgressMessage); } } private string _progressMessage;
		public PlotModel PathPlotModel { get { return _plotModel; } set { _plotModel = value; NotifyOfPropertyChange(() => PathPlotModel); } } private PlotModel _plotModel;

		public void SelectFile()
		{
			// Configure open file dialog box
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			if (!(string.IsNullOrEmpty(FilePath)))
				dlg.InitialDirectory = System.IO.Path.GetFullPath(FilePath);

			// Show open file dialog box
			Nullable<bool> result = dlg.ShowDialog();

			// Process open file dialog box results
			if (result == true)
			{
				// Open document
				FilePath = dlg.FileName;
			}
		}

		public void LoadFile(string filePath)
        {
			try
			{
				using (var sw = new StreamReader(filePath))
				{
					var ser = new XmlSerializer(typeof(SamplingSession));
					Session = (SamplingSession)ser.Deserialize(sw);

					CalibrationCount = Session.Calibration.Count;
					SampleCount = Session.Samples.Count;

					var cal = GetCalibration(Session.Calibration);

					var tmp = new PlotModel("Path Plot", "Some Text Here");

					var ss = new ScatterSeries
					{
						//StrokeThickness = 0,
						MarkerSize = 2,
						// MarkerFill = OxyColors.Blue,
						MarkerStroke = OxyColors.Black,
						MarkerType = MarkerType.Plus
					};

					foreach (var samp in Session.Samples)
					{
						var pt = samp.WiiBoards[0].COP(cal);
						var dp = new OxyPlot.DataPoint(pt.X, pt.Y);
						ss.Points.Add(dp);
					}

					tmp.Series.Add(ss);
					PathPlotModel = tmp;

					ProgressMessage = "";
				}
			}
			catch (Exception ex)
			{
				ProgressMessage = "Invalid file";
			}
        }

		public void Import()
		{
			this.TryClose(true);
		}

		public void Cancel()
		{
			this.TryClose(false);
		}

		private WiiBalanceBoardMeasurement GetCalibration(IEnumerable<Sample> samples)
		{
			var ul = 0;
			var ur = 0;
			var bl = 0;
			var br = 0;
			var count = 0;
			foreach (var meas in samples)
			{
				ul += meas.WiiBoards[0].TopLeft;
				ur += meas.WiiBoards[0].TopRight;
				bl += meas.WiiBoards[0].BottomLeft;
				br += meas.WiiBoards[0].BottomRight;
				count++;
			}

			ul /= count;
			ur /= count;
			bl /= count;
			br /= count;
			WiiBalanceBoardMeasurement cal = new WiiBalanceBoardMeasurement()
			{
				TopLeft = ul,
				TopRight = ur,
				BottomLeft = bl,
				BottomRight = br
			};

			return cal;
		}
    }
}
