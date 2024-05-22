using Dapper;
using System.Data.SqlClient;
using System.Windows;

namespace QueryWatch.AlertsPages
{
    /// <summary>
    /// Interaction logic for ChangeInBillingStatus.xaml
    /// </summary>
    public partial class ChangeInBillingStatus : Window
    {
        public ChangeInBillingStatus()
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

                var sql = @";WITH DateCTE AS (
                        SELECT
                            CAST(m.date_created AS DATE) AS Day_Date,
                            CASE WHEN m.Processed = 1 THEN 'Processed' ELSE 'Unprocessed' END AS Processing_Status
                        FROM dbo.MT_Messages m WITH(NOLOCK)
                        WHERE m.oc = @operatorId
                        AND m.srvcID % 10 = 1 
                    )
                    SELECT TOP 300
                        Day_Date,
                        [Processed],
                        [Unprocessed]
                    FROM DateCTE
                    PIVOT (
                        COUNT(Processing_Status)
                        FOR Processing_Status IN ([Processed], [Unprocessed])
                    ) AS PivotTable
                    ORDER BY Day_Date DESC;";

                var results = await connection.QueryAsync(sql, new { operatorId });

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
