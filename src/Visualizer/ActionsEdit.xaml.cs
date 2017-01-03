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
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Common;
using Visualizer.Global;
using Visualizer.ViewModels;

namespace Visualizer
{
    /// <summary>
    /// Interaction logic for ActionsEdit.xaml
    /// </summary>
    public partial class ActionsEdit : Window
    {
        public ActionsEdit()
        {
            InitializeComponent();

            var model = new ActionsEditViewModel(Medium.Control.Tape);
            this.DataContext = model;
        }

        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow = new InputRecording();
            this.Close();
            App.Current.MainWindow.Show();

            Medium.Control.Record();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow = new InputPlay();
            this.Close();
            App.Current.MainWindow.Show();

            Medium.Control.Play();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Medium.Control.Clear();

            var model = this.DataContext as ActionsEditViewModel;
            model.Clear();
        }
    }
}
