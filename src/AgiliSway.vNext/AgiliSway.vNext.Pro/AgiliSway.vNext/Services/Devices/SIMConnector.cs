using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiliSway.vNext.Services.Devices
{
	class SIMConnector : IDeviceConnector
	{
		public IDevice CurrentDevice { get { return _inlCurrentDevice; } set { _inlCurrentDevice = value; } } private IDevice _inlCurrentDevice;

		public SIMConnector()
		{
			CurrentDevice = new SIMDevice();
		}

		public void Connect()
		{
		}

		public void Disconnect()
		{
		}
	}
}
