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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using Entities;
using Telecom_Operator_Repair.Repository;

namespace Telecom_Operator_Repair
{
    
    public partial class LogIn : Window
    {
        private readonly string connectionString = "Data Source=MSI\\MSSQLSERVER01;Initial Catalog=Telecom_Operator_Repair;Integrated Security=True;Encrypt=False";
        private int userId;

        public LogIn()
        {
            InitializeComponent();
        }

        public LogIn(int userId) : this()
        {
            this.userId = userId;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            UserRepository userRepository = new UserRepository(connectionString);
            string roleName;
            User user = userRepository.AuthenticateUser(username, password, out roleName);

            if (user != null)
            {
                MessageBox.Show($"Добро пожаловать, {user.FullName} ({roleName})!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                Window mainWindow = null;

                switch (roleName)
                {
                    case "Администратор":
                        mainWindow = new Admin_Window(user.ID);
                        break;
                    case "Диспетчер":
                        mainWindow = new Dispetcher_Window(user.ID);
                        break;
                    case "Монтажник":
                        mainWindow = new Installer_Window(user.ID);
                        break;
                    default:
                        MessageBox.Show("Неизвестная роль пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                }

                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
        
    

