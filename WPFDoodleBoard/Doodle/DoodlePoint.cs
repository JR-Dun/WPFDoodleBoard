using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFDoodleBoard.Doodle
{
    public class DoodlePoint
    {
        public double X;
        public double Y;

        public DoodlePoint(Point point)
        {
            X = point.X;
            Y = point.Y;
        }
        
        public Point GetPoint
        {
            get
            {
                return new Point(X, Y);
            }
        }
    }
}
