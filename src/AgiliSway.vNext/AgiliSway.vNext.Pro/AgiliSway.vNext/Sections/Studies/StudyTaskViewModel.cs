using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgiliSway.vNext.Models;
using Caliburn.Micro;

namespace AgiliSway.vNext.Studies
{
    public class StudyTaskViewModel : Screen
    {
        public StudyTaskViewModel(Task task)
        {
            Task = task;
        }

        public string Title { get { return Task.Title; } set { Task.Title = value; NotifyOfPropertyChange(() => Title); } }
        public int Duration { get { return Task.Duration; } set { Task.Duration = value; NotifyOfPropertyChange(() => Duration); } }
        public string Description { get { return Task.Description; } set { Task.Description = value; NotifyOfPropertyChange(() => Description); } }

        public Task Task { get; private set; }
    }
}
