using QueryWatch.AlertsPages;
using QueryWatch.MainPages;
using System.Windows;

namespace QueryWatch
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

       

        private void Operators_Click(object sender, RoutedEventArgs e)
        {
            var window = new Operators();
            window.Show();
        }

        private void Alerts_Click(object sender, RoutedEventArgs e)
        {
            var window=new Alerts();
            window.Show();
        }

        private void Services_Click(object sender, RoutedEventArgs e)
        {
            var windows = new Services();
            windows.Show();
        }
    }
}