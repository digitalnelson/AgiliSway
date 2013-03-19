using AgiliSway.vNext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AgiliSway.vNext.Services.Devices
{
	public class SIMDevice : IDevice
	{
        private int _max = 100;
        private Random _rnd;

        public SIMDevice()
        {
            _rnd = new Random();
        }

		public SIMDevice(string deviceConfig) : this()
		{}

		public void Connect()
		{}

		public void Disconnect()
		{}

        private int CreatePoint()
        {
            return _rnd.Next(0, _max);
        }

        private int CreatePoint(int start)
        {
            var variance = (int)(start * .15);
            var min = 0;
            var max = 100;
            if(start - variance > 0)
                min = start - variance;
            if(start + variance < _max)
                max = start + variance;

            return _rnd.Next(min, max);
        }

        public async Task<CollectionDataSet> Collect(TimeSpan time, IProgress<CollectProgress> progress, CancellationToken ct, CollectionDataPoint calibration)
        {
			var started = DateTime.UtcNow;

			CollectionDataSet collection = new CollectionDataSet();
            collection.TimestampUtc = started;

			var elapsed = DateTime.UtcNow - started;
            while(elapsed < time && !ct.IsCancellationRequested)
            {
                var initial = CreatePoint();
                var point = new CollectionDataPoint
                {
                    BottomLeft = new CollectionValue { Z = CreatePoint(initial) },
                    BottomRight = new CollectionValue { Z = CreatePoint(initial) },
                    TopLeft = new CollectionValue { Z = CreatePoint(initial) },
                    TopRight = new CollectionValue { Z = CreatePoint(initial) },
                    TimestampUtc = DateTime.UtcNow
                };
				collection.PointSet.Add(point);

				var p = new CollectProgress{
					Point = COP(point.TopLeft.Z.Value, point.TopRight.Z.Value, point.BottomLeft.Z.Value, point.BottomRight.Z.Value),
					Elapsed = elapsed,
					Desired = time,
				};
				progress.Report(p);

                await System.Threading.Tasks.Task.Delay(25);
				elapsed = DateTime.UtcNow - started;
            }

			return collection;
        }
		
		public DeviceState DeviceState
		{
			get
			{
				return DeviceState.Disconnected;
			}
			set { }
		}

        // length between board sensors
        private const float BSL = 24;
        // width between board sensors
        private const float BSW = 43;

        public Point COP(int topLeft, int topRight, int bottomLeft, int bottomRight)
        {
            Point pt = new Point
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
