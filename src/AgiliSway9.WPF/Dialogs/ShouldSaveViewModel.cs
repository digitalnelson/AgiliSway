using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace AgiliSway9.WPF.Dialogs
{
	class ShouldSaveViewModel : Screen
	{
		public void Save()
		{
			this.TryClose(true);
		}

		public void Cancel()
		{
			this.TryClose(false);
		}
	}
}
