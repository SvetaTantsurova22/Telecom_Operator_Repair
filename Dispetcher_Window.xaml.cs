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
    /// Логика взаимодействия для Dispetcher_Window.xaml
    /// </summary>
    public partial class Dispetcher_Window : Window
    {
        private int userId;
        public Dispetcher_Window(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var create = new Create_Order_2(userId);
            create.Show();
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var rasp = new Schedule_Installer(userId);
            rasp.Show();
            this.Hide();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var listmont = new List_Of_Installer(userId);
            listmont.Show();
            this.Hide();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var list = new List_Of_Orders_2(userId);
            list.Show(); 
            this.Hide();
        }
    }
}
