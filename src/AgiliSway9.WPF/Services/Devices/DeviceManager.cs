//using AgiliSway9.WPF.Models;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiliSway9.WPF.Services.Devices
{
	public class DeviceManager : IDeviceManager
	{
		private readonly IAppPreferences _appPreferences;

		public DeviceTypes DeviceType { get { return _inlDeviceType; } set { _inlDeviceType = value; _appPreferences.DeviceType = _inlDeviceType.ToString(); Bind(); } } private DeviceTypes _inlDeviceType;
		public string DeviceConfig { get { return _inlDeviceConfig; } set { _inlDeviceConfig = value; Bind(); } } private string _inlDeviceConfig;

		public IDeviceConnector CurrentConnector { get { return _inlCurrentConnector; } set { _inlCurrentConnector = value; } } private IDeviceConnector _inlCurrentConnector;
		
		[Inject]
		public DeviceManager(IAppPreferences appPreferences)
		{
			// Pull device and config prefs out
			_appPreferences = appPreferences;

			// If have previously set a device then try to auto connect to this one
			// else just sit there and look pretty
			if (!string.IsNullOrEmpty(_appPreferences.DeviceType))
			{
				// This is the only time we should be using the private internal vars so as to not cause echos
				_inlDeviceType = (DeviceTypes)Enum.Parse(typeof(DeviceTypes), _appPreferences.DeviceType, true);
				_inlDeviceConfig = _appPreferences.DeviceConfig;

				Bind();
			}
			else
			{
				_inlDeviceType = DeviceTypes.None;
				_inlDeviceConfig = "";

				_inlCurrentConnector = null;
			}
		}

		private void Bind()
		{
			// Fire up our new device
			switch (DeviceType)
			{
				case DeviceTypes.SIM:
					CurrentConnector = new SIMConnector();
					break;

				case DeviceTypes.WBB:
					CurrentConnector = new WBBConnector();
					break;

				//case DeviceTypes.ASP:
				//	CurrentDevice = ASPLocator.Find(DeviceConfig);
				//	break;
			}
		}
	}
}