using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiimoteLib;

namespace AgiliSway9.WPF.Services.Devices
{
	public class WBBConnector : Screen, IDeviceConnector
	{
        private bool _searchForDevices = false;

        public string DeviceStatus { get { return _inlDeviceStatus; } set { _inlDeviceStatus = value; NotifyOfPropertyChange(() => DeviceStatus); } } private string _inlDeviceStatus;
		public IDevice CurrentDevice { get { return _inlCurrentDevice; } set { _inlCurrentDevice = value; } } private IDevice _inlCurrentDevice;

        public WBBConnector()
        {
            // TODO: May need a cancellation token so the process can die
            // TODO: Will need to figure out what happens if the board disconnects (i.e. runs out of batteries)
            Task.Run(async () =>
            {
                _searchForDevices = true;
                while (_searchForDevices)
                {
                    if (CurrentDevice == null)
                    {
                        try
                        {
                            var dev = GetFirstOrNull();
                            if (dev != null)
                                CurrentDevice = dev;
                        }
                        catch (Exception ex)
                        {
                            DeviceStatus = "Device Not Found.\n\nError: " + ex.Message;  // TODO: Add this message to some bindable property so we can put it in the UI somewhere
                        }
                    }

                    await Task.Delay(1000);
                }
            });
        }

        public IDevice GetFirstOrNull()
        {
            var motes = new WiimoteCollection();
            motes.FindAllWiimotes();

            foreach (var mote in motes)
            {
                try
                {
                    var dev = IoC.Get<WBBDevice>();
                    dev.Device = mote;

                    dev.Connect();

                    if (dev.DeviceState == DeviceState.Connected)
                        return dev;
                }
                catch (Exception) { }
            }

            return null;
        }

		public List<IDevice> All()
		{
			List<IDevice> devices = new List<IDevice>();

			try
			{
				var motes = new WiimoteCollection();
				motes.FindAllWiimotes();

				foreach (var mote in motes)
				{
					try
					{
						// TODO: If device matches or if deviceConfig is empty
						var dev = IoC.Get<WBBDevice>();
						dev.Device = mote;

						devices.Add(dev);
					}
					catch (Exception exConnect) { }
				}
			}
			catch (Exception ex)
			{
				// TODO: Pass the error message on but don't throw cause this may be called many times
			}

			return devices;
		}

		public IDevice Find(string deviceConfig)
		{
			try
			{
				var motes = new WiimoteCollection();
				motes.FindAllWiimotes();

				foreach (var mote in motes)
				{
					try
					{
						// TODO: If device matches or if deviceConfig is empty
						var dev = IoC.Get<WBBDevice>();
						dev.Device = mote;

						return dev;
					}
					catch (Exception exConnect) { }
				}
			}
			catch (Exception ex)
			{
				// TODO: Pass the error message on but don't throw cause this may be called many times
			}

			return null;
		}

		public void Connect()
		{
			
		}

		public void Disconnect()
		{
			
		}
	}
}
