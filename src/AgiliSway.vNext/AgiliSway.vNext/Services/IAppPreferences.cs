using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiliSway.vNext.Services
{
	public interface IAppPreferences
	{
		string DataStorePath { get; set; }
		string DeviceType { get; set; }
		string DeviceConfig { get; set; }

		void Load();
		void Save();
	}
}
