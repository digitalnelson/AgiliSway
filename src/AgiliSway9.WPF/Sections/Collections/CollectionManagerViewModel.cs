using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AgiliSway9.WPF.Common;
using AgiliSway9.WPF.Event;
using AgiliSway9.WPF.ModelCalculations;
using AgiliSway9.WPF.Models;
using AgiliSway9.WPF.Models.Legacy;
using Caliburn.Micro;
using Ninject;
using OxyPlot;
using AgiliSway9.WPF.Services;

namespace AgiliSway9.WPF.Collections
{
	public class CollectionManagerViewModel : DataAwareScreen, IHandle<StudySelectedEvent>, IHandle<SubjectSelectedEvent>
	{
		#region Service Injection
		private readonly IWindowManager _windowManager = IoC.Get<IWindowManager>();
		private readonly IDbStorage _storage = IoC.Get<IDbStorage>();
		private readonly IFileStorage _fileStorage = IoC.Get<IFileStorage>();
		private readonly IDeviceManager _deviceManager = IoC.Get<IDeviceManager>();
		#endregion

		#region Private Member Vars
		private StudySelectedEvent _selectedStudy;
		private SubjectSelectedEvent _selectedSubject;
		#endregion

		public CollectionManagerViewModel() 
		{
			this.DisplayName = "COLLECTIONS";

			Action = "Calibrate";
			
			Collections = new BindableCollection<CollectionViewModel>();
			Tasks = new BindableCollection<TaskViewModel>();

			_eventAggregator.Subscribe(this);
		}

		public void Handle(StudySelectedEvent message)
		{
			_selectedStudy = message;
			Clear();
		}

		public void Handle(SubjectSelectedEvent message)
		{
			_selectedSubject = message;
			Clear();
		}
		
		protected override void OnActivate()
		{
			base.OnActivate();

			if (Tasks.Count == 0)
				LoadTasks();

			if(Collections.Count == 0)
				LoadCollections();
		}

		private void Clear()
		{
			Tasks.Clear();

			Collections.Clear();
			SelectedCollection = null;
		}

		private void LoadTasks()
		{
			Tasks.Clear();

			if (_selectedStudy != null && _selectedStudy.Study.StudyId != -1)
			{
				var tasks = _storage.LoadTasks(_selectedStudy.Study);

				foreach (var task in tasks)
				{
					var tvm = IoC.Get<TaskViewModel>();
					tvm.Task = task;

					Tasks.Add(tvm);
				}
			}
		}

		private void LoadCollections()
		{
			Collections.Clear();

			Collection[] collections = null;
			if (_selectedSubject != null && _selectedSubject.Subject.SubjectId != -1)
				collections = _storage.LoadCollections(_selectedSubject.Subject);
			else if (_selectedStudy != null && _selectedStudy.Study.StudyId != -1)
				collections = _storage.LoadCollections(_selectedStudy.Study);
			else
				collections = _storage.LoadCollections();

			foreach (var collection in collections)
			{
				var cvm = IoC.Get<CollectionViewModel>();
				cvm.Collection = collection;

				cvm.SetTaskViewModel(Tasks);
				cvm.LoadDataSession();

				Collections.Add(cvm);
			}
		}
		
		public void CreateCollection(CollectionDataSesssion collectionDataSession, string externalFilePath)
		{
			var externalFileName = Path.GetFileName(externalFilePath);

			// TODO: This is weird... May need to move this to when the db actually saves the rows so we have more information for the filename and the contents of the file
			// It would be really nice if the file stood on its own.
			var internalFilePath = _fileStorage.GenerateFilePath(_selectedStudy.Study, _selectedSubject.Subject, externalFileName);
			_fileStorage.SaveFile(collectionDataSession, internalFilePath);

			var collection = _storage.NewCollection(_selectedSubject.Subject);
			collection.Timestamp = collectionDataSession.DataPoints.TimestampUtc;
			collection.FilePath = internalFilePath;
			collection.ImportPath = externalFilePath;

			if (!string.IsNullOrEmpty(collection.ImportPath))
			{
				var path = collection.ImportPath.Split(Path.DirectorySeparatorChar).Reverse().ToArray();
				if (path.Length > 1)
					collection.Title = string.Format("{0} ({1})", path[0], path[1]);
				else
					collection.Title = string.Format(path[0]);
			}

			var cvm = IoC.Get<CollectionViewModel>();
			cvm.Collection = collection;
			cvm.CollectionDataSesssion = collectionDataSession;

			Collections.Add(cvm);
		}

		public void ImportCollection()
		{			
			// Configure open file dialog box
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.Multiselect = true;
						
			// Show open file dialog box
			bool? result = dlg.ShowDialog();
			
			// Process open file dialog box results
			if (result.HasValue && result.Value)
			{
				foreach (var externalFilePath in dlg.FileNames)
				{
					// TODO: Will need a status bar...
					var sess = _fileStorage.LoadSamplingSessionFile(externalFilePath);

					// Import calibration data
					var calDS = new CollectionDataSet();
					calDS.PointSet = new List<CollectionDataPoint>();
					foreach (var samp in sess.Calibration)
					{
						calDS.PointSet.Add(new CollectionDataPoint
						{
							TopLeft = new CollectionValue { Z = samp.WiiBoards[0].TopLeft },
							TopRight = new CollectionValue { Z = samp.WiiBoards[0].TopRight },
							BottomLeft = new CollectionValue { Z = samp.WiiBoards[0].BottomLeft },
							BottomRight = new CollectionValue { Z = samp.WiiBoards[0].BottomRight },
						});
					}
					
					DateTime lastTime = DateTime.MinValue;
					CollectionDataSesssion curr = null;

					// Loop through samples and break into individual data sets for multitask files
					foreach (var samp in sess.Samples)
					{
						if ((samp.Timestamp - lastTime).Seconds > 2 || lastTime == DateTime.MinValue)
						{
							if (lastTime != DateTime.MinValue)
								CreateCollection(curr, externalFilePath);

							curr = new CollectionDataSesssion();
							curr.ExternalId = sess.SubjectId;

							curr.Calibration = calDS;

							curr.DataPoints = new CollectionDataSet();
							curr.DataPoints.TimestampUtc = samp.Timestamp.ToUniversalTime();  //TODO: This needs to be made correct but switch in time
						}
						
						curr.DataPoints.PointSet.Add(new CollectionDataPoint
						{
							TopLeft = new CollectionValue { Z = samp.WiiBoards[0].TopLeft },
							TopRight = new CollectionValue { Z = samp.WiiBoards[0].TopRight },
							BottomLeft = new CollectionValue { Z = samp.WiiBoards[0].BottomLeft },
							BottomRight = new CollectionValue { Z = samp.WiiBoards[0].BottomRight },
						});

						lastTime = samp.Timestamp;
					}

					if (lastTime != DateTime.MinValue)
						CreateCollection(curr, externalFilePath);

					DataChanged();
				}
			}
		}
		public bool CanImportCollection { get { return true; } }

		public void NewCollection()
		{
			if (_selectedSubject != null)
			{
				var cvm = IoC.Get<CollectionViewModel>();

				cvm.Collection = _storage.NewCollection(_selectedSubject.Subject);
				cvm.Timestamp = DateTime.UtcNow;
				
				Collections.Add(cvm);
				SelectedCollection = cvm;

				DataChanged();
			}
		}

		public void DeleteCollection()
		{
			if (SelectedCollection != null)
			{
				_storage.DeleteCollection(SelectedCollection.Collection);
				Collections.Remove(SelectedCollection);
				SelectedCollection = null;

				DataChanged();
			}
		}
		public bool CanDeleteCollection
		{
			get { return SelectedCollection != null; }
		}

		public void ExportCollections()
		{
			System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
			fbd.Description = "Choose location for file export.";

			// Show open file dialog box
			System.Windows.Forms.DialogResult result = fbd.ShowDialog();

			// Process open file dialog box results
			if (result == System.Windows.Forms.DialogResult.OK)
			{
				foreach (var collection in Collections)
				{
					string fileName = "swayraw";

					fileName += "_" + collection.Collection.Subject.ExternalId;
					fileName += "_" + collection.Collection.Timestamp.Value.ToString("yyyyMMddhhmmss");
					if (collection.Collection.Task != null)
						fileName += "_" + collection.Collection.Task.Title;
					fileName += ".csv";

					using(StreamWriter sw = new StreamWriter(Path.Combine(fbd.SelectedPath, fileName)))
					{
						sw.WriteLine("TopLeft.Z, TopRight.Z, BottomLeft.Z, BottomRight.Z");

						if (collection.CollectionDataSesssion == null && !string.IsNullOrEmpty(collection.Collection.FilePath))
							collection.CollectionDataSesssion = _fileStorage.LoadCollectionDataSesssionFile(collection.Collection.FilePath);

						if (collection.CollectionDataSesssion != null)
						{
							foreach (var dp in collection.CollectionDataSesssion.DataPoints.PointSet)
								sw.WriteLine(string.Format("{0},{1},{2},{3}", dp.TopLeft.Z, dp.TopRight.Z, dp.BottomLeft.Z, dp.BottomRight.Z));
						}
					}
				}
			}
		}
		public bool CanExportCollections
		{
			get { return Collections.Count > 0; }
		}

		private System.Threading.CancellationToken _cancellationToken = new System.Threading.CancellationToken();

		private void UpdateProgress(CollectProgress value)
		{
			var pct = ( (float)value.Elapsed.TotalMilliseconds / (float)value.Desired.TotalMilliseconds ) * 100;
			this.Progress = (int)pct;
			this.ProgressTime = string.Format("{0}:{1}:{2}.{3}", value.Elapsed.Hours.ToString("00"), value.Elapsed.Minutes.ToString("00"), value.Elapsed.Seconds.ToString("00"), value.Elapsed.Milliseconds.ToString("000"));

			SelectedCollection.AddWorkingDataPoint(new OxyPlot.DataPoint(value.Point.X, value.Point.Y));
		}

		public async System.Threading.Tasks.Task<CollectionDataSet> CollectDataPoints(TimeSpan ts, CollectionDataPoint calibration)
		{
			// Call for device collection to begin and pass it a lambda for progress updates
			var ds = await _deviceManager.CurrentConnector.CurrentDevice.Collect(ts, new Progress<CollectProgress>(UpdateProgress), _cancellationToken, calibration);

			ProgressTime = string.Format("{0}:{1}:{2}.{3}", ts.Hours.ToString("00"), ts.Minutes.ToString("00"), ts.Seconds.ToString("00"), ts.Milliseconds.ToString("000"));
			Progress = 0;

			return ds;
		}

		public async void Collect()
		{
			if (SelectedCollection != null && SelectedCollection.CanCollect)
			{
				ProgressIsEnabled = true;

				// Call for device collection to begin and pass it a lambda for progress updates
				switch (SelectedCollection.Action)
				{
					case "Calibrate":
						SelectedCollection.CollectionDataSesssion = new CollectionDataSesssion();
						SelectedCollection.CollectionDataSesssion.Calibration = await CollectDataPoints(TimeSpan.FromSeconds(1), null);
						SelectedCollection.Timestamp = SelectedCollection.CollectionDataSesssion.Calibration.TimestampUtc;
						SelectedCollection.Action = "Collect";
						
						break;
					
					case "Collect":
						var calDP = Calibration.Calculate(SelectedCollection.CollectionDataSesssion.Calibration.PointSet);
						SelectedCollection.CollectionDataSesssion.DataPoints = await CollectDataPoints(TimeSpan.FromSeconds(SelectedCollection.Task.Task.Duration), calDP);
						SelectedCollection.CollectionDataSesssionUpdated();

						// TODO: This should be done in the collection view
						var internalFilePath = _fileStorage.GenerateFilePath(_selectedStudy.Study, _selectedSubject.Subject, "Task" + SelectedCollection.Task.Task.TaskId.ToString());
						_fileStorage.SaveFile(SelectedCollection.CollectionDataSesssion, internalFilePath);

						SelectedCollection.Timestamp = SelectedCollection.CollectionDataSesssion.DataPoints.TimestampUtc;
						SelectedCollection.Collection.FilePath = internalFilePath;
						_storage.Save();

						SelectedCollection.Action = "Complete";
								
						break;
				}

				ProgressIsEnabled = false;
			}
		}

		public string Action { get { return _inlAction; } set { _inlAction = value; NotifyOfPropertyChange(() => Action); } } private string _inlAction;
		public int Progress { get { return _inlProgress; } set { _inlProgress = value; NotifyOfPropertyChange(() => Progress); } } private int _inlProgress;
		public bool ProgressIsEnabled { get { return _inlProgressIsEnabled; } set { _inlProgressIsEnabled = value; NotifyOfPropertyChange(() => ProgressIsEnabled); } } private bool _inlProgressIsEnabled;
		public string ProgressTime { get { return _inlProgressTime; } set { _inlProgressTime = value; NotifyOfPropertyChange(() => ProgressTime); } } private string _inlProgressTime;

		public BindableCollection<CollectionViewModel> Collections { get; private set; }
		
		public CollectionViewModel SelectedCollection
		{
			get { return _selectedCollection; }
			set
			{
				_selectedCollection = value;

				if (_selectedCollection != null)
				{
					_eventAggregator.Publish(new CollectionSelectedEvent() { Collection = _selectedCollection.Collection });
				}

				NotifyOfPropertyChange(() => SelectedCollection);
				NotifyOfPropertyChange(() => CanDeleteCollection);
			}
		}
		private CollectionViewModel _selectedCollection;
		
		public BindableCollection<TaskViewModel> Tasks { get; set; }
	}
}
