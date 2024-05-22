using Dapper;
using System.Data.SqlClient;
using System.Windows;

namespace QueryWatch.AlertsPages
{
    /// <summary>
    /// Interaction logic for NonStartedISTTasks.xaml
    /// </summary>
    public partial class NonStartedISTTasks : Window
    {
        public NonStartedISTTasks()
        {
            InitializeComponent();
        }

        private async void Execute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var connectionFactory = ServiceLocator.GetService<DbConnectionFactory>();
                using var connection = connectionFactory.CreateConnection();

                string task = TaskTextBox.Text;
                if (string.IsNullOrEmpty(task))
                {
                    MessageBox.Show("Task cannot be empty");
                    return;
                }

                var sql = @";WITH DateCTE AS (
                        SELECT
                            CONVERT(DATE, date_created) AS EventDate,
                            event,
                            extra
                        FROM dbo.IST_ActivityLog WITH (NOLOCK)
                        WHERE
                            task = @task
                            AND date_created >= '2022-05-21'
                    )
                    SELECT
                        COUNT(*) AS EventCount,
                        EventDate,
                        event,
                        extra
                    FROM DateCTE
                    GROUP BY EventDate, event, extra
                    ORDER BY EventDate DESC;";

                var results = await connection.QueryAsync(sql, new { task });

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
