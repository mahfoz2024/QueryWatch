using Dapper;
using System.Data.SqlClient;
using System.Windows;

namespace QueryWatch.AlertsPages
{
    /// <summary>
    /// Interaction logic for CumulativeBillingPercentAllDay.xaml
    /// </summary>
    public partial class CumulativeBillingPercentAllDay : Window
    {
        public CumulativeBillingPercentAllDay()
        {
            InitializeComponent();
        }

        private async void Execute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var connection = ServiceLocator.GetConnection();

                if (!int.TryParse(OperatorIdTextBox.Text, out int operatorId))
                {
                    MessageBox.Show("Invalid Operator ID");
                    return;
                }

                if (!int.TryParse(ServiceIdTextBox.Text, out int serviceId))
                {
                    MessageBox.Show("Invalid Service ID");
                    return;
                }

                var sql = @"SELECT CAST(Date_created AS DATE) AS Date, 
                           COUNT(*) AS TotalAttempts,
                           SUM(CASE WHEN Billing_Status > 0 THEN 1 ELSE 0 END) AS SuccessfulAttempts
                    FROM MT_Messages WITH(NOLOCK)
                    WHERE OC = @operatorId AND (@serviceId = 0 OR SrvcId = @serviceId)
                    GROUP BY CAST(Date_created AS DATE)
                    ORDER BY Date DESC
                    OFFSET 0 ROWS FETCH NEXT 100 ROWS ONLY";

                var results = await connection.QueryAsync(sql, new { operatorId, serviceId });

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
