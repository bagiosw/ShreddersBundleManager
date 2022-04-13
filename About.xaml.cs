using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace ShreddersBundleManager
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public string appVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
        public About()
        {
            InitializeComponent();
            Version.Text = "Version " + appVersion;
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CustomDraggable_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void GithubButton_Click(object sender, RoutedEventArgs e)
        {
            var uri = "https://github.com/bagiosw/ShreddersBundleManager";
            var psi = new ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            Process.Start(psi);
        }
    }
}
