﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgiliSway.vNext.Event;
using AgiliSway.vNext.Models;
using Caliburn.Micro;
using Ninject;
using AgiliSway.vNext.Common;
using AgiliSway.vNext.Services;

namespace AgiliSway.vNext.Studies
{
	public class StudyManagerViewModel : DataAwareScreen
	{
		readonly IEventAggregator _events;
		readonly IDbStorage _localStorage;

        private string NewId()
        {
            return Guid.NewGuid().ToString().Replace("{", "").Replace("}", "").Replace("-", "");
        }

		[Inject]
		public StudyManagerViewModel(IEventAggregator events, IDbStorage localStorage)
		{
			this.DisplayName = "STUDIES";
            this.Studies = new BindableCollection<StudyViewModel>();

			_events = events;
			_localStorage = localStorage;
		}

		public void LoadStudies()
		{
			var studies = _localStorage.LoadStudies();
			foreach (var study in studies)
			{
				Studies.Add(new StudyViewModel(study, _localStorage, _events));
			}
		}

        public void NewStudy()
        {
			var study = new StudyViewModel(_localStorage.NewStudy(), _localStorage, _events);
            Studies.Add(study);
            SelectedStudy = study;
			DataChanged();
        }

        public void DeleteStudy()
        {
            if (SelectedStudy != null)
            {
				_localStorage.DeleteStudy(SelectedStudy.Study);
                Studies.Remove(SelectedStudy);
                SelectedStudy = null;
				DataChanged();
            }
        }
        public bool CanDeleteStudy
        {
            get { return SelectedStudy != null; }
        }

        public void SaveAll()
        {
			_localStorage.Save();
        }
        public bool CanSaveAll
        {
            get { return SelectedStudy != null; }
        }

		public BindableCollection<StudyViewModel> Studies { get; private set; }

		public StudyViewModel SelectedStudy
		{
			get { return _selectedStudy; }
			set
			{
				_selectedStudy = value;

                if(_selectedStudy != null)
                    _events.Publish(new StudySelectedEvent() { Study = _selectedStudy.Study });
                
                NotifyOfPropertyChange(() => SelectedStudy);
                NotifyOfPropertyChange(() => CanDeleteStudy);
                NotifyOfPropertyChange(() => CanSaveAll);
			}
		}
		private StudyViewModel _selectedStudy;
	}
}
