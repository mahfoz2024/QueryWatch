using Dapper;
using System.Windows;

namespace QueryWatch.AlertsPages
{
    /// <summary>
    /// Interaction logic for NoSubscribersForLongTime.xaml
    /// </summary>
    public partial class NoSubscribersForLongTime : Window
    {

        public NoSubscribersForLongTime()
        {
            InitializeComponent();
        }

        private async void SubscriptionMethodComboBox_Initialized(object sender, EventArgs e)
        {
            var connectionFactory = ServiceLocator.GetService<DbConnectionFactory>();
            using var connection = connectionFactory.CreateConnection();

            var sql = "SELECT DISTINCT Method FROM Subscription_Methods WITH(NOLOCK)";
            var methods = await connection.QueryAsync<string>(sql);
            SubscriptionMethodComboBox.ItemsSource = methods;
        }

        private async void Execute_Click(object sender, RoutedEventArgs e)
        {
            var connectionFactory = ServiceLocator.GetService<DbConnectionFactory>();
            using var connection = connectionFactory.CreateConnection();

            int operatorId = int.Parse(OperatorIdTextBox.Text);
            int serviceId = int.Parse(ServiceIdTextBox.Text);
            string subMethod = SubscriptionMethodComboBox.SelectedItem?.ToString() ?? "";
            var startDate = DateTime.Today.AddDays(-15);
            var fifteenDaysAgo = DateTime.Today.AddDays(-15);

            var sqlSubscribers = @"SELECT CAST(SubscribedOn AS DATE) AS DateOfSubscription, 
                                 COUNT(*) AS TotalSubscribers
                          FROM Subscribers WITH(NOLOCK)
                          WHERE SubscribedOn >= @startDate AND OC = @operatorId AND Service = @serviceId 
                                AND (@subMethod = '' OR SubMethod_Id IN (SELECT PK FROM Subscription_Methods WHERE Method = @subMethod))
                          GROUP BY CAST(SubscribedOn AS DATE)
                          ORDER BY DateOfSubscription DESC";

            var sqlVisitors = @"SELECT CAST(DateCreated AS DATE) AS DateOfVisit, 
                             COUNT(*) AS TotalVisitors 
                       FROM vwVisits
                       WHERE DateCreated >= @fifteenDaysAgo AND OperatorId = @operatorId AND ServiceId = @serviceId
                             AND (@subMethod = '' OR SubscriptionMethodId IN (SELECT PK FROM Subscription_Methods WHERE Method = @subMethod))
                       GROUP BY CAST(DateCreated AS DATE)
                       ORDER BY DateOfVisit DESC";

            var totalSubscribers = await connection.QueryAsync(sqlSubscribers, new { startDate, operatorId, serviceId, subMethod });
            var totalVisitors = await connection.QueryAsync(sqlVisitors, new { fifteenDaysAgo, operatorId, serviceId, subMethod });

            dataGrid1.ItemsSource = totalSubscribers;
            dataGrid2.ItemsSource = totalVisitors;
        }

    }
}
