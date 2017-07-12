using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPFDoodleBoard.DoodleShape
{
    public sealed class Arrow : Shape
    {
        #region Dependency Properties

        public static readonly DependencyProperty X1Property = DependencyProperty.Register("X1", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty X2Property = DependencyProperty.Register("X2", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion

        #region CLR Properties

        [TypeConverter(typeof(LengthConverter))]
        public double X1
        {
            get { return (double)base.GetValue(X1Property); }
            set { base.SetValue(X1Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Y1
        {
            get { return (double)base.GetValue(Y1Property); }
            set { base.SetValue(Y1Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double X2
        {
            get { return (double)base.GetValue(X2Property); }
            set { base.SetValue(X2Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Y2
        {
            get { return (double)base.GetValue(Y2Property); }
            set { base.SetValue(Y2Property, value); }
        }

        #endregion

        #region Overrides

        protected override Geometry DefiningGeometry
        {
            get
            {
                // Create a StreamGeometry for describing the shape
                StreamGeometry geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;

                using (StreamGeometryContext context = geometry.Open())
                {
                    InternalDrawArrowGeometry(context);
                }

                // Freeze the geometry for performance benefits
                geometry.Freeze();

                return geometry;
            }
        }

        #endregion

        #region Privates

        private void InternalDrawArrowGeometry(StreamGeometryContext context)
        {
            Point p1, p2, p3, p4;
            p1 = new Point(X1, Y1);
            p2 = new Point(X2, Y2);
            double slopy, cosy, siny;
            double length = StrokeThickness * 5;
            double width = StrokeThickness * 5;
            double distance = getDistance(p1, p2);
            if (width >= distance * 0.5)
            {
                length = width = distance * 0.5;
            }
            slopy = Math.Atan2(Y1 - Y2, X1 - X2);
            cosy = Math.Cos(slopy);
            siny = Math.Sin(slopy);

            p3 = new Point(p2.X + (length * cosy - (width / 2.0 * siny)), p2.Y + (length * siny + (width / 2.0 * cosy)));
            p4 = new Point(p2.X + (length * cosy + (width / 2.0 * siny)), p2.Y - ((width / 2.0 * cosy) - length * siny));

            context.BeginFigure(p1, true, false);
            context.LineTo(p2, true, true);
            context.LineTo(p3, true, true);
            context.LineTo(p2, true, true);
            context.LineTo(p4, true, true);
            context.LineTo(p2, true, true);
        }

        private double getDistance(Point p1, Point p2)
        {
            double num1 = Math.Pow(p1.X - p2.X, 2);
            double num2 = Math.Pow(p1.Y - p2.Y, 2);
            double distance = Math.Sqrt(num1 + num2);
            return distance;
        }
        #endregion
    }
}
