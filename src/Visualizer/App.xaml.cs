using System.Windows;

namespace Visualizer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            Current.MainWindow = new ActionsEdit();
            Current.MainWindow.Show();
        }
    }
}
