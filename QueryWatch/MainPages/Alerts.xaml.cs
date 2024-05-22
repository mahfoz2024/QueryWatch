using QueryWatch.AlertsPages;
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

namespace QueryWatch.MainPages
{
    /// <summary>
    /// Interaction logic for Alerts.xaml
    /// </summary>
    public partial class Alerts : Window
    {
        public Alerts()
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

        private void MsisdnIdentificationDrop_Click(object sender, RoutedEventArgs e)
        {
            var window=new MsisdnIdentificationDrop();
            window.Show();
        }
    }
}
