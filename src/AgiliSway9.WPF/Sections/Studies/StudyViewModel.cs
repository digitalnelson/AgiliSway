using AgiliSway9.WPF.Events;
using AgiliSway9.WPF.Models;
using AgiliSway9.WPF.Services;
using Caliburn.Micro;

namespace AgiliSway9.WPF.Studies
{
	public class StudyViewModel : Screen
	{
		private readonly IEventAggregator _events;
		private readonly IDbStorage _localStorage;

		public StudyViewModel(Study study, IDbStorage localStorage, IEventAggregator events)
		{
			Study = study;
			_localStorage = localStorage;
			_events = events;
			
			Tasks = new BindableCollection<StudyTaskViewModel>();

			var tsks = localStorage.LoadTasks(study);
			foreach (Task tsk in tsks)
				Tasks.Add(new StudyTaskViewModel(tsk));

			Groups = new BindableCollection<StudyGroupViewModel>();

			var grps = localStorage.LoadGroups(study);
			foreach (Group grp in grps)
				Groups.Add(new StudyGroupViewModel(grp));
		}

		public void DataChanged()
		{
			_events.Publish(new DataChangedEvent { });
		}

		public Study Study { get; private set; }

		public int Id { get { return Study.StudyId; } set { Study.StudyId = value; DataChanged(); NotifyOfPropertyChange(() => Id); } }
		public string Title { get { return Study.Title; } set { Study.Title = value; DataChanged(); NotifyOfPropertyChange(() => Title); } }
		public string Description { get { return Study.Description; } set { Study.Description = value; DataChanged(); NotifyOfPropertyChange(() => Description); } }
		public string Notes { get { return Study.Notes; } set { Study.Notes = value; DataChanged(); NotifyOfPropertyChange(() => Notes); } }
		public string DataFolder { get { return Study.DataFolder; } set { Study.DataFolder = value; DataChanged(); NotifyOfPropertyChange(() => DataFolder); } }

		#region Groups
		public void NewGroup()
		{
			var grp = _localStorage.NewGroup(Study);
			grp.Title = "New Group";

			Groups.Add(new StudyGroupViewModel(grp));
			DataChanged();
		}
		public bool CanNewGroup
		{
			get { return true; }
		}

		public void DeleteGroup()
		{
			if (SelectedGroup != null)
			{
				Groups.Remove(SelectedGroup);
				SelectedGroup = null;
				DataChanged();
			}
		}
		public bool CanDeleteGroup
		{
			get { return SelectedGroup != null; }
		}

		public BindableCollection<StudyGroupViewModel> Groups { get; private set; }
		public StudyGroupViewModel SelectedGroup { 
			get { return _selectedGroup; } 
			set { 
				_selectedGroup = value; 
				NotifyOfPropertyChange(() => SelectedGroup); 
				NotifyOfPropertyChange(() => CanDeleteGroup);
			} 
		} private StudyGroupViewModel _selectedGroup;
		#endregion

		#region Tasks
		public void NewTask()
		{
			var tsk = _localStorage.NewTask(Study);
			tsk.Title = "New Task";

			Tasks.Add(new StudyTaskViewModel(tsk));
			DataChanged();
		}
		public bool CanNewTask
		{
			get { return true; }
		}

		public void DeleteTask()
		{
			if (SelectedTask != null)
			{
				Tasks.Remove(SelectedTask);
				SelectedTask = null;
				DataChanged();
			}
		}
		public bool CanDeleteTask
		{
			get { return SelectedTask != null; }
		}

		public BindableCollection<StudyTaskViewModel> Tasks { get; private set; }
		public StudyTaskViewModel SelectedTask { 
			get { return _selectedTask; } 
			set { _selectedTask = value; 
				NotifyOfPropertyChange(() => SelectedTask);
				NotifyOfPropertyChange(() => CanDeleteTask);
			} 
		} private StudyTaskViewModel _selectedTask;
		#endregion
	}
}
