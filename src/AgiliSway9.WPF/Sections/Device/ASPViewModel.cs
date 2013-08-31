using AgiliSway9.WPF.Services;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgiliSway9.WPF.Sections.Device
{
	public class ASPViewModel : DeviceConfigScreen
	{
		public ASPViewModel()
		{
			this.DisplayName = "ASP";
			this.DeviceType = DeviceTypes.ASP;
		}
	}
}
