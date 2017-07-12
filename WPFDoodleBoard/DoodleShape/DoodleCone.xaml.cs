using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFDoodleBoard.DoodleShape
{
    /// <summary>
    /// DoodleCone.xaml 的交互逻辑
    /// </summary>
    public partial class DoodleCone : UserControl
    {
        public Point startPoint;
        public Point endPoint;
        public double strokeThickness;
        public Brush stroke;
        public double rotate;

        public DoodleCone(Point startPoint, Point endPoint, double strokeThickness, Brush stroke)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.strokeThickness = strokeThickness;
            this.stroke = stroke;

            InitializeComponent();

            SetMargin(startPoint, endPoint);
            DrawLine(startPoint, endPoint);
        }

        public void SetMargin(Point startPoint, Point endPoint)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;

            double left = startPoint.X < endPoint.X ? startPoint.X : endPoint.X;
            double top = startPoint.Y < endPoint.Y ? startPoint.Y : endPoint.Y;

            Margin = new Thickness(left, top, 0, 0);
        }

        public void DrawLine(Point startPoint, Point endPoint)
        {
            double w = Math.Abs(startPoint.X - endPoint.X);
            double h = Math.Abs(startPoint.Y - endPoint.Y);
            double x1 = w * 0;
            double x2 = w * 0.5;
            double x3 = w;

            double y1 = h * 0;
            double y2 = h * 0.15;
            double y3 = h * 0.85;
            double y4 = h;

            bgGrid.Width = w;
            bgGrid.Height = h;

            Line line1 = new Line()
            {
                X1 = x2,
                Y1 = y1,
                X2 = x1,
                Y2 = y3,
                StrokeThickness = strokeThickness,
                Stroke = stroke
            };
            Line line2 = new Line()
            {
                X1 = x2,
                Y1 = y1,
                X2 = x3,
                Y2 = y3,
                StrokeThickness = strokeThickness,
                Stroke = stroke
            };


            Path path3 = new Path();
            PathGeometry pathGeometry3 = new PathGeometry();
            ArcSegment arc3 = new ArcSegment(new Point(x3, y3), new Size(x2, y2), 0, false, SweepDirection.Counterclockwise, true);
            PathFigure figure3 = new PathFigure();
            figure3.StartPoint = new Point(x1, y3);
            figure3.Segments.Add(arc3);
            pathGeometry3.Figures.Add(figure3);
            path3.Data = pathGeometry3;
            path3.Stroke = stroke;
            path3.StrokeThickness = strokeThickness;

            Path path4 = new Path();
            PathGeometry pathGeometry4 = new PathGeometry();
            ArcSegment arc4 = new ArcSegment(new Point(x3, y3), new Size(x2, y2), 0, false, SweepDirection.Clockwise, true);
            PathFigure figure4 = new PathFigure();
            figure4.StartPoint = new Point(x1, y3);
            figure4.Segments.Add(arc4);
            pathGeometry4.Figures.Add(figure4);
            path4.Data = pathGeometry4;
            path4.Stroke = stroke;
            path4.StrokeThickness = strokeThickness;
            path4.StrokeDashArray = new DoubleCollection() { 3, 3 };


            container.Children.Clear();
            container.Children.Add(line1);
            container.Children.Add(line2);
            container.Children.Add(path3);
            container.Children.Add(path4);
        }

        public void SetBackground(Brush brush)
        {
            bgGrid.Background = brush;
        }

        public void SetPoints(Point startPoint, Point endPoint)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;

            SetMargin(startPoint, endPoint);
            DrawLine(startPoint, endPoint);
        }

        public void SetRotate(double rotate)
        {
            this.rotate = rotate;

            RotateTransform rotateTransform = bgGrid.RenderTransform as RotateTransform;
            rotateTransform.Angle = rotate;
            rotateTransform.CenterX = bgGrid.Width / 2;
            rotateTransform.CenterY = bgGrid.Height / 2;
        }
    }
}
