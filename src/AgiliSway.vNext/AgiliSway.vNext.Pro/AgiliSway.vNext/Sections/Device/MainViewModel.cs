using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Ninject;
using AgiliSway.vNext.Services;

namespace AgiliSway.vNext.Sections.Device
{
	public class MainViewModel : Conductor<IScreen>.Collection.OneActive
	{
		private readonly IAppPreferences _appPreferences;
		private readonly IDeviceManager _deviceManager;

		private IScreen _nullViewModel;
		private IScreen _wbbViewModel;
		private IScreen _aspViewModel;
		private IScreen _simViewModel;

		[Inject]
		public MainViewModel(IAppPreferences appPreferences, IDeviceManager deviceManager)
		{
			_appPreferences = appPreferences;
			_deviceManager = deviceManager;

			this.DisplayName = "DEVICES";

			_nullViewModel = IoC.Get<NULLViewModel>();
			_wbbViewModel = IoC.Get<WBBViewModel>();
			_aspViewModel = IoC.Get<ASPViewModel>();
			_simViewModel = IoC.Get<SIMViewModel>();

			Items.Add(_nullViewModel);
			Items.Add(_wbbViewModel);
			Items.Add(_aspViewModel);
			Items.Add(_simViewModel);
		}

		protected override void OnActivate()
		{
			base.OnActivate();

			var devType = _deviceManager.DeviceType;
			switch (devType)
			{
				case DeviceTypes.None:
					ActivateItem(_nullViewModel);
					break;
				case DeviceTypes.WBB:
					ActivateItem(_wbbViewModel);
					break;
				case DeviceTypes.ASP:
					ActivateItem(_aspViewModel);
					break;
				case DeviceTypes.SIM:
					ActivateItem(_simViewModel);
					break;
			}
		}
	}
}
