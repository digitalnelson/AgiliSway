using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgiliSway.vNext.Models
{
    public class Task
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }

		public Study Study { get; set; }
    }
}
