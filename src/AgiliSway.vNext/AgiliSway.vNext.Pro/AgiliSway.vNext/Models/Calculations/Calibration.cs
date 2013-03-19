using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgiliSway.vNext.Models;

namespace AgiliSway.vNext.ModelCalculations
{
	public class Calibration
	{
		public static CollectionDataPoint Calculate(IList<CollectionDataPoint> calibrationDataPoints)
		{
			CollectionDataPoint sum = new CollectionDataPoint();

			sum.TopLeft.X = 0;
			sum.TopLeft.Y = 0;
			sum.TopLeft.Z = 0;

			sum.TopRight.X = 0;
			sum.TopRight.Y = 0;
			sum.TopRight.Z = 0;

			sum.BottomLeft.X = 0;
			sum.BottomLeft.Y = 0;
			sum.BottomLeft.Z = 0;

			sum.BottomRight.X = 0;
			sum.BottomRight.Y = 0;
			sum.BottomRight.Z = 0;

			var count = 0;
			foreach (var cal in calibrationDataPoints)
			{
				sum.TopLeft.X += cal.TopLeft.X;
				sum.TopLeft.Y += cal.TopLeft.Y;
				sum.TopLeft.Z += cal.TopLeft.Z;

				sum.TopRight.X += cal.TopRight.X;
				sum.TopRight.Y += cal.TopRight.Y;
				sum.TopRight.Z += cal.TopRight.Z;

				sum.BottomLeft.X += cal.BottomLeft.X;
				sum.BottomLeft.Y += cal.BottomLeft.Y;
				sum.BottomLeft.Z += cal.BottomLeft.Z;

				sum.BottomRight.X += cal.BottomRight.X;
				sum.BottomRight.Y += cal.BottomRight.Y;
				sum.BottomRight.Z += cal.BottomRight.Z;

				count++;
			}

			var avgCal = new CollectionDataPoint();

			if (count > 0)
			{
				avgCal.TopLeft.X = sum.TopLeft.X / count;
				avgCal.TopLeft.Y = sum.TopLeft.Y / count;
				avgCal.TopLeft.Z = sum.TopLeft.Z / count;

				avgCal.TopRight.X = sum.TopRight.X / count;
				avgCal.TopRight.Y = sum.TopRight.Y / count;
				avgCal.TopRight.Z = sum.TopRight.Z / count;

				avgCal.BottomLeft.X = sum.BottomLeft.X / count;
				avgCal.BottomLeft.Y = sum.BottomLeft.Y / count;
				avgCal.BottomLeft.Z = sum.BottomLeft.Z / count;

				avgCal.BottomRight.X = sum.BottomRight.X / count;
				avgCal.BottomRight.Y = sum.BottomRight.Y / count;
				avgCal.BottomRight.Z = sum.BottomRight.Z / count;
			}

			return avgCal;

		}
	}
}
