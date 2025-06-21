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

namespace Telecom_Operator_Repair
{
    /// <summary>
    /// Логика взаимодействия для Admin_Window.xaml
    /// </summary>
    public partial class Admin_Window : Window
    {
        private int userId;

        public Admin_Window(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var a = new List_Of_Orders(userId);
            a.Show();
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var a = new Create_Order(userId);
            a.Show();
            this.Hide();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var listinstaller = new List_Of_Installer(userId);
            listinstaller.Show();
            this.Hide();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var raspisanie = new Schedule_Installer(userId);
            raspisanie.Show();
            this.Hide();
        }
    }
}
