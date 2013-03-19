using AgiliSway.vNext.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiimoteLib;

namespace AgiliSway.vNext.Services.Devices
{
	public class WBBDevice : IDevice
	{
		public Wiimote Device { get { return _inlDevice; } set { _inlDevice = value; } } private Wiimote _inlDevice;

        private Random _rnd;
        private int _max = 100;

		public WBBDevice()
		{
            _rnd = new Random();
        }

		// TODO: Wait some amount of time and call again cause the device may not be running
		// TODO: This one is picky in that it may or may not connect based on the WBB button state
		// TODO: Should have some method of attacking the board until it connects or the outside manager
		// TODO: tells us to stop.

		// TODO: If things are really not working may need to send an event to the CM event bus letting the user know
		// TODO: some intervention is needed like pairing the device or selecting a different device cause this one is hosed.
		public void Connect()
		{
			try
			{
                try {
                    //Device.GetStatus();
                    Device.Disconnect(); 
                }
                catch (Exception) { }

                Device.Connect();

				Device.SetLEDs(true, false, false, false);

				Device.WiimoteChanged += new EventHandler<WiimoteChangedEventArgs>(managedMote_WiimoteChanged);

                DeviceState = Services.DeviceState.Connected;
			}
			catch (Exception)
			{ }
		}

		public void Disconnect()
		{
			try
			{
                DeviceState = Services.DeviceState.Disconnected;
				Device.Disconnect();
			}
			catch (Exception)
			{ }
		}

		public Models.CollectionDataPoint GetDataPoint()
		{
			throw new NotImplementedException();
		}

        public DeviceState DeviceState { get { return _inlDeviceState; } set { _inlDeviceState = value; } } private DeviceState _inlDeviceState;

        BlockingCollection<CollectionDataPoint> _dataPoints = new BlockingCollection<CollectionDataPoint>();

		void managedMote_WiimoteChanged(object sender, WiimoteChangedEventArgs e)
		{
			WiimoteState ws = e.WiimoteState;

            if (DeviceState == Services.DeviceState.Collecting)
            {
                _dataPoints.Add(new CollectionDataPoint
                {
                    TimestampUtc = DateTime.UtcNow,
                    BottomLeft = new CollectionValue { Z = e.WiimoteState.BalanceBoardState.SensorValuesRaw.BottomLeft },
                    BottomRight = new CollectionValue { Z = e.WiimoteState.BalanceBoardState.SensorValuesRaw.BottomRight },
                    TopLeft = new CollectionValue { Z = e.WiimoteState.BalanceBoardState.SensorValuesRaw.TopLeft },
                    TopRight = new CollectionValue { Z = e.WiimoteState.BalanceBoardState.SensorValuesRaw.TopRight },
                });
            }
		}

        public async Task<Models.CollectionDataSet> Collect(TimeSpan time, IProgress<Models.CollectProgress> progress, System.Threading.CancellationToken ct, CollectionDataPoint calibration)
		{
            var started = DateTime.UtcNow;

            CollectionDataSet collection = new CollectionDataSet();
            collection.TimestampUtc = started;

            _dataPoints = new BlockingCollection<CollectionDataPoint>();
            DeviceState = Services.DeviceState.Collecting;

            var elapsed = DateTime.UtcNow - started;
            while (elapsed < time && !ct.IsCancellationRequested)
            {
                CollectionDataPoint lastPoint = null;
                CollectionDataPoint outPoint = null;
                while (_dataPoints.TryTake(out outPoint, 0))
                {
                    collection.PointSet.Add(outPoint);
                    lastPoint = outPoint;
                }

                if (lastPoint != null)
                {
                    try
                    {
                        AgiliSway.vNext.Models.Point cop = null;
                        if (calibration != null)
                        {
                            cop = COP(lastPoint.TopLeft.Z.Value - calibration.TopLeft.Z.Value,
                                lastPoint.TopRight.Z.Value - calibration.TopRight.Z.Value,
                                lastPoint.BottomLeft.Z.Value - calibration.BottomLeft.Z.Value,
                                lastPoint.BottomRight.Z.Value - calibration.BottomRight.Z.Value);
                        }
                        else
                            cop = COP(lastPoint.TopLeft.Z.Value, lastPoint.TopRight.Z.Value, lastPoint.BottomLeft.Z.Value, lastPoint.BottomRight.Z.Value);

                        var p = new CollectProgress
                        {
                            Point = cop,
                            Elapsed = elapsed,
                            Desired = time,
                        };
                        progress.Report(p);
                    }
                    catch (Exception) { }
                }

                await System.Threading.Tasks.Task.Delay(50);
                elapsed = DateTime.UtcNow - started;
            }

            DeviceState = Services.DeviceState.Connected;

            return collection;
		}

        private int CreatePoint()
        {
            return _rnd.Next(0, _max);
        }

        private int CreatePoint(int start)
        {
            var variance = (int)(start * .15);
            var min = 0;
            var max = 100;
            if (start - variance > 0)
                min = start - variance;
            if (start + variance < _max)
                max = start + variance;

            return _rnd.Next(min, max);
        }

        // length between board sensors
        private const float BSL = 24;
        // width between board sensors
        private const float BSW = 43;

        public AgiliSway.vNext.Models.Point COP(int topLeft, int topRight, int bottomLeft, int bottomRight)
        {
            AgiliSway.vNext.Models.Point pt = new AgiliSway.vNext.Models.Point
            {
                X = 0,
                Y = 0
            };

            int Fz = topLeft + topRight + bottomLeft + bottomRight;

            if (Fz >= 50)
            {
                pt.X = ((BSW / 2.0f) * (float)(topRight + bottomRight - topLeft - bottomLeft)) / (float)Fz;
                pt.Y = ((BSL / 2.0f) * (float)(topLeft + topRight - bottomLeft - bottomRight)) / (float)Fz;
            }

            return pt;
        }
	}
}
