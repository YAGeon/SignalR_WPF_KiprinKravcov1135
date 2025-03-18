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

namespace SignalR_WPF_KiprinKravcov1135
{
    /// <summary>
    /// Логика взаимодействия для WinOptions.xaml
    /// </summary>
    public partial class WinOptions : Window
    {
        public string Address { get; set; } = "http://localhost:5050";
        public WinOptions()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void SetAddress(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
