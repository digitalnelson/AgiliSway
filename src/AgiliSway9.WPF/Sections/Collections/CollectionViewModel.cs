using AgiliSway9.WPF.Common;
using AgiliSway9.WPF.Events;
using AgiliSway9.WPF.ModelCalculations;
using AgiliSway9.WPF.Models;
using AgiliSway9.WPF.Models.Legacy;
using AgiliSway9.WPF.Services;
using Caliburn.Micro;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;

namespace AgiliSway9.WPF.Collections
{
	public class CollectionViewModel : DataAwareScreen, IHandle<DeviceSelected>
	{
		#region Service Injection
		private readonly IFileStorage _fileStorage = IoC.Get<IFileStorage>();
		private readonly IDeviceManager _deviceManager = IoC.Get<IDeviceManager>();
		#endregion

		public CollectionViewModel()
		{
			ScatterPlotModel = new PlotModel();
			ScatterPlotModel.PlotType = PlotType.Cartesian;

			MainSeries = new ScatterSeries
			{
				MarkerSize = 2,
				MarkerStroke = OxyColors.Black,
				MarkerType = MarkerType.Plus
			};
			ScatterPlotModel.Series.Add(MainSeries);

			FilteredSeries = new ScatterSeries
			{
				MarkerSize = 1,
				MarkerStroke = OxyColors.Red,
				MarkerType = MarkerType.Plus
			};
			ScatterPlotModel.Series.Add(FilteredSeries);

			_eventAggregator.Subscribe(this);
		}

		public void UpdateCanCollect()
		{
			if (_deviceManager.DeviceType == DeviceTypes.None 
				|| _deviceManager.CurrentConnector == null
				|| _deviceManager.CurrentConnector.CurrentDevice == null)
			{	
				CanCollect = false;
				Action = "";
				return;
			}

			if (Task == null)
			{
				CanCollect = false;
				Action = "";
				return;
			}

			if (CollectionDataSesssion != null 
				&& CollectionDataSesssion.DataPoints != null
				&& CollectionDataSesssion.DataPoints.PointSet != null 
				&& CollectionDataSesssion.DataPoints.PointSet.Count > 0)
			{
				CanCollect = false;
				Action = "Complete";
				return;
			}

			CanCollect = true;
			Action = "Calibrate";

			return;
		}

		public void Handle(DeviceSelected message)
		{
			UpdateCanCollect();
		}

		public void TaskUpdated()
		{
			UpdateCanCollect();

			// TODO: If task is already set and there is data, throw a warning....

			if(Collection != null && Task != null)
				this.Collection.Task = Task.Task;
		}

		public void CollectionDataSesssionUpdated()
		{
			UpdateCanCollect();

			if (CollectionDataSesssion != null && CollectionDataSesssion.Calibration != null)
			{
				Collection.Timestamp =  CollectionDataSesssion.DataPoints.TimestampUtc;

				var calDP = Calibration.Calculate(CollectionDataSesssion.Calibration.PointSet);

				double sumAPDist = 0;
				double sumMLDist = 0;
				double valCount = 0;
				PointF prevPt;
				prevPt.X = 0;
				prevPt.Y = 0;

				MainSeries.Points.Clear();
				foreach (var samp in CollectionDataSesssion.DataPoints.PointSet)
				{
					var pt = SwayPosition.Calculate(calDP, samp);
					var dp = new OxyPlot.DataPoint(pt.X, pt.Y);

					if ((valCount >= 250) && (valCount < (CollectionDataSesssion.DataPoints.PointSet.Count - 250)))
					{
						sumAPDist += Math.Abs(pt.Y - prevPt.Y);
						sumMLDist += Math.Abs(pt.X - prevPt.X);

						MainSeries.Points.Add(dp);
					}
					else
					{
						FilteredSeries.Points.Add(dp);
					}

					prevPt = pt;
					valCount++;
				}

				APDist = sumAPDist / valCount;
				MLDist = sumMLDist / valCount;

				try
				{
					ScatterPlotModel.RefreshPlot(true);
				}
				catch (Exception) { }

				NotifyOfPropertyChange(() => Timestamp);
				NotifyOfPropertyChange(() => ScatterPlotModel);
			}
		}

		public void SetTaskViewModel(IList<TaskViewModel> tasks)
		{
			if (Collection != null && Collection.TaskId != null)
			{
				foreach (var tvm in tasks)
				{
					if (tvm.Task.TaskId == Collection.TaskId)
						Task = tvm;
				}
			}
		}

		public void LoadDataSession()
		{
			if (CollectionDataSesssion == null)
				CollectionDataSesssion = _fileStorage.LoadCollectionDataSesssionFile(FilePath);
		}

		public void AddWorkingDataPoint(OxyPlot.DataPoint dp)
		{
			MainSeries.Points.Add(dp);

			System.Threading.Tasks.Task.Run(() =>
			{
				ScatterPlotModel.RefreshPlot(true);
			});
			NotifyOfPropertyChange(() => ScatterPlotModel);
		}

		// TODO: Rename this to _collectionModel;
		public Collection Collection { get { return _inlCollection; } set { _inlCollection = value; NotifyOfPropertyChange(() => Collection); NotifyOfPropertyChange(() => Timestamp); } } private Collection _inlCollection;

		public int Id { get { return Collection.CollectionId; } set { Collection.CollectionId = value; NotifyOfPropertyChange(() => Id); DataChanged(); } }
		public string ExternalId { get { return Collection.ExternalId; } set { Collection.ExternalId = value; NotifyOfPropertyChange(() => ExternalId); DataChanged(); } }
		public string Title { get { return Collection.Title; } set { Collection.Title = value; NotifyOfPropertyChange(() => Title); DataChanged(); } }
		public string FilePath { get { return Collection.FilePath; } set { Collection.FilePath = value; NotifyOfPropertyChange(() => FilePath); DataChanged(); } }
		public string Notes { get { return Collection.Notes; } set { Collection.Notes = value; NotifyOfPropertyChange(() => Notes); DataChanged(); } }
		public DateTime? Timestamp { get { return Collection.Timestamp; } set { Collection.Timestamp = value; NotifyOfPropertyChange(() => Timestamp); DataChanged(); } }

		// TODO: Make sure to set the task on the Collection
		//public Task Task { get { return Collection.Task; } set { Collection.Task = value; NotifyOfPropertyChange(() => Task); } }
		public TaskViewModel Task { get { return _inlTask; } set { _inlTask = value; NotifyOfPropertyChange(() => Task); TaskUpdated(); } } private TaskViewModel _inlTask;

		public string Action { get { return _inlAction; } set { _inlAction = value; NotifyOfPropertyChange(() => Action); } } private string _inlAction;
		public bool CanCollect { get { return _inlCanCollect; } set { _inlCanCollect = value; NotifyOfPropertyChange(() => CanCollect); } } private bool _inlCanCollect;
		public CollectionDataSesssion CollectionDataSesssion 
		{
			get { return _inlCollectionDataSesssion; } 
			set 
			{
				_inlCollectionDataSesssion = value; NotifyOfPropertyChange(() => CollectionDataSesssion); CollectionDataSesssionUpdated(); 
			} 
		} private CollectionDataSesssion _inlCollectionDataSesssion;

		public PlotModel ScatterPlotModel { get { return _plotModel; } set { _plotModel = value; NotifyOfPropertyChange(() => ScatterPlotModel); } } private PlotModel _plotModel;
		public ScatterSeries MainSeries { get { return _inlRealtimeSeries; } set { _inlRealtimeSeries = value; NotifyOfPropertyChange(() => MainSeries); } } private ScatterSeries _inlRealtimeSeries;
		public ScatterSeries FilteredSeries { get { return _inlFilteredSeries; } set { _inlFilteredSeries = value; NotifyOfPropertyChange(() => FilteredSeries); } } private ScatterSeries _inlFilteredSeries;

		public double APDist { get { return _inlAPDist; } set { _inlAPDist = value; NotifyOfPropertyChange(() => APDist); } } private double _inlAPDist;
		public double MLDist { get { return _inlMLDist; } set { _inlMLDist = value; NotifyOfPropertyChange(() => MLDist); } } private double _inlMLDist;
	}
}
