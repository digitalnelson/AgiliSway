using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using AgiliSway9.WPF.Services;

namespace AgiliSway9.WPF
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

			var prefs = IoC.Get<IAppPreferences>();

			prefs.Load();

			if (string.IsNullOrEmpty(prefs.DataStorePath))
			{
				System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
				fbd.Description = "Choose location for AgiliSway data store.  The study database, sway files, and reports will be stored in this location.";

				// Show open file dialog box
				System.Windows.Forms.DialogResult result = fbd.ShowDialog();

				// Process open file dialog box results
				if (result == System.Windows.Forms.DialogResult.OK)
				{
					prefs.DataStorePath = fbd.SelectedPath;
				}
				else
					System.Windows.Application.Current.Shutdown();
			}

			base.OnStartup(e);
		}

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show("Error: " + e.Exception.Message);

            System.Windows.Application.Current.Shutdown();
        }

		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);

			IoC.Get<IAppPreferences>().Save();
		}
	}
}
