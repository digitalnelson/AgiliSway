using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AgiliSway9.WPF.Services.Storage
{
	public class AppPreferencesIS : IAppPreferences
	{
		private string _fileName = "AppPrefs.txt";

		public string DataStorePath { get; set; }
		public string DeviceType { get; set; }
		public string DeviceConfig { get; set; }

		public void Load()
		{
			// Restore application-scope property from isolated storage
			IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();
			try
			{
				using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(_fileName, FileMode.Open, storage))
				using (StreamReader reader = new StreamReader(stream))
				{
					// Restore each application-scope property individually
					while (!reader.EndOfStream)
					{
						string[] keyValue = reader.ReadLine().Split(new char[] { ',' });

						if (keyValue[0] == "DataStorePath")
							DataStorePath = keyValue[1];
						else if (keyValue[0] == "DeviceType")
							DeviceType = keyValue[1];
						else if (keyValue[0] == "DeviceConfig")
							DeviceConfig = keyValue[1];
						else
							Application.Current.Properties[keyValue[0]] = keyValue[1];
					}
				}
			}
			catch (FileNotFoundException)
			{}
		}
		
		public void Save()
		{
			// Persist application-scope property to isolated storage
			IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();
			using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(_fileName, FileMode.Create, storage))
			using (StreamWriter writer = new StreamWriter(stream))
			{
				writer.WriteLine("{0},{1}", "DataStorePath", DataStorePath);
				writer.WriteLine("{0},{1}", "DeviceType", DeviceType);
				writer.WriteLine("{0},{1}", "DeviceConfig", DeviceConfig);

				// Persist each application-scope property individually
				foreach (string key in Application.Current.Properties.Keys)
				{
					writer.WriteLine("{0},{1}", key, Application.Current.Properties[key]);
				}
			}
		}
	}
}
