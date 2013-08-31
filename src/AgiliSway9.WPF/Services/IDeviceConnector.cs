using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiliSway9.WPF.Services
{
	public interface IDeviceConnector
	{
		IDevice CurrentDevice { get; set; }
	}
}
