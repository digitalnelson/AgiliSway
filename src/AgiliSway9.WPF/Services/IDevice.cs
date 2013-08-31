using AgiliSway9.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AgiliSway9.WPF.Services
{
	public interface IDevice
	{
		DeviceState DeviceState { get; set; }

		void Connect();
		void Disconnect();

		Task<CollectionDataSet> Collect(TimeSpan time, IProgress<CollectProgress> progress, CancellationToken ct, CollectionDataPoint calibration);
	}

	public enum DeviceState
	{
		Connected,
		Collecting,
		Connecting,
		Disconnected
	}
}
