using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiliSway9.WPF.Flyouts
{
	public class SaveChangesViewModel : Screen
	{
		public void Save()
		{
			IsVisible = false;
		}

		public void DontSave()
		{
			IsVisible = false;
		}

		public bool IsVisible { get { return _inlIsVisible; } set { _inlIsVisible = value; NotifyOfPropertyChange(() => IsVisible); } } private bool _inlIsVisible;
	}
}
