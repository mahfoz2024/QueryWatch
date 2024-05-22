using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QueryWatch.AlertsPages
{
    /// <summary>
    /// Interaction logic for MsisdnIdentificationDrop.xaml
    /// </summary>
    public partial class MsisdnIdentificationDrop : Window
    {
        public MsisdnIdentificationDrop()
        {
            InitializeComponent();
        }

        private async void Execute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var connectionFactory = ServiceLocator.GetService<DbConnectionFactory>();
                using var connection = connectionFactory.CreateConnection();

                int operatorId;
                if (!int.TryParse(OperatorIdTextBox.Text, out operatorId))
                {
                    MessageBox.Show("Invalid Operator ID");
                    return;
                }

                var sql1 = @"select DateKey, Count(*)
                     from vwVisits with (nolock)
                     where operatorId = @OperatorId
                     and Datekey > 20221001
                     Group by DateKey
                     order by DateKey desc;";

                var sql2 = @"select DATEADD(hour, DATEDIFF(hour, 0, DateCreated), 0) as HourKey, Count(*)
                     from vwVisits with (nolock)
                     where operatorId = @OperatorId
                     and Datekey > 20221001
                     group by DATEADD(hour, DATEDIFF(hour, 0, DateCreated), 0)
                     order by HourKey desc;";

                var results1 = await connection.QueryAsync(sql1, new { OperatorId = operatorId });
                var results2 = await connection.QueryAsync(sql2, new { OperatorId = operatorId });

                dataGrid1.ItemsSource = results1;
                dataGrid2.ItemsSource = results2;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
