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

namespace UXStudy
{
    /// <summary>
    /// Interaction logic for TabbedSubMenuControl.xaml
    /// </summary>
    public partial class TabbedSubMenuControl : UserControl
    {
        public TabbedSubMenuControl()
        {
            InitializeComponent();
        }

        private void scroll_Loaded(object sender, RoutedEventArgs e)
        {
            scroll.AddHandler(MouseWheelEvent, new RoutedEventHandler(scrollControls), true);
        }

        private void scrollControls(object sender, RoutedEventArgs e)
        {

            MouseWheelEventArgs eargs = (MouseWheelEventArgs)e;

            double x = eargs.Delta;

            double y = scroll.VerticalOffset;

            scroll.ScrollToVerticalOffset(y - x);
        }
    }
}
