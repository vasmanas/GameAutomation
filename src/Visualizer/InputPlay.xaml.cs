using System.Windows;
using Visualizer.Global;

namespace Visualizer
{
    /// <summary>
    /// Interaction logic for InputPlay.xaml
    /// </summary>
    public partial class InputPlay : Window
    {
        public InputPlay()
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
