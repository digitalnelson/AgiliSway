using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgiliSway.vNext.Models
{
    public class Subject
    {
		public Subject()
		{
			Collections = new List<Collection>();
		}

        public int SubjectId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ExternalId { get; set; }
        public string Birthdate { get; set; }
		public string Notes { get; set; }

		public int? StudyId { get; set; }
		public virtual Study Study { get; set; }

		public int? GroupId { get; set; }
		public virtual Group Group { get; set; }
		
		public virtual ICollection<Collection> Collections { get; set; }
    }
}
