using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LinearRegression
{
    class MyPoint : DependencyObject
    {
        public static readonly DependencyProperty XProperty;
        public static readonly DependencyProperty YProperty;

        public MyPoint () { }  /* default constructor which allows to make points via DataGrid */

        static MyPoint()
        {
            XProperty = DependencyProperty.Register("X", typeof(double), typeof(MyPoint));
            YProperty = DependencyProperty.Register("Y", typeof(double), typeof(MyPoint));
        }

        public MyPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set
            {
                SetValue(YProperty, value);
            }
        }

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set
            {
                SetValue(XProperty, value);
            }
        }
    }
}
