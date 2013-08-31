using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using AgiliSway9.WPF.Subjects;
using AgiliSway9.WPF.Collections;
using AgiliSway9.WPF.Studies;
using Ninject;
using AgiliSway9.WPF.Event;
using AgiliSway9.WPF.Dialogs;
using AgiliSway9.WPF.Services;
using AgiliSway9.WPF.Flyouts;

namespace AgiliSway9.WPF
{
	public class ShellViewModel :  Conductor<IScreen>.Collection.OneActive, IShell, IHandle<DataChangedEvent>, IHandle<StudySelectedEvent>, IHandle<SubjectSelectedEvent>
	{
		private readonly StudyManagerViewModel _studyManager;
		private readonly IDbStorage _localStorage;
		private readonly IEventAggregator _events;
		private readonly IWindowManager _windowManager;
		private readonly IAppPreferences _appPreferences;
		private readonly IDeviceManager _deviceManager;

		private string _selectedStudy = "All Studies";
		private string _selectedSubject = "All Subjects";

		[Inject]
		public ShellViewModel(StudyManagerViewModel studyManager, SubjectManagerViewModel subjectManager, CollectionManagerViewModel collectionManager, 
			IDbStorage localStorage, IEventAggregator events, IWindowManager windowManager, IAppPreferences appPreferences, IDeviceManager deviceManager)
		{
			_studyManager = studyManager;
			_localStorage = localStorage;
			_events = events;
			_windowManager = windowManager;
			_appPreferences = appPreferences;
			_deviceManager = deviceManager;

			SaveChanges = IoC.Get<SaveChangesViewModel>();

			_events.Subscribe(this);

			Items.Add(studyManager);
			Items.Add(subjectManager);
			Items.Add(collectionManager);
			Items.Add(IoC.Get<AgiliSway9.WPF.Sections.Device.MainViewModel>());
			//Items.Add(new DeviceViewModel() { DisplayName = "ANALYSIS" });

			UpdateTitle();
		}

		protected override void OnActivate()
		{
			base.OnActivate();

			long ticks = DateTime.Now.Ticks;

			 _studyManager.LoadStudies();

			long ticks2 = DateTime.Now.Ticks;
			TimeSpan ts = new TimeSpan(ticks2 - ticks);
			Console.WriteLine("Load Studies: " + ts.TotalMilliseconds);
		}

		public SaveChangesViewModel SaveChanges { get { return _inlSaveChanges; } set { _inlSaveChanges = value; NotifyOfPropertyChange(() => SaveChanges); } } private SaveChangesViewModel _inlSaveChanges;

		public void ShowSaveChanges()
		{
			SaveChanges.IsVisible = true;
		}

		public void Save()
		{
			_localStorage.Save();
			CanSave = false;
		}
		public bool CanSave
		{
			get { return _canSave; }
			set { _canSave = value; NotifyOfPropertyChange(() => CanSave); }
		} 
		private bool _canSave = false;

		protected override void OnDeactivate(bool close)
		{
			base.OnDeactivate(close);

			//_localStorage.Save();
			_localStorage.Close();
		}

		public void Handle(DataChangedEvent message)
		{
			CanSave = true;
		}

		public override void CanClose(Action<bool> callback)
		{
			if (CanSave)
			{
                // TODO: Figure out why this does not show sometimes on exception shutdown

				bool? toSave = _windowManager.ShowDialog(new ShouldSaveViewModel());

				if (toSave.HasValue && toSave.Value)
					_localStorage.Save();
			}

			base.CanClose(callback);
		}

		private void UpdateTitle()
		{
			this.DisplayName = string.Format("AGILISWAY PRO - {0} - {1}", _selectedStudy, _selectedSubject);
		}

		public void Handle(SubjectSelectedEvent message)
		{
			_selectedSubject = message.Title;

			UpdateTitle();
		}

		public void Handle(StudySelectedEvent message)
		{
			_selectedStudy = message.Study.Title;
			_selectedSubject = "All Subjects";

			UpdateTitle();
		}

		public void Prefs()
		{
			System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
			fbd.Description = "Choose location for AgiliSway data store.  The study database, sway files, and reports will be stored in this location.";

			if (!string.IsNullOrEmpty(_appPreferences.DataStorePath))
				fbd.SelectedPath = _appPreferences.DataStorePath;

			// Show open file dialog box
			System.Windows.Forms.DialogResult result = fbd.ShowDialog();

			// Process open file dialog box results
			if (result == System.Windows.Forms.DialogResult.OK && fbd.SelectedPath != _appPreferences.DataStorePath)
			{
				_appPreferences.DataStorePath = fbd.SelectedPath;
				System.Windows.Forms.MessageBox.Show("Please restart AgiliSway for change to take effect.");
			}
		}

		public void Help()
		{
			_windowManager.ShowWindow(new HelpViewModel());
		}
	}
}
