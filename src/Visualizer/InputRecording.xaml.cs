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
using System.Windows.Shapes;
using Visualizer.Global;

namespace Visualizer
{
    /// <summary>
    /// Interaction logic for InputRecording.xaml
    /// </summary>
    public partial class InputRecording : Window
    {
        public InputRecording()
        {
            InitializeComponent();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            // TODO:
            //Medium.Control.Pause();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            Medium.Control.Stop();

            App.Current.MainWindow = new ActionsEdit();
            this.Close();
            App.Current.MainWindow.Show();
        }
    }
}
