using Dapper;
using System.Data.SqlClient;
using System.Windows;

namespace QueryWatch.AlertsPages
{
    /// <summary>
    /// Interaction logic for AverageMOProcessingTime.xaml
    /// </summary>
    public partial class AverageMOProcessingTime : Window
    {
        public AverageMOProcessingTime()
        {
            InitializeComponent();
        }

        private async void Execute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var connectionFactory = ServiceLocator.GetService<DbConnectionFactory>();
                using var connection = connectionFactory.CreateConnection();

                if (!int.TryParse(Source_ConTextBox.Text, out int sourceCon))
                {
                    MessageBox.Show("Invalid Source Con");
                    return;
                }

                var sql = @"SELECT TOP 100 *,
                           DATEDIFF(MINUTE, Date_created, Date_updated) AS Elapsed_Minutes
                    FROM dbo.MO_Messages WITH (NOLOCK)
                    WHERE Source_Con = @sourceCon
                      AND CONVERT(DATE, Date_created) = CONVERT(DATE, GETDATE())
                    ORDER BY Date_created DESC;";

                var results = await connection.QueryAsync(sql, new { sourceCon });

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
