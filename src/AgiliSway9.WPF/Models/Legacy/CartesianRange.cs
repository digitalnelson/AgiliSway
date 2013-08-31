using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgiliSway9.WPF.Models.Legacy
{
    public class CartesianRange
    {
        public CartesianPair MinX = new CartesianPair() { X = Double.MaxValue, Y = 0 };
        public CartesianPair MaxX = new CartesianPair() { X = Double.MinValue, Y = 0 };
        public CartesianPair MinY = new CartesianPair() { X = 0, Y = Double.MaxValue };
        public CartesianPair MaxY = new CartesianPair() { X = 0, Y = Double.MinValue };

        public void UpdatePair(double x, double y)
        {
            if (x < MinX.X)
                MinX = new CartesianPair(x, y);
            if (x > MaxX.X)
                MaxX = new CartesianPair(x, y);
            if (y < MinY.Y)
                MinY = new CartesianPair(x, y);
            if (y > MaxY.Y)
                MaxY = new CartesianPair(x, y);
        }
    }
}
