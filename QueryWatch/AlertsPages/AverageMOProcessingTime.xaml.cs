using Dapper;
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
            var connectionFactory = ServiceLocator.GetService<DbConnectionFactory>();
            using var connection = connectionFactory.CreateConnection();
            var sql = $@"SELECT TOP 100 *,
       DATEDIFF(MINUTE, Date_created, Date_updated) AS Elapsed_Minutes
FROM dbo.MO_Messages WITH (NOLOCK)
WHERE Source_Con = {int.Parse(Source_ConTextBox.Text)}
  AND CONVERT(DATE, Date_created) = CONVERT(DATE, GETDATE())
ORDER BY Date_created DESC;";
            var results = await connection.QueryAsync(sql);

            dataGrid1.ItemsSource = results;
        }
    }
}
