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

namespace Telecom_Operator_Repair
{
    /// <summary>
    /// Логика взаимодействия для Editing_Order.xaml
    /// </summary>
    public partial class Editing_Order : Window
    {
        private int userId;

        public Editing_Order(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }
    }
}
