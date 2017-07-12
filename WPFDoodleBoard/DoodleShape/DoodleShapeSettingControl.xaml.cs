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
using static WPFDoodleBoard.Doodle.DoodleEnum;

namespace WPFDoodleBoard.DoodleShape
{
    /// <summary>
    /// DoodleShapeSettingControl.xaml 的交互逻辑
    /// </summary>
    public partial class DoodleShapeSettingControl : UserControl
    {
        private double _left, _top;
        private double _width, _height;
        private double _rotate;
        private Point _startPoint, _endPoint;
        private Point _moveDownPoint;
        private bool mouseDown;
        private ShapeSettingType settingType;
        private UIElement _element;

        private const double border = 10;
        private const double dragHeight = 20;
        private const double copyOffset = 20;

        public event ShapeSettingChange shapeSettingChange;
        public delegate void ShapeSettingChange(UIElement element, Point startPoint, Point endPoint);

        public event ShapeSettingRotate shapeSettingRotate;
        public delegate void ShapeSettingRotate(UIElement element, double rotate);

        public DoodleShapeSettingControl()
        {
            settingType = ShapeSettingType.ShapeSettingType_Move;

            InitializeComponent();
        }


        public void SetPoint(UIElement element, Point startPoint, Point endPoint, double rotate)
        {
            _element = element;
            _rotate = rotate;

            startPoint.X -= border;
            startPoint.Y -= border;
            endPoint.X += border;
            endPoint.Y += border;

            SetPoint(startPoint, endPoint);
            SetRotate(rotate);
        }

        #region UI
        public void SetPoint(Point startPoint, Point endPoint, bool needSend = false)
        {
            _left = GetLeft(startPoint, endPoint);
            _top = GetTop(startPoint, endPoint);
            _width = GetWidth(startPoint, endPoint);
            _height = GetHeight(startPoint, endPoint);

            //if (_element is DoodleField || _element is DoodleSpell)
            //{
            //    double min = _width <= (_height - dragHeight) ? _width : _height;
            //    _width = min;
            //    _height = min + dragHeight;

            //    _startPoint = new Point(_left, _top);
            //    _endPoint = new Point(_left + _width, _top + _height);
            //}
            //else
            //{
                _startPoint = new Point(_left, _top);
                _endPoint = new Point(_left + _width, _top + _height);
            //}

            container.Margin = new Thickness(_left, _top, 0, 0);
            container.Width = _width;
            container.Height = _height;
            
            if (needSend)
            {
                Point start = new Point(_startPoint.X + border, _startPoint.Y + border);
                Point end = new Point(_endPoint.X - border, _endPoint.Y - border);
                shapeSettingChange?.Invoke(_element, start, end);
            }
        }

        public void SetRotate(double rotate, bool needSend = false)
        {
            var changeRotate = rotate - _rotate;

            _rotate = rotate;

            RotateTransform rotateTransform = container.RenderTransform as RotateTransform;
            rotateTransform.Angle = rotate;
            rotateTransform.CenterX = _width / 2;
            rotateTransform.CenterY = _height / 2;
            
            if (needSend)
            {
                shapeSettingRotate?.Invoke(_element, rotate);
            }
        }
        #endregion

        #region private method
        private double GetLeft(Point startPoint, Point endPoint)
        {
            if (startPoint.X > endPoint.X)
            {
                return endPoint.X;
            }
            else
            {
                return startPoint.X;
            }
        }

        private double GetTop(Point startPoint, Point endPoint)
        {
            if (startPoint.Y > endPoint.Y)
            {
                return endPoint.Y;
            }
            else
            {
                return startPoint.Y;
            }
        }

        private double GetWidth(Point startPoint, Point endPoint)
        {
            return Math.Abs(startPoint.X - endPoint.X);
        }

        private double GetHeight(Point startPoint, Point endPoint)
        {
            return Math.Abs(startPoint.Y - endPoint.Y);
        }

        private Point GetCenter()
        {
            double w = Math.Abs(_startPoint.X - _endPoint.X) / 2;
            double h = Math.Abs(_startPoint.Y - _endPoint.Y) / 2;

            double x, y;
            if (_startPoint.X < _endPoint.X)
            {
                x = _startPoint.X + w;
            }
            else
            {
                x = _endPoint.X + w;
            }

            if (_startPoint.Y < _endPoint.Y)
            {
                y = _startPoint.Y + h;
            }
            else
            {
                y = _endPoint.Y + h;
            }

            return new Point(x, y);
        }

        private void SetType(double w, double h)
        {
            switch (settingType)
            {
                case ShapeSettingType.ShapeSettingType_LeftTop:
                    {
                        if (w < 0)
                        {
                            settingType = ShapeSettingType.ShapeSettingType_RightTop;
                        }

                        if (h < 0)
                        {
                            settingType = ShapeSettingType.ShapeSettingType_LeftBottom;
                        }
                    }
                    break;
                case ShapeSettingType.ShapeSettingType_RightTop:
                    {
                        if (w < 0)
                        {
                            settingType = ShapeSettingType.ShapeSettingType_LeftTop;
                        }

                        if (h < 0)
                        {
                            settingType = ShapeSettingType.ShapeSettingType_RightBottom;
                        }
                    }
                    break;
                case ShapeSettingType.ShapeSettingType_LeftBottom:
                    {
                        if (w < 0)
                        {
                            settingType = ShapeSettingType.ShapeSettingType_RightBottom;
                        }

                        if (h < 0)
                        {
                            settingType = ShapeSettingType.ShapeSettingType_LeftTop;
                        }
                    }
                    break;
                case ShapeSettingType.ShapeSettingType_RightBottom:
                    {
                        if (w < 0)
                        {
                            settingType = ShapeSettingType.ShapeSettingType_LeftBottom;
                        }

                        if (h < 0)
                        {
                            settingType = ShapeSettingType.ShapeSettingType_RightTop;
                        }
                    }
                    break;
            }
        }

        #endregion

        #region public method
        public void SetVisibility(bool show)
        {
            if (!show)
            {
                this.SetPoint(null, new Point(0, 0), new Point(0, 0), 0);
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                this.Visibility = Visibility.Visible;
            }
        }
        #endregion

        #region 旋转点转换
        /// <summary>
        /// 获取旋转角度
        /// </summary>
        /// <param name="p">移动的点</param>
        /// <param name="centerPoint">原点（中心点）</param>
        /// <returns></returns>
        /// 
        private double GetRotate(Point p, Point centerPoint)
        {
            double rotate = 0;                          //旋转角度
            double a = Math.Abs(centerPoint.X - p.X);   //对边
            double b = Math.Abs(centerPoint.Y - p.Y);   //邻边
            rotate = Math.Atan2(a, b) * 180 / Math.PI;
            if (p.X > centerPoint.X)
            {
                //右
                if (p.Y > centerPoint.Y)
                {
                    //下
                    rotate = 180 - rotate;
                }
                else
                {
                    //上
                }
            }
            else
            {
                //左
                if (p.Y > centerPoint.Y)
                {
                    //下
                    rotate = 180 + rotate;
                }
                else
                {
                    //上
                    rotate = 360 - rotate;
                }
            }

            return rotate;
        }
        /// <summary>
        /// 返回点P围绕中点旋转弧度rad后的坐标
        /// </summary>
        /// <param name="P">待旋转点坐标</param>
        /// <param name="rad">旋转弧度</param>
        /// <param name="isClockwise">true:顺时针/false:逆时针</param>
        /// <returns>旋转后坐标</returns>
        private Point RotatePoint(Point P, double rad, bool isClockwise = true)
        {
            Point Center = GetCenter();
            //点Temp1
            Point Temp1 = new Point(P.X - Center.X, P.Y - Center.Y);
            //点Temp1到原点的长度
            double lenO2Temp1 = DistanceTo(Temp1);
            //∠T1OX弧度
            double angT1OX = RadPOX(Temp1.X, Temp1.Y);
            //∠T2OX弧度（T2为T1以O为圆心旋转弧度rad）
            double angT2OX = angT1OX - (isClockwise ? 1 : -1) * rad;
            //点Temp2
            Point Temp2 = new Point(
             lenO2Temp1 * Math.Cos(angT2OX),
             lenO2Temp1 * Math.Sin(angT2OX));
            //点Q
            return new Point(Temp2.X + Center.X, Temp2.Y + Center.Y);
        }

        //该点到指定点pTarget的距离
        public double DistanceTo(Point p)
        {
            return Math.Sqrt((0 - p.X) * (0 - p.X) + (0 - p.Y) * (0 - p.Y));
        }

        /// <summary>
        /// 计算点P(x,y)与X轴正方向的夹角
        /// </summary>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        /// <returns>夹角弧度</returns>
        private double RadPOX(double x, double y)
        {
            //P在(0,0)的情况
            if (x == 0 && y == 0) return 0;
            //P在四个坐标轴上的情况：x正、x负、y正、y负
            if (y == 0 && x > 0) return 0;
            if (y == 0 && x < 0) return Math.PI;
            if (x == 0 && y > 0) return Math.PI / 2;
            if (x == 0 && y < 0) return Math.PI / 2 * 3;
            //点在第一、二、三、四象限时的情况
            if (x > 0 && y > 0) return Math.Atan(y / x);
            if (x < 0 && y > 0) return Math.PI - Math.Atan(y / -x);
            if (x < 0 && y < 0) return Math.PI + Math.Atan(-y / -x);
            if (x > 0 && y < 0) return Math.PI * 2 - Math.Atan(-y / x);
            return 0;
        }
        #endregion

        #region 鼠标事件
        private void RightBottom_MouseDown(object sender, MouseButtonEventArgs e)
        {
            settingType = ShapeSettingType.ShapeSettingType_RightBottom;
            window.Background = Brushes.Transparent;

            mouseDown = true;
        }

        private void LeftTop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            settingType = ShapeSettingType.ShapeSettingType_LeftTop;
            window.Background = Brushes.Transparent;

            mouseDown = true;
        }

        private void RightTop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            settingType = ShapeSettingType.ShapeSettingType_RightTop;
            window.Background = Brushes.Transparent;

            mouseDown = true;
        }

        private void LeftBottom_MouseDown(object sender, MouseButtonEventArgs e)
        {
            settingType = ShapeSettingType.ShapeSettingType_LeftBottom;
            window.Background = Brushes.Transparent;

            mouseDown = true;
        }

        private void Rotate_MouseDown(object sender, MouseButtonEventArgs e)
        {
            settingType = ShapeSettingType.ShapeSettingType_Rotate;
            window.Background = Brushes.Transparent;

            mouseDown = true;
        }

        private void container_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (settingType != ShapeSettingType.ShapeSettingType_Move) return;
            window.Background = Brushes.Transparent;

            _moveDownPoint = e.GetPosition(window);

            mouseDown = true;
        }

        private void container_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown) return;

            Point p = e.GetPosition(window);

            switch (settingType)
            {
                case ShapeSettingType.ShapeSettingType_Move:
                    {
                        double v = p.X - _moveDownPoint.X;
                        double h = p.Y - _moveDownPoint.Y;
                        _moveDownPoint = p;
                        Point startPoint = new Point(_startPoint.X + v, _startPoint.Y + h);
                        Point endPoint = new Point(_endPoint.X + v, _endPoint.Y + h);
                        SetPoint(startPoint, endPoint, true);
                    }
                    break;
                case ShapeSettingType.ShapeSettingType_LeftTop:
                    {
                        p = RotatePoint(p, _rotate * Math.PI / 180);

                        Point sPoint = _startPoint;
                        Point ePoint = _endPoint;

                        double w = ePoint.X - p.X;
                        double h = ePoint.Y - p.Y;
                        //if (w < 10) w = 10;
                        //if (h < 10) h = 10;
                        if (w < 0 || h < 0)
                        {
                            SetType(w, h);
                            return;
                        }

                        Point startPoint = new Point(_endPoint.X - w, _endPoint.Y - h);
                        SetPoint(startPoint, _endPoint, true);
                    }
                    break;
                case ShapeSettingType.ShapeSettingType_RightTop:
                    {
                        p = RotatePoint(p, _rotate * Math.PI / 180);

                        Point sPoint = _startPoint;
                        Point ePoint = _endPoint;

                        double w = p.X - sPoint.X;
                        double h = ePoint.Y - p.Y;
                        //if (w < 10) w = 10;
                        //if (h < 10) h = 10;
                        if (w < 0 || h < 0)
                        {
                            SetType(w, h);
                            return;
                        }

                        Point startPoint = new Point(_startPoint.X, _endPoint.Y - h);
                        Point endPoint = new Point(_startPoint.X + w, _endPoint.Y);
                        SetPoint(startPoint, endPoint, true);
                    }
                    break;
                case ShapeSettingType.ShapeSettingType_LeftBottom:
                    {
                        p = RotatePoint(p, _rotate * Math.PI / 180);

                        Point sPoint = _startPoint;
                        Point ePoint = _endPoint;

                        double w = ePoint.X - p.X;
                        double h = p.Y - sPoint.Y;
                        //if (w < 10) w = 10;
                        //if (h < 10) h = 10;
                        if (w < 0 || h < 0)
                        {
                            SetType(w, h);
                            return;
                        }

                        Point startPoint = new Point(_endPoint.X - w, _startPoint.Y);
                        Point endPoint = new Point(_endPoint.X, _startPoint.Y + h);
                        SetPoint(startPoint, endPoint, true);
                    }
                    break;
                case ShapeSettingType.ShapeSettingType_RightBottom:
                    {
                        p = RotatePoint(p, _rotate * Math.PI / 180);

                        Point sPoint = _startPoint;
                        Point ePoint = _endPoint;

                        double w = p.X - sPoint.X;
                        double h = p.Y - sPoint.Y;
                        //if (w < 10) w = 10;
                        //if (h < 10) h = 10;
                        if (w < 0 || h < 0)
                        {
                            SetType(w, h);
                            return;
                        }

                        Point endPoint = new Point(_startPoint.X + w, _startPoint.Y + h);
                        SetPoint(_startPoint, endPoint, true);
                    }
                    break;
                case ShapeSettingType.ShapeSettingType_Rotate:
                    {
                        Point centerPoint = GetCenter();
                        double rotate = GetRotate(p, centerPoint);
                        SetRotate(rotate, true);
                    }
                    break;
            }
        }

        private void container_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
            window.Background = null;

            settingType = ShapeSettingType.ShapeSettingType_Move;
        }
        #endregion
    }
}
