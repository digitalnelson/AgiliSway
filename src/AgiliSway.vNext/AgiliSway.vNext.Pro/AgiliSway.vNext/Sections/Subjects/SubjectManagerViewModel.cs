using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgiliSway.vNext.Event;
using AgiliSway.vNext.Models;
using Caliburn.Micro;
using Ninject;
using AgiliSway.vNext.Common;
using System.IO;
using AgiliSway.vNext.Services;

namespace AgiliSway.vNext.Subjects
{
    public class SubjectManagerViewModel : DataAwareScreen, IHandle<StudySelectedEvent>
	{
		#region Private Member Variables
		
		readonly IEventAggregator _events;
		readonly IDbStorage _localStorage;
        readonly IWindowManager _windowManager;

        private StudySelectedEvent _selectedStudy;
		
		#endregion

		[Inject]
		public SubjectManagerViewModel(IEventAggregator events, IDbStorage localStorage, IWindowManager windowManager)
		{
			this.DisplayName = "SUBJECTS";

			_events = events;
            _localStorage = localStorage;
            _windowManager = windowManager;

            Subjects = new BindableCollection<SubjectViewModel>();

            events.Subscribe(this);
		}

		public void Handle(StudySelectedEvent message)
		{
			_selectedStudy = message;
			Clear();
		}

		protected override void OnActivate()
		{
			base.OnActivate();

			if(Subjects.Count == 0)
				LoadSubjects();

			if (SelectedSubject != null)
				SelectedSubject.Update();
		}

		private void Clear()
		{
			Subjects.Clear();
			SelectedSubject = null;
		}

        private void LoadSubjects()
        {
            Subjects.Clear();

            Subject[] subjects = null;
            if(_selectedStudy != null)
				subjects = _localStorage.LoadSubjects(_selectedStudy.Study);
            else
				subjects = _localStorage.LoadSubjects();

			var studyId = -1;
			if (_selectedStudy != null && _selectedStudy.Study != null)
				studyId = _selectedStudy.Study.StudyId;

			Subjects.Add(new SubjectViewModel(new Subject() { FirstName = "ALL", LastName = "SUBJECTS", SubjectId = -1, StudyId = studyId }, _events, _localStorage, _windowManager));
            foreach (var subj in subjects)
            {
				Subjects.Add(new SubjectViewModel(subj, _events, _localStorage, _windowManager));
            }
        }

		public void ImportSubjects()
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
					using (StreamReader sr = new StreamReader(externalFilePath))
					{
						var firstLine = sr.ReadLine();
						var columns = firstLine.Split('\t');

						var columnLookup = new Dictionary<string, int>();
						for(int i=0; i<columns.Length; i++)
							columnLookup[columns[i].ToLower()] = i;

						if (!columnLookup.ContainsKey("subjectid"))
							throw new Exception("Must have at least one column with the header: subjectid");

						var subjectIdIdx = columnLookup["subjectid"];

						string line = null;
						while(!string.IsNullOrEmpty(line = sr.ReadLine()))
						{
							var cols = line.Split('\t');

							var subjectId = cols[subjectIdIdx];

							StringBuilder sbNote = new StringBuilder();
							for (int i = 0; i < cols.Length; i++ )
							{
								if (i != subjectIdIdx)
									sbNote.AppendLine(string.Format("{0}: {1}", columns[i], cols[i]));
							}

							var subj = new SubjectViewModel(_localStorage.NewSubject(_selectedStudy.Study), _events, _localStorage, _windowManager);
							subj.ExternalId = subjectId;
							subj.Notes = sbNote.ToString();

							Subjects.Add(subj);
						}
					}
				}

				DataChanged();
			}
		}
		public bool CanImportSubjects { get { return true; } }

        public void NewSubject()
        {
            if (_selectedStudy != null)
            {
				var subj = new SubjectViewModel(_localStorage.NewSubject(_selectedStudy.Study), _events, _localStorage, _windowManager);
                Subjects.Add(subj);
                SelectedSubject = subj;
				
				DataChanged();
            }
        }

        public void DeleteSubject()
        {
            if (SelectedSubject != null)
            {
				_localStorage.DeleteSubject(SelectedSubject.Subject);
                Subjects.Remove(SelectedSubject);
                SelectedSubject = null;

				DataChanged();
            }
        }

        public BindableCollection<SubjectViewModel> Subjects { get; private set; }
        public SubjectViewModel SelectedSubject
		{
			get { return _selectedSubject; }
			set
			{
				_selectedSubject = value;

				if (_selectedSubject != null)
				{
					_selectedSubject.Update();
					_events.Publish(new SubjectSelectedEvent() { Subject = _selectedSubject.Subject, Title = _selectedSubject.Title });
				}

                NotifyOfPropertyChange(() => SelectedSubject);
			}
		}
        private SubjectViewModel _selectedSubject;
	}
}
