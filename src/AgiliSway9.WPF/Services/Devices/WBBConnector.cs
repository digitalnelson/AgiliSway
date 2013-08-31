using AgiliSway9.WPF.Events;
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
		private IEventAggregator _eventAggregator;

		public string DeviceStatus { get { return _inlDeviceStatus; } set { _inlDeviceStatus = value; NotifyOfPropertyChange(() => DeviceStatus); } } private string _inlDeviceStatus;
		public IDevice CurrentDevice { get { return _inlCurrentDevice; } set { _inlCurrentDevice = value; } } private IDevice _inlCurrentDevice;

		public WBBConnector()
		{
			this._eventAggregator = IoC.Get<IEventAggregator>();

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
							this._eventAggregator.Publish(new WiiBBSearchEvent()
							{
								SearchState = WiiBBSearchState.Searching,
								Message = "Starting search for available devices."
							});

							var motes = new WiimoteCollection();
							motes.FindAllWiimotes();

							WBBDevice device = (WBBDevice)null;
							foreach (var mote in motes)
							{
								try
								{
									this._eventAggregator.Publish(new WiiBBSearchEvent()
									{
										SearchState = WiiBBSearchState.Found,
										Message = "Found device."
									});

									device = IoC.Get<WBBDevice>();
									device.Device = mote;
									this._eventAggregator.Publish(new WiiBBSearchEvent()
									{
										SearchState = WiiBBSearchState.Connecting,
										Message = "Connecting to device."
									});

									device.Connect();
									if (device.DeviceState == DeviceState.Connected)
									{
										this._eventAggregator.Publish(new WiiBBSearchEvent()
										{
											SearchState = WiiBBSearchState.Connected,
											Message = "Connected to device."
										});
										break;
									}
									else
										device = (WBBDevice)null;
								}
								catch (Exception ex)
								{
									this._eventAggregator.Publish(new WiiBBSearchEvent()
									{
										SearchState = WiiBBSearchState.Error,
										Message = ("Error with device: " + ex.Message)
									});
								}
							}

							if (device != null)
								this.CurrentDevice = (IDevice)device;
						}
						catch (Exception exception_2)
						{
							this.DeviceStatus = "Device Not Found.\n\nError: " + exception_2.Message;
							this.CurrentDevice = (IDevice)null;
							this._eventAggregator.Publish(new WiiBBSearchEvent()
							{
								SearchState = WiiBBSearchState.Error,
								Message = this.DeviceStatus
							});
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
