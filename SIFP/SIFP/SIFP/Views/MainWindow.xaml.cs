using System.Windows;

namespace SIFP.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            DrawerHost.OpenMode = MaterialDesignThemes.Wpf.DrawerHostOpenMode.Standard;
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            DrawerHost.OpenMode = MaterialDesignThemes.Wpf.DrawerHostOpenMode.Model;
        }
    }
}
