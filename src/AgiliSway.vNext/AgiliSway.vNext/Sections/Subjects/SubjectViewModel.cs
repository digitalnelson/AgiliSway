using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgiliSway.vNext.Models;
using Caliburn.Micro;
using Ninject;
using AgiliSway.vNext.Services;

namespace AgiliSway.vNext.Subjects
{
	public class SubjectViewModel : Screen
	{
		readonly IEventAggregator _events;
		readonly IDbStorage _storage;
		readonly IWindowManager _windowManager;

		public SubjectViewModel(Subject subj, IEventAggregator events, IDbStorage localStorage, IWindowManager windowManager)
        {
            Subject = subj;
			Groups = new BindableCollection<GroupViewModel>();
			Collections = new BindableCollection<CollectionViewModel>();

			_events = events;
			_storage = localStorage;
			_windowManager = windowManager;
        }

		public void Update()
		{
			Groups.Clear();
			Collections.Clear();

			if (Subject.SubjectId != -1)
			{
				foreach (var grp in Subject.Study.Groups)
				{
					var grpViewModel = new GroupViewModel(grp);

					if (Subject.Group == grp)
						SelectedGroup = grpViewModel;

					Groups.Add(grpViewModel);
				}

				foreach (var coll in Subject.Collections)
				{
					Collections.Add(new CollectionViewModel(coll));
				}
			}
		}

        public Subject Subject { get; private set; }

		public int Id { get { return Subject.SubjectId; } set { Subject.SubjectId = value; NotifyOfPropertyChange(() => Id); NotifyOfPropertyChange(() => Identifier); } }
		public string ExternalId { get { return Subject.ExternalId; } set { Subject.ExternalId = value; NotifyOfPropertyChange(() => ExternalId); NotifyOfPropertyChange(() => Title); } }
		public string FirstName { get { return Subject.FirstName; } set { Subject.FirstName = value; NotifyOfPropertyChange(() => FirstName); NotifyOfPropertyChange(() => Title); } }
		public string LastName { get { return Subject.LastName; } set { Subject.LastName = value; NotifyOfPropertyChange(() => LastName); NotifyOfPropertyChange(() => Title); } }
        public string Birthdate { get { return Subject.Birthdate; } set { Subject.Birthdate = value; NotifyOfPropertyChange(() => Birthdate); } }
		public string Notes { get { return Subject.Notes; } set { Subject.Notes = value; NotifyOfPropertyChange(() => Notes); } }

		public BindableCollection<GroupViewModel> Groups { get; private set; }
		public GroupViewModel SelectedGroup
		{
			get { return _selectedGroup; }
			set
			{
				_selectedGroup = value;
				if(_selectedGroup != null)
					Subject.Group = _selectedGroup.Group;
				NotifyOfPropertyChange(() => SelectedGroup);
			}
		} private GroupViewModel _selectedGroup;

		public BindableCollection<CollectionViewModel> Collections { get; private set; }

		public string Title
		{
			get
			{
				var title = "";

				if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
					title = FirstName + " " + LastName;
				else if (!string.IsNullOrEmpty(LastName))
					title = LastName;
				else if (!string.IsNullOrEmpty(FirstName))
					title = FirstName;

				if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(ExternalId))
					title += " - ";

				if (!string.IsNullOrEmpty(ExternalId))
					title += ExternalId;

				return title;
			}
		}

		public string Identifier
		{
			get
			{
				if (Id > -1)
					return Id.ToString();
				else
					return "";
			}
		}
	}
}
