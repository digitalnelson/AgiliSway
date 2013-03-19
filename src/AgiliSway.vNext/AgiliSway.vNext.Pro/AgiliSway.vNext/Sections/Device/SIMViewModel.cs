using AgiliSway.vNext.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiliSway.vNext.Sections.Device
{
	public class SIMViewModel : DeviceConfigScreen
	{	
		public SIMViewModel()
		{
			this.DisplayName = "SIM";
			this.DeviceType = DeviceTypes.SIM;
		}	
	}
}
