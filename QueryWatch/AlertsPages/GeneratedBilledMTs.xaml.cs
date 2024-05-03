using Dapper;
using System.Windows;

namespace QueryWatch.AlertsPages
{
    /// <summary>
    /// Interaction logic for GeneratedBilledMTs.xaml
    /// </summary>
    public partial class GeneratedBilledMTs : Window
    {

        public GeneratedBilledMTs()
        {
            InitializeComponent();
        }

        private async void Execute_Click(object sender, RoutedEventArgs e)
        {
            var connectionFactory = ServiceLocator.GetService<DbConnectionFactory>();
            using var connection = connectionFactory.CreateConnection();

            var operatorId = int.Parse(OperatorIdTextBox.Text);
            var sourceConText = SourceConTextBox.Text;
            var serviceIdText = ServiceIdTextBox.Text;

            var whereClause = $"WHERE date_created >= DATEADD(day, -30, GETDATE()) AND OC = {operatorId}";
            if (!string.IsNullOrEmpty(sourceConText))
            {
                whereClause += $" AND Source_Con = {int.Parse(sourceConText)}";
            }
            if (!string.IsNullOrEmpty(serviceIdText))
            {
                whereClause += $" AND SrvcId = {int.Parse(serviceIdText)}";
            }

            var sql = $@"SELECT CAST(date_created AS DATE) AS Date, 
       (SUM(CASE WHEN Billing_Status > 0 THEN 1 ELSE 0 END) * 100.0) / COUNT(*) AS BillingPercentage,
       SUM(CASE WHEN Billing_Status > 0 THEN 1 ELSE 0 END) AS BillingsDeliveredSuccessful,
       SUM(CASE WHEN Billing_Status < 0 THEN 1 ELSE 0 END) AS BillingsFailedOrRejected,
       SUM(CASE WHEN Billing_Status = 0 THEN 1 ELSE 0 END) AS BillingsForwardedOrPending,
       SUM(CASE WHEN Processed > -1 THEN 1 ELSE 0 END) AS ProcessedProcessed,
       SUM(CASE WHEN Processed = -1 THEN 1 ELSE 0 END) AS ProcessedNotProcessed,
       SUM(CASE WHEN Processed = -2 THEN 1 ELSE 0 END) AS ProcessedBlocked,
       SUM(CASE WHEN Processed = -4 THEN 1 ELSE 0 END) AS ProcessedSKippedOutofSchedule, 
       SUM(CASE WHEN Processed = -5 THEN 1 ELSE 0 END) AS ProcessedMTSkippedAsUserAlreadyCharged,
       SUM(CASE WHEN Processed = -3 THEN 1 ELSE 0 END) AS ProcessedSkippedAsUnsubscribed,
       SUM(CASE WHEN Delivery_Status < 0 THEN 1 ELSE 0 END) AS DeliveryStatsFailedFreeMT,
       COUNT(*) AS Total
FROM MT_Messages With(Nolock)
{whereClause}
GROUP BY CAST(date_created AS DATE)
ORDER BY Date DESC";

            var results = await connection.QueryAsync(sql);

            dataGrid1.ItemsSource = results;
        }
    }
}
