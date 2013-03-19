using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgiliSway.vNext.Models
{
    public class Study
    {
		public Study()
		{
			Subjects = new List<Subject>();
			Tasks = new List<Task>();
			Groups = new List<Group>();
		}

        public int StudyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string DataFolder { get; set; }

		public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}
