using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgiliSway.vNext.Models;
using AgiliSway.vNext.Models.Legacy;

namespace AgiliSway.vNext.ModelCalculations
{
	public class SwayPosition
	{
		public static PointF Calculate(CollectionDataPoint calibrationDataPoint, CollectionDataPoint collectionDataPoint)
		{
			PointF pt;
			pt.X = 0;
			pt.Y = 0;

            var cdp = collectionDataPoint;
            var adp = calibrationDataPoint;

			// TODO: Check for nulls and do COP if we can e.g. Accusway Plus

			// TODO: Otherwise just calculate the COB
            var tl = adp.TopLeft.Z != null ? collectionDataPoint.TopLeft.Z - adp.TopLeft.Z : cdp.TopLeft.Z;
            var tr = adp.TopRight.Z != null ? collectionDataPoint.TopRight.Z - adp.TopRight.Z : cdp.TopRight.Z;
            var bl = adp.BottomLeft.Z != null ? collectionDataPoint.BottomLeft.Z - adp.BottomLeft.Z : cdp.BottomLeft.Z;
            var br = adp.BottomRight.Z != null ? collectionDataPoint.BottomRight.Z - adp.BottomRight.Z : cdp.BottomRight.Z;

			var Fz = tl + tr + bl + br;

			if (Fz >= 50)
			{
				pt.X = ((BSW / 2.0f) * (float)(tr + br - tl - bl)) / (float)Fz;
				pt.Y = ((BSL / 2.0f) * (float)(tl + tr - bl - br)) / (float)Fz;
			}

			return pt;
		}

		// length between board sensors
		private const float BSL = 24;
		// width between board sensors
		private const float BSW = 43;
	}
}
