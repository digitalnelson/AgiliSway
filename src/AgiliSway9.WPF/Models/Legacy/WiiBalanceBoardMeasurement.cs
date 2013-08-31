using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AgiliSway9.WPF.Models.Legacy
{
    public class WiiBalanceBoardMeasurementGroup
    {
        public int StartIndex;
        public WiiBalanceBoardMeasurement[] Measurements = new WiiBalanceBoardMeasurement[0];
    }

	public struct PointF
	{
		public float X;
		public float Y;
	}

    public class WiiBalanceBoardMeasurement
    {
        public long Ticks;
        public int TopLeft {get; set;}
		public int TopRight { get; set; }
		public int BottomLeft { get; set; }
		public int BottomRight { get; set; }

        // length between board sensors
        private const float BSL = 24;
        // width between board sensors
        private const float BSW = 43;

        public PointF COP(int topLeft, int topRight, int bottomLeft, int bottomRight)
        {
            PointF pt;
            pt.X = 0;
            pt.Y = 0;

            //float Kx = (TopLeft + BottomLeft) / (TopRight + BottomRight);
            //float Ky = (TopLeft + TopRight) / (BottomLeft + BottomRight);

            //pt.X = ((float)(Kx - 1) / (float)(Kx + 1)) * (float)(-BSL / 2);
            //pt.Y = ((float)(Ky - 1) / (float)(Ky + 1)) * (float)(-BSW / 2);

            int Fz = topLeft + topRight + bottomLeft + bottomRight;

            if (Fz >= 50)
            {
                pt.X = ((BSW / 2.0f) * (float)(topRight + bottomRight - topLeft - bottomLeft)) / (float)Fz;
				pt.Y = ((BSL / 2.0f) * (float)(topLeft + topRight - bottomLeft - bottomRight)) / (float)Fz;
            }

			return pt;
        }

        public PointF COP()
        {
            return COP(TopLeft, TopRight, BottomLeft, BottomRight);
        }

        public PointF COP(WiiBalanceBoardMeasurement cal)
        {
            return COP(TopLeft - cal.TopLeft, TopRight - cal.TopRight, BottomLeft - cal.BottomLeft, BottomRight - cal.BottomRight);
        }

        public double WeightKg(WiiBalanceBoardMeasurement cal)
        {
            var totalKg = ((TopLeft - cal.TopLeft) + (TopRight - cal.TopRight) + (BottomLeft - cal.BottomLeft) +
                           (BottomRight - cal.BottomRight))/100.0;
            return totalKg;
        }

        public double WeightLbs(WiiBalanceBoardMeasurement cal)
        {
            return WeightKg(cal) * 2.20462262;
        }
    }
}
