using Dapper;
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
            using var connection = ServiceLocator.GetConnection();

            var sql = @"SELECT CAST(Date_created AS DATE) AS Date, 
                       COUNT(*) AS TotalAttempts,
                       SUM(CASE WHEN Billing_Status > 0 THEN 1 ELSE 0 END) AS SuccessfulAttempts
                FROM MT_Messages With(Nolock)
                WHERE OC = @operatorId AND (@serviceId = '' OR SrvcId = @serviceId)
                GROUP BY CAST(Date_created AS DATE)
                ORDER BY Date DESC
                OFFSET 0 ROWS FETCH NEXT 100 ROWS ONLY";

            var results = await connection.QueryAsync(sql,
                                                   new
                                                   {
                                                       operatorId = int.Parse(OperatorIdTextBox.Text),
                                                       serviceId = ServiceIdTextBox.Text
                                                   });

            dataGrid1.ItemsSource = results;
        }
    }
}
