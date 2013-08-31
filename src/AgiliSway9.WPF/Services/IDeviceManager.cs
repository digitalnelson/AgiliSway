using AgiliSway9.WPF.Services.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiliSway9.WPF.Services
{
	public interface IDeviceManager
	{
		DeviceTypes DeviceType { get; set; }
		string DeviceConfig { get; set; }

		IDeviceConnector CurrentConnector { get; set; }
	}

	public enum DeviceTypes
	{
		None,
		WBB,
		ASP,
		SIM
	}
}
