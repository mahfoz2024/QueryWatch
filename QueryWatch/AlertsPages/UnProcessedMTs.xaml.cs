using Dapper;
using System.Windows;

namespace QueryWatch.AlertsPages
{
    /// <summary>
    /// Interaction logic for UnProcessedMTs.xaml
    /// </summary>
    public partial class UnProcessedMTs : Window
    {

        public UnProcessedMTs()
        {
            InitializeComponent();
        }

        private async void Execute_Click(object sender, RoutedEventArgs e)
        {
            var connectionFactory = ServiceLocator.GetService<DbConnectionFactory>();
            using var connection = connectionFactory.CreateConnection();

            var sql = @"SELECT CAST(date_created AS DATE) AS Date, 
                   (SUM(CASE WHEN Billing_Status > 0 THEN 1 ELSE 0 END) * 100.0) / COUNT(*) AS BillingSuccessPercentage,
                   SUM(CASE WHEN Billing_Status > 0 THEN 1 ELSE 0 END) AS SuccessfulBillings,
                   SUM(CASE WHEN Billing_Status < 0 THEN 1 ELSE 0 END) AS FailedOrRejectedBillings,
                   SUM(CASE WHEN Billing_Status = 0 THEN 1 ELSE 0 END) AS ForwardedOrPendingBillings,
                   SUM(CASE WHEN Processed > -1 THEN 1 ELSE 0 END) AS ProcessedMessages,
                   SUM(CASE WHEN Processed = -1 THEN 1 ELSE 0 END) AS NotProcessedMessages,
                   SUM(CASE WHEN Processed = -2 THEN 1 ELSE 0 END) AS BlockedMessages,
                   SUM(CASE WHEN Processed = -4 THEN 1 ELSE 0 END) AS SkippedOutOfScheduleMessages,
                   SUM(CASE WHEN Processed = -5 THEN 1 ELSE 0 END) AS SkippedUserAlreadyChargedMessages,
                   SUM(CASE WHEN Processed = -3 THEN 1 ELSE 0 END) AS SkippedUnsubscribedMessages,
                   SUM(CASE WHEN Delivery_Status < 0 THEN 1 ELSE 0 END) AS FailedFreeMTDeliveries,
                   COUNT(*) AS TotalMessages
            FROM MT_Messages WITH(NOLOCK)
            WHERE date_created >= DATEADD(day, -30, GETDATE())
              AND DATEPART(hour, date_created) <= 19
              AND SOA = @soa
              AND (@serviceId = '' OR SrvcId = @serviceId)
              AND OC = @operatorId
            GROUP BY CAST(date_created AS DATE)
            ORDER BY Date DESC";

            var results = await connection.QueryAsync(sql,
                                                   new
                                                   {
                                                       soa = SoaTextBox.Text,
                                                       serviceId = ServiceIdTextBox.Text,
                                                       operatorId = int.Parse(OperatorIdTextBox.Text)
                                                   });

            dataGrid1.ItemsSource = results;
        }
    }
}
