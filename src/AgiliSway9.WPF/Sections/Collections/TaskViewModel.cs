using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgiliSway9.WPF.Models;
using Caliburn.Micro;

namespace AgiliSway9.WPF.Collections
{
	public class TaskViewModel : Screen
    {
		public TaskViewModel()
        {}

        public string Title { get { return Task.Title; } set { Task.Title = value; NotifyOfPropertyChange(() => Title); } }
        public int Duration { get { return Task.Duration; } set { Task.Duration = value; NotifyOfPropertyChange(() => Duration); } }
        public string Description { get { return Task.Description; } set { Task.Description = value; NotifyOfPropertyChange(() => Description); } }

        public Task Task { get; set; }
    }
}
