using System.Windows;
using Jellyfish;

namespace GameFinder
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void OnAppExit(object sender, ExitEventArgs e)
        {
            Session.Config.Save();
        }

        private void OnAppStartup(object sender, StartupEventArgs e)
        {
            Session.Config = Preferences.LoadOrDefault<Config>(Preferences.RecommendedPath) ??
                             new Config(Preferences.RecommendedPath);
        }
    }
}