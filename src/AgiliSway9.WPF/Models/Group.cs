using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgiliSway9.WPF.Models
{
    public class Group
    {
        public int GroupId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

		public Study Study { get; set; }
    }
}
