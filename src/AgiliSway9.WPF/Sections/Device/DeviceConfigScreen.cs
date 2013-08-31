using AgiliSway9.WPF.Events;
using AgiliSway9.WPF.Services;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiliSway9.WPF.Sections.Device
{
	public class DeviceConfigScreen : Screen
	{
		#region Service Injection
		protected readonly IEventAggregator _eventAggregator = IoC.Get<IEventAggregator>();
		protected readonly IFileStorage _fileStorage = IoC.Get<IFileStorage>();
		protected readonly IDeviceManager _deviceManager = IoC.Get<IDeviceManager>();
		#endregion

		public DeviceTypes DeviceType { get; protected set; }

		public DeviceConfigScreen()
		{}

		protected override void OnActivate()
		{
			base.OnActivate();

			if (_deviceManager.DeviceType != this.DeviceType)
			{
				_deviceManager.DeviceType = this.DeviceType;

				_eventAggregator.Publish(new DeviceSelected { DeviceType = _deviceManager.DeviceType });
			}
		}
	}
}
