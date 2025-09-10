using System.Windows;
using System.Windows.Navigation;

namespace ProgPart17312
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Launch HomePage in a NavigationWindow
            var navWindow = new NavigationWindow();
            navWindow.Navigate(new HomePage()); // Make sure HomePage.xaml exists
            navWindow.Show();
        }
    }
}
