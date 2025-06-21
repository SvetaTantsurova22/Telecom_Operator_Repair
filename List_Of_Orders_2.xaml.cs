using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using Entities;
using Telecom_Operator_Repair.Repository;

namespace Telecom_Operator_Repair
{
    /// <summary>
    /// Логика взаимодействия для List_Of_Orders_2.xaml
    /// </summary>
    public partial class List_Of_Orders_2 : Window
    {
        private readonly string connectionString = "MSI\\MSSQLSERVER01;Initial Catalog=Telecom_Operator_Repair;Integrated Security=True;Encrypt=False";
        private int userId;

        public List_Of_Orders_2(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadOrders();
            LoadInstallers();
        }
        private void LoadInstallers()
        {
            string query = "SELECT ID, FullName FROM Users WHERE UserRole = 3";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                Dictionary<int, string> installers = new Dictionary<int, string>();
                while (reader.Read())
                {
                    installers.Add(reader.GetInt32(0), reader.GetString(1));
                }

                InstallerComboBox.ItemsSource = installers;
                InstallerComboBox.DisplayMemberPath = "Value";
                InstallerComboBox.SelectedValuePath = "Key";
            }
        }

        private void AssignInstaller_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is OrderDisplay selectedOrder &&
                InstallerComboBox.SelectedValue is int installerId)
            {
                string query = "UPDATE Orders SET AssignedTo = @InstallerId, StatusID = 2 WHERE ID = @OrderId";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@InstallerId", installerId);
                    cmd.Parameters.AddWithValue("@OrderId", selectedOrder.OrderId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Монтажник назначен.");
                LoadOrders();
            }
            else
            {
                MessageBox.Show("Выберите заказ и монтажника.");
            }
        }



        public void LoadOrders()
        {
            List<OrderDisplay> orders = new List<OrderDisplay>();

            string query = @"
        SELECT 
            o.ID,
            CONVERT(varchar, o.CreatedDate, 104) AS OrderDate,
            o.Address,
            o.CustomerAccount,
            o.CustomerName + ' ' + o.CustomerPhone AS ContactInfo,
            o.OrderType
        FROM Orders o
        WHERE o.StatusID IN (1, 2) -- Показываем только активные заказы";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    orders.Add(new OrderDisplay
                    {
                        OrderId = reader.GetInt32(0),
                        OrderDate = reader.GetString(1),
                        Address = reader.GetString(2),
                        AccountNumber = reader.GetString(3),
                        ContactInfo = reader.GetString(4),
                        ServiceType = reader.GetString(5)
                    });
                }
            }

            OrdersDataGrid.ItemsSource = orders;
        }

        private void OrdersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is OrderDisplay selected)
            {
                SelectedOrderId.Text = selected.OrderId.ToString();
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var a = new Dispetcher_Window(userId);
            a.Show();
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
