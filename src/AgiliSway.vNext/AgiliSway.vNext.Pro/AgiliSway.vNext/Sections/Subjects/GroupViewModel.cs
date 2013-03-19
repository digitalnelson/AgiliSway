﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgiliSway.vNext.Models;
using Caliburn.Micro;

namespace AgiliSway.vNext.Subjects
{
	public class GroupViewModel : Screen
	{
		public GroupViewModel(Group group)
		{
			Group = group;
		}

		public string Title { get { return Group.Title; } set { Group.Title = value; NotifyOfPropertyChange(() => Title); } }
		public string Description { get { return Group.Description; } set { Group.Description = value; NotifyOfPropertyChange(() => Description); } }

		public Group Group { get; private set; }
	}
}
