using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgiliSway9.WPF.Models;

namespace AgiliSway9.WPF.Events
{
	public class SubjectSelectedEvent
	{
		public Subject Subject { get; set; }

		public string Title { get; set; }
	}
}
