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

namespace WPFDoodleBoard
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            UpdateUI();
        }

        private void UpdateUI()
        {
            if (doodle.canDraw)
            {
                buttonModeDraw.IsEnabled = false;
                buttonModeSelect.IsEnabled = true;
            }
            else
            {
                buttonModeDraw.IsEnabled = true;
                buttonModeSelect.IsEnabled = false;
            }
        }

        private void buttonDraw_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetDrawType(Doodle.DoodleEnum.DoodleEnumType.draw);
        }

        private void buttonLine_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetDrawType(Doodle.DoodleEnum.DoodleEnumType.line);
        }

        private void buttonRect_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetDrawType(Doodle.DoodleEnum.DoodleEnumType.rect);
        }

        private void buttonCircle_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetDrawType(Doodle.DoodleEnum.DoodleEnumType.circle);
        }

        private void buttonArrow_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetDrawType(Doodle.DoodleEnum.DoodleEnumType.arrow);
        }

        private void buttonCube_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetDrawType(Doodle.DoodleEnum.DoodleEnumType.cube);
        }

        private void buttonCylinder_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetDrawType(Doodle.DoodleEnum.DoodleEnumType.cylinder);
        }

        private void buttonCone_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetDrawType(Doodle.DoodleEnum.DoodleEnumType.cone);
        }




        private void buttonBlack_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetDrawColor(Doodle.DoodleEnum.DoodleEnumColor.black);
        }

        private void buttonBlue_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetDrawColor(Doodle.DoodleEnum.DoodleEnumColor.blue);
        }

        private void buttonRed_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetDrawColor(Doodle.DoodleEnum.DoodleEnumColor.red);
        }

        private void buttonGreen_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetDrawColor(Doodle.DoodleEnum.DoodleEnumColor.green);
        }

        private void buttonOrange_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetDrawColor(Doodle.DoodleEnum.DoodleEnumColor.orange);
        }

        private void buttonBrush1_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetDrawWidth(Doodle.DoodleEnum.DoodleEnumBrushType.small);
        }

        private void buttonBrush2_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetDrawWidth(Doodle.DoodleEnum.DoodleEnumBrushType.middle);
        }

        private void buttonBrush3_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetDrawWidth(Doodle.DoodleEnum.DoodleEnumBrushType.big);
        }

        private void buttonBrush4_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetDrawWidth(Doodle.DoodleEnum.DoodleEnumBrushType.bigger);
        }



        private void buttonModeDraw_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetCanDraw();
            UpdateUI();
        }

        private void buttonModeSelect_Click(object sender, RoutedEventArgs e)
        {
            doodle.SetCanSelect();
            UpdateUI();
        }
    }
}
