using System;
using System.Collections;
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
using WPFDoodleBoard.Doodle;
using WPFDoodleBoard.DoodleShape;
using static WPFDoodleBoard.Doodle.DoodleEnum;

namespace WPFDoodleBoard
{
    /// <summary>
    /// DoodleControl.xaml 的交互逻辑
    /// </summary>
    public partial class DoodleControl : UserControl
    {
        public ArrayList doodleModels;
        public DoodleEnumColor lineColor;
        public DoodleEnumType drawType;
        public DoodleEnumBrushType brushType;
        public double lineWidth;

        public bool canDraw;
        public bool mouseDown;

        private UIElement element;
        private SolidColorBrush brushColor;


        public DoodleControl()
        {
            canDraw = true;
            InitializeComponent();
            //
            doodleModels = new ArrayList();

            SetDrawType(DoodleEnumType.draw);
            SetDrawColor(DoodleEnumColor.orange);
            SetDrawWidth(DoodleEnumBrushType.middle);


            shapeSetting.shapeSettingChange += shapeSetting_shapeSettingChange;
            shapeSetting.shapeSettingRotate += shapeSetting_shapeSettingRotate;
        }


        #region getter setter
        public void SetDrawType(DoodleEnumType type)
        {
            drawType = type;
        }
        public void SetDrawColor(DoodleEnumColor color)
        {
            lineColor = color;
            switch (lineColor)
            {
                case DoodleEnumColor.black:
                    brushColor = Brushes.Black;
                    break;
                case DoodleEnumColor.blue:
                    brushColor = Brushes.Blue;
                    break;
                case DoodleEnumColor.green:
                    brushColor = Brushes.Green;
                    break;
                case DoodleEnumColor.orange:
                    brushColor = Brushes.Orange;
                    break;
                case DoodleEnumColor.red:
                    brushColor = Brushes.Red;
                    break;
                default:
                    break;
            }
        }
        public void SetDrawWidth(DoodleEnumBrushType type)
        {
            brushType = type;
            switch (brushType)
            {
                case DoodleEnumBrushType.small:
                    lineWidth = 1.0;
                    break;
                case DoodleEnumBrushType.middle:
                    lineWidth = 2.0;
                    break;
                case DoodleEnumBrushType.big:
                    lineWidth = 4.0;
                    break;
                case DoodleEnumBrushType.bigger:
                    lineWidth = 8.0;
                    break;
                default:
                    break;
            }
        }

        private void SetElement(UIElement element, Point startPoint, Point endPoint)
        {
            if (element is DoodleCube)
            {
                DoodleCube cube = (DoodleCube)element;
                cube.SetPoints(startPoint, endPoint);
            }
            else if (element is DoodleCone)
            {
                DoodleCone cone = (DoodleCone)element;
                cone.SetPoints(startPoint, endPoint);
            }
            else if (element is DoodleCylinder)
            {
                DoodleCylinder cylinder = (DoodleCylinder)element;
                cylinder.SetPoints(startPoint, endPoint);
            }
        }

        private void SetRotate(UIElement element, double rotate)
        {
            if (element is DoodleCube)
            {
                DoodleCube cube = (DoodleCube)element;
                cube.SetRotate(rotate);
            }
            else if (element is DoodleCone)
            {
                DoodleCone cone = (DoodleCone)element;
                cone.SetRotate(rotate);
            }
            else if (element is DoodleCylinder)
            {
                DoodleCylinder cylinder = (DoodleCylinder)element;
                cylinder.SetRotate(rotate);
            }
        }

        #endregion

        #region life time
        private void doodleCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Clear();
            foreach (DoodleModel model in doodleModels)
            {
                drawWithModel(model);
            }
        }
        #endregion

        #region mouse method
        private void doodleCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!canDraw)
            {
                shapeSetting.SetVisibility(false);
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point point = e.GetPosition(doodleCanvas);
                drawWithBeganPoint(point);
                mouseDown = true;
            }
        }

        private void doodleCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown && e.LeftButton == MouseButtonState.Pressed)
            {
                Point point = e.GetPosition(doodleCanvas);
                drawWithMovePoint(point);
            }
        }

        private void doodleCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
            element = null;
        }

        #endregion

        #region doodle method
        private void drawWithBeganPoint(Point point)
        {
            DoodleModel model = new DoodleModel();
            model.points.Add(new DoodlePoint(getLogicPoint(point)));
            model.drawType = drawType;
            model.lineColor = lineColor;
            model.lineWidth = lineWidth;
            doodleModels.Add(model);

            switch (model.drawType)
            {
                case DoodleEnumType.eraser:
                    {
                    }
                    break;
                case DoodleEnumType.draw:
                    {
                        Path path = new Path()
                        {
                            StrokeThickness = getBrushWidth(lineWidth),
                            Stroke = getBrushColor(lineColor)
                        };
                        doodleCanvas.Children.Add(path);
                        element = path;
                    }
                    break;
                case DoodleEnumType.rect:
                    {
                        Rectangle rect = new Rectangle()
                        {
                            Margin = new Thickness(point.X, point.Y, 0, 0),
                            Width = 0,
                            Height = 0,
                            StrokeThickness = getBrushWidth(lineWidth),
                            Stroke = getBrushColor(lineColor)
                        };
                        doodleCanvas.Children.Add(rect);
                        element = rect;
                    }
                    break;
                case DoodleEnumType.line:
                    {
                        Line line = new Line()
                        {
                            X1 = point.X,
                            Y1 = point.Y,
                            X2 = point.X,
                            Y2 = point.Y,
                            StrokeThickness = getBrushWidth(lineWidth),
                            Stroke = getBrushColor(lineColor)
                        };

                        doodleCanvas.Children.Add(line);
                        element = line;
                    }
                    break;
                case DoodleEnumType.circle:
                    {
                        Ellipse circle = new Ellipse()
                        {
                            Margin = new Thickness(point.X, point.Y, 0, 0),
                            Width = 0,
                            Height = 0,
                            StrokeThickness = getBrushWidth(lineWidth),
                            Stroke = getBrushColor(lineColor)
                        };
                        doodleCanvas.Children.Add(circle);
                        element = circle;
                    }
                    break;
                case DoodleEnumType.arrow:
                    {
                        Arrow arrow = new Arrow()
                        {
                            X1 = point.X,
                            Y1 = point.Y,
                            X2 = point.X,
                            Y2 = point.Y,
                            StrokeThickness = getBrushWidth(lineWidth),
                            Stroke = getBrushColor(lineColor)
                        };
                        doodleCanvas.Children.Add(arrow);
                        element = arrow;
                    }
                    break;
                case DoodleEnumType.cube:
                    {
                        DoodleCube cone = new DoodleCube(point, point, getBrushWidth(lineWidth), getBrushColor(lineColor));
                        doodleCanvas.Children.Add(cone);
                        element = cone;
                    }
                    break;
                case DoodleEnumType.cylinder:
                    {
                        DoodleCylinder cone = new DoodleCylinder(point, point, getBrushWidth(lineWidth), getBrushColor(lineColor));
                        doodleCanvas.Children.Add(cone);
                        element = cone;
                    }
                    break;
                case DoodleEnumType.cone:
                    {
                        DoodleCone cone = new DoodleCone(point, point, getBrushWidth(lineWidth), getBrushColor(lineColor));
                        doodleCanvas.Children.Add(cone);
                        element = cone;
                    }
                    break;
                default:
                    break;
            }
        }

        private void drawWithMovePoint(Point point)
        {
            DoodleModel model = (DoodleModel)doodleModels[doodleModels.Count - 1];
            model.points.Add(new DoodlePoint(getLogicPoint(point)));

            switch (model.drawType)
            {
                case DoodleEnumType.eraser:
                    {
                    }
                    break;
                case DoodleEnumType.draw:
                    {
                        Path path = (Path)element;
                        doodleCanvas.Children.Remove(path);
                        PathFigure pathFigure = new PathFigure();
                        pathFigure.StartPoint = getLocalPoint(model.points[0].GetPoint);

                        List<Point> controls = new List<Point>();
                        for (int i = 0; i < model.points.Count; i++)
                        {
                            controls.AddRange(BezierControlPoints(model.points, i));
                        }
                        for (int i = 1; i < model.points.Count; i++)
                        {
                            BezierSegment bs = new BezierSegment(controls[i * 2 - 1], controls[i * 2], getLocalPoint(model.points[i].GetPoint), true);
                            bs.IsSmoothJoin = true;
                            pathFigure.Segments.Add(bs);
                        }
                        PathFigureCollection pathFigureCollection = new PathFigureCollection();
                        pathFigureCollection.Add(pathFigure);
                        PathGeometry pathGeometry = new PathGeometry(pathFigureCollection);
                        path.Data = pathGeometry;
                        doodleCanvas.Children.Add(path);
                    }
                    break;
                case DoodleEnumType.rect:
                    {
                        Point startPoint = getLocalPoint(model.startPoint.GetPoint);
                        Rectangle rect = (Rectangle)element;
                        rect.Margin = new Thickness(getSmallX(startPoint, point), getSmallY(startPoint, point), 0, 0);
                        rect.Width = Math.Abs(startPoint.X - point.X);
                        rect.Height = Math.Abs(startPoint.Y - point.Y);
                    }
                    break;
                case DoodleEnumType.line:
                    {
                        Point startPoint = getLocalPoint(model.startPoint.GetPoint);
                        Line line = (Line)element;
                        line.X1 = startPoint.X;
                        line.Y1 = startPoint.Y;
                        line.X2 = point.X;
                        line.Y2 = point.Y;
                    }
                    break;
                case DoodleEnumType.circle:
                    {
                        Point startPoint = getLocalPoint(model.startPoint.GetPoint);
                        Ellipse circle = (Ellipse)element;
                        circle.Margin = new Thickness(getSmallX(startPoint, point), getSmallY(startPoint, point), 0, 0);
                        circle.Width = Math.Abs(startPoint.X - point.X);
                        circle.Height = Math.Abs(startPoint.Y - point.Y);
                    }
                    break;
                case DoodleEnumType.arrow:
                    {
                        Point startPoint = getLocalPoint(model.startPoint.GetPoint);
                        Arrow arrow = (Arrow)element;
                        arrow.X1 = startPoint.X;
                        arrow.Y1 = startPoint.Y;
                        arrow.X2 = point.X;
                        arrow.Y2 = point.Y;
                    }
                    break;
                case DoodleEnumType.cube:
                    {
                        Point startPoint = getLocalPoint(model.startPoint.GetPoint);
                        DoodleCube cone = (DoodleCube)element;
                        cone.SetPoints(startPoint, point);
                    }
                    break;
                case DoodleEnumType.cylinder:
                    {
                        Point startPoint = getLocalPoint(model.startPoint.GetPoint);
                        DoodleCylinder cone = (DoodleCylinder)element;
                        cone.SetPoints(startPoint, point);
                    }
                    break;
                case DoodleEnumType.cone:
                    {
                        Point startPoint = getLocalPoint(model.startPoint.GetPoint);
                        DoodleCone cone = (DoodleCone)element;
                        cone.SetPoints(startPoint, point);
                    }
                    break;
                default:
                    break;
            }
        }

        private void drawWithModel(DoodleModel model)
        {
            switch (model.drawType)
            {
                case DoodleEnumType.eraser:
                    {
                    }
                    break;
                case DoodleEnumType.draw:
                    {
                        Path path = new Path()
                        {
                            StrokeThickness = getBrushWidth(model.lineWidth),
                            Stroke = getBrushColor(model.lineColor)
                        };
                        PathFigure pathFigure = new PathFigure();
                        pathFigure.StartPoint = getLocalPoint(model.points[0].GetPoint);

                        List<Point> controls = new List<Point>();
                        for (int i = 0; i < model.points.Count; i++)
                        {
                            controls.AddRange(BezierControlPoints(model.points, i));
                        }
                        for (int i = 1; i < model.points.Count; i++)
                        {
                            BezierSegment bs = new BezierSegment(controls[i * 2 - 1], controls[i * 2], getLocalPoint(model.points[i].GetPoint), true);
                            bs.IsSmoothJoin = true;
                            pathFigure.Segments.Add(bs);
                        }
                        PathFigureCollection pathFigureCollection = new PathFigureCollection();
                        pathFigureCollection.Add(pathFigure);
                        PathGeometry pathGeometry = new PathGeometry(pathFigureCollection);
                        path.Data = pathGeometry;
                        doodleCanvas.Children.Add(path);
                    }
                    break;
                case DoodleEnumType.rect:
                    {
                        Point startPoint = getLocalPoint(model.startPoint.GetPoint);
                        Point point = getLocalPoint(model.points[model.points.Count - 1].GetPoint);
                        Rectangle rect = new Rectangle()
                        {
                            Margin = new Thickness(getSmallX(startPoint, point), getSmallY(startPoint, point), 0, 0),
                            Width = Math.Abs(startPoint.X - point.X),
                            Height = Math.Abs(startPoint.Y - point.Y),
                            StrokeThickness = getBrushWidth(model.lineWidth),
                            Stroke = getBrushColor(model.lineColor)
                        };
                        doodleCanvas.Children.Add(rect);
                    }
                    break;
                case DoodleEnumType.line:
                    {
                        Point startPoint = getLocalPoint(model.startPoint.GetPoint);
                        Point point = getLocalPoint((Point)model.points[model.points.Count - 1].GetPoint);
                        Line line = new Line()
                        {
                            X1 = startPoint.X,
                            Y1 = startPoint.Y,
                            X2 = point.X,
                            Y2 = point.Y,
                            StrokeThickness = getBrushWidth(model.lineWidth),
                            Stroke = getBrushColor(model.lineColor)
                        };

                        doodleCanvas.Children.Add(line);
                    }
                    break;
                case DoodleEnumType.circle:
                    {
                        Point startPoint = getLocalPoint(model.startPoint.GetPoint);
                        Point point = getLocalPoint((Point)model.points[model.points.Count - 1].GetPoint);
                        Ellipse circle = new Ellipse()
                        {
                            Margin = new Thickness(getSmallX(startPoint, point), getSmallY(startPoint, point), 0, 0),
                            Width = Math.Abs(startPoint.X - point.X),
                            Height = Math.Abs(startPoint.Y - point.Y),
                            StrokeThickness = getBrushWidth(model.lineWidth),
                            Stroke = getBrushColor(model.lineColor)
                        };
                        doodleCanvas.Children.Add(circle);
                    }
                    break;
                case DoodleEnumType.arrow:
                    {
                        Point startPoint = getLocalPoint(model.startPoint.GetPoint);
                        Point point = getLocalPoint(model.points[model.points.Count - 1].GetPoint);
                        Arrow arrow = new Arrow()
                        {
                            X1 = startPoint.X,
                            Y1 = startPoint.Y,
                            X2 = point.X,
                            Y2 = point.Y,
                            StrokeThickness = getBrushWidth(model.lineWidth),
                            Stroke = getBrushColor(model.lineColor)
                        };
                        doodleCanvas.Children.Add(arrow);
                    }
                    break;
                case DoodleEnumType.cube:
                    {
                        Point startPoint = getLocalPoint(model.startPoint.GetPoint);
                        Point point = getLocalPoint(model.points[model.points.Count - 1].GetPoint);
                        DoodleCube cone = new DoodleCube(startPoint, point, getBrushWidth(model.lineWidth), getBrushColor(model.lineColor));
                        doodleCanvas.Children.Add(cone);
                    }
                    break;
                case DoodleEnumType.cylinder:
                    {
                        Point startPoint = getLocalPoint(model.startPoint.GetPoint);
                        Point point = getLocalPoint(model.points[model.points.Count - 1].GetPoint);
                        DoodleCylinder cone = new DoodleCylinder(startPoint, point, getBrushWidth(model.lineWidth), getBrushColor(model.lineColor));
                        doodleCanvas.Children.Add(cone);
                    }
                    break;
                case DoodleEnumType.cone:
                    {
                        Point startPoint = getLocalPoint(model.startPoint.GetPoint);
                        Point point = getLocalPoint(model.points[model.points.Count - 1].GetPoint);
                        DoodleCone cone = new DoodleCone(startPoint, point, getBrushWidth(model.lineWidth), getBrushColor(model.lineColor));
                        doodleCanvas.Children.Add(cone);
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region invoke method
        private void Cube_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DoodleCube cube = (DoodleCube)sender;
            ShapeSettingSelected(cube);

            e.Handled = true;
        }

        private void Cylinder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DoodleCylinder cylinder = (DoodleCylinder)sender;
            ShapeSettingSelected(cylinder);
            e.Handled = true;
        }

        private void Cone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DoodleCone cone = (DoodleCone)sender;
            ShapeSettingSelected(cone);
            e.Handled = true;
        }

        private void ShapeSettingSelected(UIElement element)
        {
            if (element is DoodleCube)
            {
                DoodleCube cube = (DoodleCube)element;
                ShapeSettingSelected(cube, cube.startPoint, cube.endPoint, cube.rotate);
            }
            else if (element is DoodleCylinder)
            {
                DoodleCylinder cylinder = (DoodleCylinder)element;
                ShapeSettingSelected(cylinder, cylinder.startPoint, cylinder.endPoint, cylinder.rotate);
            }
            else if (element is DoodleCone)
            {
                DoodleCone cone = (DoodleCone)element;
                ShapeSettingSelected(cone, cone.startPoint, cone.endPoint, cone.rotate);
            }
        }

        private void ShapeSettingSelected(UIElement element, Point startPoint, Point endPoint, double rotate)
        {
            shapeSetting.SetPoint(element, startPoint, endPoint, rotate);
            shapeSetting.SetVisibility(true);
        }

        //shapeSetting
        private void shapeSetting_shapeSettingChange(UIElement element, Point startPoint, Point endPoint)
        {
            SetElement(element, startPoint, endPoint);
        }

        private void shapeSetting_shapeSettingRotate(UIElement element, double rotate)
        {
            SetRotate(element, rotate);
        }

        #endregion

        #region 贝塞尔
        private List<Point> BezierControlPoints(IList<DoodlePoint> list, int n)
        {
            List<Point> point = new List<Point>();
            point.Add(new Point());
            point.Add(new Point());
            if (n == 0)
            {
                point[0] = getLocalPoint(list[0].GetPoint);
            }
            else
            {
                point[0] = Average(getLocalPoint(list[n - 1].GetPoint), getLocalPoint(list[n].GetPoint));
            }
            if (n == list.Count - 1)
            {
                point[1] = getLocalPoint(list[list.Count - 1].GetPoint);
            }
            else
            {
                point[1] = Average(getLocalPoint(list[n].GetPoint), getLocalPoint(list[n + 1].GetPoint));
            }
            Point ave = Average(point[0], point[1]);
            Point sh = Sub(getLocalPoint(list[n].GetPoint), ave);
            point[0] = Mul(Add(point[0], sh), getLocalPoint(list[n].GetPoint), 0);//0.6
            point[1] = Mul(Add(point[1], sh), getLocalPoint(list[n].GetPoint), 0);//0.6

            return point;

        }

        private Point Average(Point x, Point y)
        {
            return new Point((x.X + y.X) / 2, (x.Y + y.Y) / 2);
        }

        private Point Add(Point x, Point y)
        {
            return new Point(x.X + y.X, x.Y + y.Y);
        }

        private Point Sub(Point x, Point y)
        {
            return new Point(x.X - y.X, x.Y - y.Y);
        }

        private Point Mul(Point x, Point y, double d)
        {
            Point temp = Sub(x, y);
            temp = new Point(temp.X * d, temp.Y * d);
            temp = Add(y, temp);
            return temp;
        }
        #endregion

        #region private method
        /// <summary>
        /// 实际坐标 转 逻辑坐标
        /// </summary>
        /// <param name="point"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private Point getLogicPoint(Point point)
        {
            Size size = new Size(doodleCanvas.ActualWidth, doodleCanvas.ActualHeight);
            double x = point.X / size.Width - 0.5;
            double y = point.Y / size.Height - 0.5;
            Point logicPoint = new Point(x, y);
            return logicPoint;
        }

        /// <summary>
        /// 逻辑坐标 转 实际坐标
        /// </summary>
        /// <param name="point"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private Point getLocalPoint(Point point)
        {
            Size size = new Size(doodleCanvas.ActualWidth, doodleCanvas.ActualHeight);
            double x = (point.X + 0.5) * size.Width;
            double y = (point.Y + 0.5) * size.Height;
            Point localPoint = new Point(x, y);
            return localPoint;
        }

        /// <summary>
        /// 获取两点间距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private double getDistance(Point p1, Point p2)
        {
            double num1 = Math.Pow(p1.X - p2.X, 2);
            double num2 = Math.Pow(p1.Y - p2.Y, 2);
            double distance = Math.Sqrt(num1 + num2);
            return distance;
        }

        private double getSmallX(Point p1, Point p2)
        {
            if (p1.X > p2.X)
            {
                return p2.X;
            }
            else
            {
                return p1.X;
            }
        }
        private double getSmallY(Point p1, Point p2)
        {
            if (p1.Y > p2.Y)
            {
                return p2.Y;
            }
            else
            {
                return p1.Y;
            }
        }

        /// <summary>
        /// 获取涂鸦的宽度
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        private double getBrushWidth(double w)
        {
            return w / 1000.0 * doodleCanvas.ActualWidth;
        }

        /// <summary>
        /// 获取涂鸦的颜色
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private SolidColorBrush getBrushColor(DoodleEnumColor color)
        {
            SolidColorBrush result = Brushes.Black;
            switch (color)
            {
                case DoodleEnumColor.black:
                    result = Brushes.Black;
                    break;
                case DoodleEnumColor.blue:
                    result = Brushes.Blue;
                    break;
                case DoodleEnumColor.green:
                    result = Brushes.Green;
                    break;
                case DoodleEnumColor.orange:
                    result = Brushes.Orange;
                    break;
                case DoodleEnumColor.red:
                    result = Brushes.Red;
                    break;
                default:
                    break;
            }

            return result;
        }
        #endregion

        #region public method
        /// <summary>
        /// 撤销
        /// </summary>
        public void Undo()
        {
            if (doodleModels.Count > 0)
            {
                Clear();
                doodleModels.RemoveAt(doodleModels.Count - 1);
                foreach (DoodleModel model in doodleModels)
                {
                    drawWithModel(model);
                }
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            doodleCanvas.Children.Clear();
        }

        /// <summary>
        /// 能否涂鸦
        /// </summary>
        /// <param name="value"></param>
        public void SetCanDraw(bool value = true)
        {
            canDraw = value;

            if (canDraw)
            {
                foreach (var child in doodleCanvas.Children)
                {
                    if (child is DoodleCube)
                    {
                        DoodleCube cube = (DoodleCube)child;
                        cube.SetBackground(Brushes.Transparent);
                        cube.MouseLeftButtonDown -= Cube_MouseLeftButtonDown;
                    }
                    else if (child is DoodleCone)
                    {
                        DoodleCone cone = (DoodleCone)child;
                        cone.SetBackground(Brushes.Transparent);
                        cone.MouseLeftButtonDown -= Cone_MouseLeftButtonDown;
                    }
                    else if (child is DoodleCylinder)
                    {
                        DoodleCylinder cylinder = (DoodleCylinder)child;
                        cylinder.SetBackground(Brushes.Transparent);
                        cylinder.MouseLeftButtonDown -= Cylinder_MouseLeftButtonDown;
                    }
                }
            }
            else
            {
                foreach (var child in doodleCanvas.Children)
                {
                    if (child is DoodleCube)
                    {
                        DoodleCube cube = (DoodleCube)child;
                        cube.SetBackground(Brushes.Transparent);
                        cube.MouseLeftButtonDown += Cube_MouseLeftButtonDown;
                    }
                    else if (child is DoodleCone)
                    {
                        DoodleCone cone = (DoodleCone)child;
                        cone.SetBackground(Brushes.Transparent);
                        cone.MouseLeftButtonDown += Cone_MouseLeftButtonDown;
                    }
                    else if (child is DoodleCylinder)
                    {
                        DoodleCylinder cylinder = (DoodleCylinder)child;
                        cylinder.SetBackground(Brushes.Transparent);
                        cylinder.MouseLeftButtonDown += Cylinder_MouseLeftButtonDown;
                    }
                }
            }
        }

        /// <summary>
        /// 能够选中
        /// </summary>
        /// <param name="value"></param>
        public void SetCanSelect(bool value = true)
        {
            SetCanDraw(!value);
        }
        #endregion

    }
}
