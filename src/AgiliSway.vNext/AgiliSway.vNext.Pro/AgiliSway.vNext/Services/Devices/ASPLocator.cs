using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiliSway.vNext.Services.Devices
{
	public class ASPLocator
	{
		public static IDevice Find(string deviceConfig)
		{
			try
			{
				var dev = IoC.Get<ASPDevice>();
				dev.PortName = deviceConfig; // TODO: pull individual parms out for more complex config

				return dev;
			}
			catch (Exception ex)
			{
				// TODO: Pass the error message on but don't throw cause this may be called many times
			}

			return null;
		}
	}
}
