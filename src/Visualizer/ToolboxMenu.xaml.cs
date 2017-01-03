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
using InputDevicesSimulator;
using Visualizer.Global;

namespace Visualizer
{
    /// <summary>
    /// Interaction logic for ToolboxMenu.xaml
    /// </summary>
    public partial class ToolboxMenu : Window
    {
        public ToolboxMenu()
        {
            InitializeComponent();
        }

        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            this.IfPlayingStop();

            if (!this.IfRecordingStop())
            {
                this.btnRecord.Content = FindResource("Recording");
            }

            Medium.Control.Record();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            this.IfRecordingStop();
            this.IfPlayingStop();

            Medium.Control.Stop();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            this.IfRecordingStop();

            if (!this.IfPlayingStop())
            {
                this.btnPlay.Content = FindResource("Playing");
            }

            Medium.Control.Play();
        }

        private bool IfPlayingStop()
        {
            if (this.btnPlay.Content == FindResource("Playing"))
            {
                this.btnPlay.Content = FindResource("Play");

                return true;
            }

            return false;
        }

        private bool IfRecordingStop()
        {
            if (this.btnRecord.Content == FindResource("Recording"))
            {
                this.btnRecord.Content = FindResource("Record");

                return true;
            }

            return false;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow = new ActionsEdit(); 
            this.Close();
            App.Current.MainWindow.Show();
        }
    }
}
