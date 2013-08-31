using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgiliSway9.WPF.Models;
using Caliburn.Micro;

namespace AgiliSway9.WPF.Studies
{
    public class StudyGroupViewModel : Screen
    {
        public StudyGroupViewModel(Group group)
        {
            Group = group;
        }

        public string Title { get { return Group.Title; } set { Group.Title = value; NotifyOfPropertyChange(() => Title); } }
        public string Description { get { return Group.Description; } set { Group.Description = value; NotifyOfPropertyChange(() => Description); } }

        public Group Group { get; private set; }
    }
}
