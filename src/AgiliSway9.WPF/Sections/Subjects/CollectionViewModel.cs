using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgiliSway9.WPF.Models;
using Caliburn.Micro;

namespace AgiliSway9.WPF.Subjects
{
	public class CollectionViewModel : Screen
	{
		public CollectionViewModel(Collection collection)
        {
            Collection = collection;
        }

		public int Id { get { return Collection.CollectionId; } set { Collection.CollectionId = value; NotifyOfPropertyChange(() => Id); } }		
		public string TaskTitle 
		{ 
			get 
			{
				if (Collection.Task != null)
					return Collection.Task.Title;
				else
					return "No Task Defined";
			} 
		}

		public string Timestamp
		{
			get
			{
				if (Collection.Timestamp != null)
					return Collection.Timestamp.ToString();
				else
					return "No Timestamp";
			}
		}

        public Collection Collection { get; private set; }
	}
}
