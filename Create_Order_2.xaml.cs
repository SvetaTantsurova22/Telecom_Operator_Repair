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
using Entities;
using Telecom_Operator_Repair.Repository;

namespace Telecom_Operator_Repair
{
    /// <summary>
    /// Логика взаимодействия для Create_Order_2.xaml
    /// </summary>
    public partial class Create_Order_2 : Window
    {
        private readonly string connectionString = "Data Source=MSI\\MSSQLSERVER01;Initial Catalog=Telecom_Operator_Repair;Integrated Security=True;Encrypt=False";
        private int userId;

        public Create_Order_2(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string address = AddressTextBox.Text.Trim();
            string account = AccountTextBox.Text.Trim();
            string contactInfo = ContactInfoTextBox.Text.Trim();
            string orderType = (ServiceTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            DateTime createdDate = OrderDatePicker.SelectedDate ?? DateTime.Now;

            if (string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(contactInfo) || string.IsNullOrWhiteSpace(orderType))
            {
                MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string[] contactParts = contactInfo.Split(',');
            if (contactParts.Length < 2)
            {
                MessageBox.Show("Введите контактные данные в формате: 'ФИО, Телефон'", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var order = new Order
            {
                Address = address,
                AccountNumber = account,
                CustomerName = contactParts[0].Trim(),
                CustomerPhone = contactParts[1].Trim(),
                OrderType = orderType,
                CreatedDate = createdDate,
                CreatedBy = userId
            };

            var repo = new RepairRepository(connectionString);
            bool success = repo.AddOrder(order);

            if (success)
            {
                MessageBox.Show("Заказ добавлен успешно!", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                var createform = new Create_Order(userId);
                createform.Show();
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении заказа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var adminform = new Dispetcher_Window(userId);
            adminform.Show();
            this.Hide();
        }
    }
}
