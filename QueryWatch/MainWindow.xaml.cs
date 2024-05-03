using QueryWatch.AlertsPages;
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

        private void NoSubscribers_Click(object sender, RoutedEventArgs e)
        {
            var NoSubscribers = new NoSubscribersForLongTime();
            NoSubscribers.Show();

        }

        private void CumulativeBillingPercentAllDay_Click(object sender, RoutedEventArgs e)
        {
            var window = new CumulativeBillingPercentAllDay();
            window.Show();
        }

        private void UnProcessedMTs_Click(object sender, RoutedEventArgs e)
        {
            var window = new UnProcessedMTs();
            window.Show();
        }

        private void GeneratedBilledMTs_Click(object sender, RoutedEventArgs e)
        {
            var window = new GeneratedBilledMTs();
            window.Show();
        }

        private void NonStartedISTTasks_Click(object sender, RoutedEventArgs e)
        {
            var window = new NonStartedISTTasks();
            window.Show();
        }

        private void AverageMOProcessingTime_Click(object sender, RoutedEventArgs e)
        {
            var window = new AverageMOProcessingTime();
            window.Show();
        }

        private void ChangeInBillingStatus_Click(object sender, RoutedEventArgs e)
        {
            var window = new ChangeInBillingStatus();
            window.Show();
        }

        private void BillingPercentInLastHour_Click(object sender, RoutedEventArgs e)
        {
            var window = new BillingPercentInLastHour();
            window.Show();
        }


    }
}