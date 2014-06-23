/******************************************************************************/
/*                                                                            */
/*   Program: MyHitTest                                                       */
/*   A smal sample for a WPF Hit-Test                                         */
/*                                                                            */
/*   21.5.2014 0.0.0.0 uhwgmxorg Start                                        */
/*   23.6.2014 0.0.0.0 uhwgmxorg Now moving dots with the right mouse button  */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MyHitTest
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        bool _hit = false;
        MyVisualHost _visualHost = new MyVisualHost();

        public double DotSize { get; set; }
        public string DotColor { get; set; }

        /// <summary>
        /// Properies
        /// </summary>
        private bool checkBoxAdd;
        public bool CheckBoxAdd { get { return checkBoxAdd; } set { checkBoxAdd = value; OnPropertyChanged("CheckBoxAdd"); } }
        private string message;
        public string Message { get { return message; } set { message = value; OnPropertyChanged("Message"); } }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        /******************************/
        /*           Events           */
        /******************************/
        #region Events

        /// <summary>
        /// Window_Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _visualHost = new MyVisualHost();
            _visualHost.Hit += HitHandler;
            MyCanvas.Children.Add(_visualHost);
        }

        /// <summary>
        /// Hit
        /// </summary>
        public void HitHandler()
        {
            Console.Beep();
            Message = "Hit !!";
            _hit = true;
        }

        /// <summary>
        /// Window_MouseLeftButtonDown
        /// add a dot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!CheckBoxAdd)
            {
                if(!_hit)
                    Message = "Hit fail";
                _hit = false;
                return;
            }
            var pos = Mouse.GetPosition(this);
            DotSize = 10;
            DotColor = "#FFFF0000";
            _visualHost.AddDot(pos,DotSize,(SolidColorBrush)(new BrushConverter().ConvertFrom(DotColor)));
            Message = "Add";
        }

        /// <summary>
        /// Window_MouseRightButtonDown
        /// clear all dots
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        /// <summary>
        /// MyCanvas_MouseMove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (CheckBoxAdd) return;
            var mea = e;
            if (mea.RightButton == MouseButtonState.Released) return;
            double x = e.GetPosition((IInputElement)((RoutedEventArgs)(e)).Source).X;
            double y = e.GetPosition((IInputElement)((RoutedEventArgs)(e)).Source).Y;

            _visualHost.MoveDot(new Point(x, y), DotSize, (SolidColorBrush)(new BrushConverter().ConvertFrom(DotColor)));
        }

        /// <summary>
        /// Button_Clear_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            _visualHost.Clear();
            Message = "Clear";
        }

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        /// <summary>
        /// OnPropertyChanged
        /// </summary>
        /// <param name="p"></param>
        private void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        #endregion
    }
}
