using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;
using Ninject;
using Xceed.Wpf.Toolkit;
using AgiliSway.vNext.Services;
using AgiliSway.vNext.Services.Devices;
using AgiliSway.vNext.Services.Storage;

namespace AgiliSway.vNext
{
	public class NinjectBootstrapper : Bootstrapper<ShellViewModel>
	{
		protected override void Configure()
		{
			ConventionManager.AddElementConvention<DateTimePicker>(DateTimePicker.ValueProperty, "Text", "TextChanged");
			//ConventionManager.AddElementConvention<DateTimePicker>(DateTimePicker.SelectedTextProperty, "SelectedText", " SelectedTextChanged");

			_kernel = new StandardKernel();

			_kernel.Bind<IWindowManager>().To<WindowManager>().InSingletonScope();
			_kernel.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
			_kernel.Bind<IShell>().To<ShellViewModel>().InSingletonScope();
			_kernel.Bind<IDbStorage>().To<LocalStorageServiceSqlCompact>().InSingletonScope();
			_kernel.Bind<IAppPreferences>().To<AppPreferencesIS>().InSingletonScope();
			_kernel.Bind<IFileStorage>().To<FileStorageLocal>().InSingletonScope();
			_kernel.Bind<IDeviceManager>().To<DeviceManager>().InSingletonScope();
		}

		protected override object GetInstance(Type serviceType, string key)
		{
			if (serviceType != null)
				return _kernel.Get(serviceType, key);
			else
				return null;
		}

		protected override IEnumerable<object> GetAllInstances(Type serviceType)
		{
			return _kernel.GetAll(serviceType);
		}

		private IKernel _kernel;
	}
}
