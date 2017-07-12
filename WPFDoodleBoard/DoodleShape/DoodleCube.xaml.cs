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
    /// DoodleCube.xaml 的交互逻辑
    /// </summary>
    public partial class DoodleCube : UserControl
    {
        public Point startPoint;
        public Point endPoint;
        public double strokeThickness;
        public Brush stroke;
        public double rotate;

        public DoodleCube(Point startPoint, Point endPoint, double strokeThickness, Brush stroke)
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
            double x2 = w * 0.2;
            double x3 = w * 0.8;
            double x4 = w;

            double y1 = h * 0;
            double y2 = h * 0.2;
            double y3 = h * 0.8;
            double y4 = h;

            bgGrid.Width = w;
            bgGrid.Height = h;

            Line line1 = new Line()
            {
                X1 = x1,
                Y1 = y2,
                X2 = x1,
                Y2 = y4,
                StrokeThickness = strokeThickness,
                Stroke = stroke
            };
            Line line2 = new Line()
            {
                X1 = x1,
                Y1 = y2,
                X2 = x3,
                Y2 = y2,
                StrokeThickness = strokeThickness,
                Stroke = stroke
            };
            Line line3 = new Line()
            {
                X1 = x3,
                Y1 = y2,
                X2 = x3,
                Y2 = y4,
                StrokeThickness = strokeThickness,
                Stroke = stroke
            };
            Line line4 = new Line()
            {
                X1 = x1,
                Y1 = y4,
                X2 = x3,
                Y2 = y4,
                StrokeThickness = strokeThickness,
                Stroke = stroke
            };


            Line line5 = new Line()
            {
                X1 = x1,
                Y1 = y2,
                X2 = x2,
                Y2 = y1,
                StrokeThickness = strokeThickness,
                Stroke = stroke
            };
            Line line6 = new Line()
            {
                X1 = x3,
                Y1 = y2,
                X2 = x4,
                Y2 = y1,
                StrokeThickness = strokeThickness,
                Stroke = stroke
            };
            Line line7 = new Line()
            {
                X1 = x3,
                Y1 = y4,
                X2 = x4,
                Y2 = y3,
                StrokeThickness = strokeThickness,
                Stroke = stroke
            };

            Line line8 = new Line()
            {
                X1 = x2,
                Y1 = y1,
                X2 = x4,
                Y2 = y1,
                StrokeThickness = strokeThickness,
                Stroke = stroke
            };
            Line line9 = new Line()
            {
                X1 = x4,
                Y1 = y1,
                X2 = x4,
                Y2 = y3,
                StrokeThickness = strokeThickness,
                Stroke = stroke
            };

            Line line10 = new Line()
            {
                X1 = x1,
                Y1 = y4,
                X2 = x2,
                Y2 = y3,
                StrokeThickness = strokeThickness,
                Stroke = stroke,
                StrokeDashArray = new DoubleCollection() { 3, 3 }
            };
            Line line11 = new Line()
            {
                X1 = x2,
                Y1 = y1,
                X2 = x2,
                Y2 = y3,
                StrokeThickness = strokeThickness,
                Stroke = stroke,
                StrokeDashArray = new DoubleCollection() { 3, 3 }
            };
            Line line12 = new Line()
            {
                X1 = x2,
                Y1 = y3,
                X2 = x4,
                Y2 = y3,
                StrokeThickness = strokeThickness,
                Stroke = stroke,
                StrokeDashArray = new DoubleCollection() { 3, 3 }
            };


            container.Children.Clear();
            container.Children.Add(line1);
            container.Children.Add(line2);
            container.Children.Add(line3);
            container.Children.Add(line4);
            container.Children.Add(line5);
            container.Children.Add(line6);
            container.Children.Add(line7);
            container.Children.Add(line8);
            container.Children.Add(line9);
            container.Children.Add(line10);
            container.Children.Add(line11);
            container.Children.Add(line12);
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
