﻿using AgiliSway9.WPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiliSway9.WPF.Sections.Device
{
	public class NULLViewModel : DeviceConfigScreen
	{
		public NULLViewModel()
		{
			this.DisplayName = "NONE";
			this.DeviceType = DeviceTypes.None;
		}
	}
}
