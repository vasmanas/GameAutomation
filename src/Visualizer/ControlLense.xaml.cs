using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Visualizer
{
    /// <summary>
    /// Interaction logic for ControlLense.xaml
    /// </summary>
    public partial class ControlLense : Window
    {
        public ControlLense()
        {
            this.InitializeComponent();
        }
        
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var hwnd = new WindowInteropHelper(this).Handle;
            WindowsServices.SetWindowExTransparent(hwnd);
        }
    }
}
