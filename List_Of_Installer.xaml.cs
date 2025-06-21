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
using System.Data.SqlClient;
using Entities;
using System.Collections.ObjectModel;

namespace Telecom_Operator_Repair
{
    /// <summary>
    /// Логика взаимодействия для List_Of_Installer.xaml
    /// </summary>
    public partial class List_Of_Installer : Window
    {
        private int userId;
        private string connectionString = "Data Source=MSI\\MSSQLSERVER01;Initial Catalog=Telecom_Operator_Repair;Integrated Security=True;Encrypt=False";
        private ObservableCollection<Installer> installers;

        public List_Of_Installer(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadInstallers();
        }

        private void LoadInstallers()
        {
            installers = new ObservableCollection<Installer>();

            string query = @"
        SELECT 
            u.ID, 
            u.FullName, 
            u.Phone,
            CASE 
                WHEN EXISTS (
                    SELECT 1 FROM Orders o 
                    WHERE o.AssignedTo = u.ID AND o.StatusID IN (2, 3) -- В работе, Требует доработки
                ) THEN 'Занят'
                ELSE 'Свободен'
            END AS Status
        FROM Users u
        WHERE u.UserRole = 3";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        installers.Add(new Installer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ID")),
                            FullName = reader.GetString(reader.GetOrdinal("FullName")),
                            PhoneNumber = reader.GetString(reader.GetOrdinal("Phone")),
                            Status = reader.GetString(reader.GetOrdinal("Status"))
                        });
                    }
                }
            }

            InstallersDataGrid.ItemsSource = installers;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var adminform = new Admin_Window(userId);
            adminform.Show();
            this.Hide();
        }
    }
}
