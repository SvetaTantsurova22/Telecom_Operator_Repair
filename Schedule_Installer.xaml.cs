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
    /// Логика взаимодействия для Schedule_Installer.xaml
    /// </summary>
    public partial class Schedule_Installer : Window
    {
        private int userId;
        private string connectionString = "Data Source=MSI\\MSSQLSERVER01;Initial Catalog=Telecom_Operator_Repair;Integrated Security=True;Encrypt=False";
        private ObservableCollection<InstallerSchedule> schedules;

        public Schedule_Installer(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadSchedule();
        }

        private void LoadSchedule()
        {
            schedules = new ObservableCollection<InstallerSchedule>();

            string query = @"
        SELECT 
            o.ID AS OrderId,
            CONVERT(varchar, s.WorkDate, 104) + ' ' + LEFT(CONVERT(varchar, s.StartTime, 108), 5) + '-' + LEFT(CONVERT(varchar, s.EndTime, 108), 5) AS Date,
            o.Address,
            o.CustomerName + ' ' + o.CustomerPhone AS CustomerContacts,
            u.FullName AS InstallerName
        FROM Schedule s
        JOIN Users u ON s.UserID = u.ID
        LEFT JOIN Orders o ON s.OrderID = o.ID
        WHERE s.OrderID IS NOT NULL
        ORDER BY s.WorkDate, s.StartTime";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        schedules.Add(new InstallerSchedule
                        {
                            OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                            Date = reader.GetString(reader.GetOrdinal("Date")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            CustomerContacts = reader.GetString(reader.GetOrdinal("CustomerContacts")),
                            InstallerName = reader.GetString(reader.GetOrdinal("InstallerName"))
                        });
                    }
                }
            }

            // Назначаем источник данных для DataGrid
            this.Dispatcher.Invoke(() =>
            {
                ((DataGrid)this.FindName("ScheduleGrid")).ItemsSource = schedules;
            });
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var adminform = new Admin_Window(userId);
            adminform.Show();
            this.Hide();
        }
    }
}
