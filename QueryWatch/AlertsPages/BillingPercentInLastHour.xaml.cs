using Dapper;
using System.Data.SqlClient;
using System.Windows;

namespace QueryWatch.AlertsPages
{
    /// <summary>
    /// Interaction logic for BillingPercentInLastHour.xaml
    /// </summary>
    public partial class BillingPercentInLastHour : Window
    {
        public BillingPercentInLastHour()
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

                var sql1 = @";WITH DateCTE AS (
                        SELECT
                            CONVERT(DATE, t.Date_created) AS DateCreated,
                            t.OC,
                            t.SrvcId,
                            t.Billing_Status,
                            t.Date_created
                        FROM dbo.MT_Messages AS t WITH(NOLOCK)
                        WHERE t.OC = @operatorId
                        AND t.SrvcId % 10 = 1 
                        AND t.Billing_Status > 0
                    )
                    SELECT
                        DateCTE.DateCreated,
                        COUNT(*) AS [Count]
                    FROM DateCTE
                    WHERE DATEPART(HOUR, DateCTE.Date_created) BETWEEN 0 AND 10
                    GROUP BY DateCTE.DateCreated
                    ORDER BY DateCTE.DateCreated DESC;";

                var sql2 = @";WITH DateCTE AS (
                        SELECT
                            CONVERT(DATE, m.date_created) AS DateCreated,
                            m.date_created,
                            m.updated_date,
                            r.Proc_MSec,
                            m.Billing_Status
                        FROM dbo.MT_Messages m WITH (NOLOCK)
                        LEFT JOIN dbo.MT_messages_Response r ON r.Req_id = m.Req_id
                        WHERE m.oc = @operatorId
                        AND m.srvcID % 10 = 1 
                        AND m.date_created >= DATEADD(DAY, -10, CONVERT(DATE, GETDATE()))
                        AND m.Billing_Status != 0
                    )
                    SELECT
                        DateCTE.DateCreated,
                        COUNT(*) AS [Count],
                        AVG(DateDiff(MINUTE, DateCTE.date_created, DateCTE.updated_date)) AS Diff,
                        AVG(CASE WHEN DateCTE.Proc_MSec IS NULL THEN 0 ELSE CAST(DateCTE.Proc_MSec AS BIGINT) END) AS AvgProcMSec
                    FROM DateCTE
                    WHERE DATEPART(HOUR, DateCTE.date_created) BETWEEN 0 AND 10
                    GROUP BY DateCTE.DateCreated
                    ORDER BY DateCTE.DateCreated DESC;";

                var results1 = await connection.QueryAsync(sql1, new { operatorId });
                var results2 = await connection.QueryAsync(sql2, new { operatorId });

                dataGrid1.ItemsSource = results1;
                dataGrid2.ItemsSource = results2;
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
