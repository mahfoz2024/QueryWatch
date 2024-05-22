using Dapper;
using System.Data.SqlClient;
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
            try
            {
                var connectionFactory = ServiceLocator.GetService<DbConnectionFactory>();
                using var connection = connectionFactory.CreateConnection();

                if (!int.TryParse(OperatorIdTextBox.Text, out int operatorId))
                {
                    MessageBox.Show("Invalid Operator ID");
                    return;
                }

                int? sourceCon = null;
                if (!string.IsNullOrEmpty(SourceConTextBox.Text) && int.TryParse(SourceConTextBox.Text, out int parsedSourceCon))
                {
                    sourceCon = parsedSourceCon;
                }

                int? serviceId = null;
                if (!string.IsNullOrEmpty(ServiceIdTextBox.Text) && int.TryParse(ServiceIdTextBox.Text, out int parsedServiceId))
                {
                    serviceId = parsedServiceId;
                }

                var sql = @"
            SELECT CAST(date_created AS DATE) AS Date, 
                   (SUM(CASE WHEN Billing_Status > 0 THEN 1 ELSE 0 END) * 100.0) / COUNT(*) AS BillingPercentage,
                   SUM(CASE WHEN Billing_Status > 0 THEN 1 ELSE 0 END) AS BillingsDeliveredSuccessful,
                   SUM(CASE WHEN Billing_Status < 0 THEN 1 ELSE 0 END) AS BillingsFailedOrRejected,
                   SUM(CASE WHEN Billing_Status = 0 THEN 1 ELSE 0 END) AS BillingsForwardedOrPending,
                   SUM(CASE WHEN Processed > -1 THEN 1 ELSE 0 END) AS ProcessedProcessed,
                   SUM(CASE WHEN Processed = -1 THEN 1 ELSE 0 END) AS ProcessedNotProcessed,
                   SUM(CASE WHEN Processed = -2 THEN 1 ELSE 0 END) AS ProcessedBlocked,
                   SUM(CASE WHEN Processed = -4 THEN 1 ELSE 0 END) AS ProcessedSkippedOutofSchedule, 
                   SUM(CASE WHEN Processed = -5 THEN 1 ELSE 0 END) AS ProcessedMTSkippedAsUserAlreadyCharged,
                   SUM(CASE WHEN Processed = -3 THEN 1 ELSE 0 END) AS ProcessedSkippedAsUnsubscribed,
                   SUM(CASE WHEN Delivery_Status < 0 THEN 1 ELSE 0 END) AS DeliveryStatsFailedFreeMT,
                   COUNT(*) AS Total
            FROM MT_Messages WITH (NOLOCK)
            WHERE date_created >= DATEADD(day, -30, GETDATE()) 
              AND OC = @operatorId
              AND (@sourceCon IS NULL OR Source_Con = @sourceCon)
              AND (@serviceId IS NULL OR SrvcId = @serviceId)
            GROUP BY CAST(date_created AS DATE)
            ORDER BY Date DESC
            OFFSET 0 ROWS FETCH NEXT 100 ROWS ONLY";

                var results = await connection.QueryAsync(sql, new { operatorId, sourceCon, serviceId });

                dataGrid1.ItemsSource = results;
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"A database error occurred: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}");
            }
        }

    }
}
