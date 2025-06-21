using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Telecom_Operator_Repair
{
    /// <summary>
    /// Логика взаимодействия для Installer_Window.xaml
    /// </summary>
    public partial class Installer_Window : Window
    {
        private int userId;
        private string connectionString = "Data Source=MSI\\MSSQLSERVER01;Initial Catalog=Telecom_Operator_Repair;Integrated Security=True;Encrypt=False";
        private ObservableCollection<Order> orders;

        public Installer_Window(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                orders = new ObservableCollection<Order>();

                string query = @"
        SELECT 
            ID, 
            OrderType,
            CustomerAccount AS AccountNumber, 
            CustomerName,
            CustomerPhone,
            Address, 
            CreatedDate, 
            CreatedBy,
            StatusID,
            (SELECT StatusName FROM OrderStatuses WHERE ID = StatusID) AS StatusName,
            Notes AS Comment,
            AssignedTo
        FROM Orders
        WHERE AssignedTo = @UserId";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ID")),
                                OrderType = reader.GetString(reader.GetOrdinal("OrderType")),
                                AccountNumber = reader.GetString(reader.GetOrdinal("AccountNumber")),
                                CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                                CustomerPhone = reader.GetString(reader.GetOrdinal("CustomerPhone")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                                StatusId = reader.GetInt32(reader.GetOrdinal("StatusID")),
                                StatusName = reader.IsDBNull(reader.GetOrdinal("StatusName")) ? "Неизвестен" : reader.GetString(reader.GetOrdinal("StatusName")),
                                Comment = reader.IsDBNull(reader.GetOrdinal("Comment")) ? "" : reader.GetString(reader.GetOrdinal("Comment")),
                                AssignedTo = reader.GetInt32(reader.GetOrdinal("AssignedTo"))
                            });
                        }
                    }
                }

                MessageBox.Show($"Загружено заказов: {orders.Count}");
                OrdersDataGrid.ItemsSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заказов: {ex.Message}");
            }
        }



        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(OrderIdTextBox.Text, out int orderId))
            {
                MessageBox.Show("Выберите заказ для сохранения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string selectedStatus = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (string.IsNullOrEmpty(selectedStatus))
            {
                MessageBox.Show("Выберите статус.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string comment = CommentTextBox.Text?.Trim() ?? "";

            int statusId = GetStatusIdByName(selectedStatus);
            if (statusId == 0)
            {
                MessageBox.Show("Статус не найден в базе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string query = @"
            UPDATE Orders
            SET StatusID = @StatusID, Notes = @Notes
            WHERE ID = @OrderId";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StatusID", statusId);
                    cmd.Parameters.AddWithValue("@Notes", comment);
                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    conn.Open();
                    int affected = cmd.ExecuteNonQuery();

                    if (affected > 0)
                    {
                        MessageBox.Show("Данные успешно сохранены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadOrders();
                    }
                    else
                    {
                        MessageBox.Show("Заказ не найден или не обновлён.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private int GetStatusIdByName(string statusName)
        {
            int statusId = 0;
            string query = "SELECT ID FROM OrderStatuses WHERE StatusName = @StatusName";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StatusName", statusName);
                conn.Open();

                var result = cmd.ExecuteScalar();
                if (result != null)
                    statusId = Convert.ToInt32(result);
            }

            return statusId;
        }

        private void OrdersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOrder = OrdersDataGrid.SelectedItem as Order;
            if (selectedOrder != null)
            {
                OrderIdTextBox.Text = selectedOrder.Id.ToString();

                // Устанавливаем статус в ComboBox
                foreach (ComboBoxItem item in StatusComboBox.Items)
                {
                    if ((string)item.Content == selectedOrder.StatusName)
                    {
                        StatusComboBox.SelectedItem = item;
                        break;
                    }
                }

                CommentTextBox.Text = selectedOrder.Comment;
            }
            else
            {
                OrderIdTextBox.Clear();
                StatusComboBox.SelectedIndex = 0;
                CommentTextBox.Clear();
            }
        }

    }
}
