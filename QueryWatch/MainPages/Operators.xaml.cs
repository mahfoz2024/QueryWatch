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

namespace QueryWatch.MainPages
{
    /// <summary>
    /// Interaction logic for Operators.xaml
    /// </summary>
    public partial class Operators : Window
    {
        public Operators()
        {
            InitializeComponent();
        }

        private async void Execute_Click(object sender, RoutedEventArgs e)
        {
            var connectionFactory = ServiceLocator.GetService<DbConnectionFactory>();
            using var connection = connectionFactory.CreateConnection();
            string query="";
            ComboBoxItem? selectedItem = GetByComboBox.SelectedItem as ComboBoxItem;
            switch (selectedItem!.Content.ToString())
            {
                case "Id":
                    query = @"Select * from Operators with(nolock) where Id=" + QueryTextBox.Text;
                    break;
                case "Name":
                    query = @"Select * from Operators with(nolock) where OC='" + QueryTextBox.Text+"'";
                    break;
                case "CountryId":
                    query = @"Select * from Operators with(nolock) where Country=" + QueryTextBox.Text;
                    break;
                case "Source_con":
                    query = @"Select * from Operators with(nolock) where Source_con=" + QueryTextBox.Text;
                    break;
            }
           
            var results = await connection.QueryAsync(query);

            dataGrid1.ItemsSource = results;
        }

      
    }
}
