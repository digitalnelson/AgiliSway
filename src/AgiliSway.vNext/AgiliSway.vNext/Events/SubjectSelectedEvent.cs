using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgiliSway.vNext.Models;

namespace AgiliSway.vNext.Event
{
	public class SubjectSelectedEvent
	{
		public Subject Subject { get; set; }

		public string Title { get; set; }
	}
}
