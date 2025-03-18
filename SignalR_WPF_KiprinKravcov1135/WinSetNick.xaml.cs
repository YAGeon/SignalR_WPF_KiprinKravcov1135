using Microsoft.AspNetCore.SignalR.Client;
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
    /// Логика взаимодействия для WinSetNick.xaml
    /// </summary>
    public partial class WinSetNick : Window
    {
        private readonly HubConnection connection;

        public string Nick { get; set; }
        public string Message { get; set; }

        public WinSetNick(Microsoft.AspNetCore.SignalR.Client.HubConnection _connection, string s)
        {
            InitializeComponent();
            Message = s;
            DataContext = this;
            connection = _connection;
        }

        private async void Send(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Nick))
            {
                MessageBox.Show("Ник не может быть пустым");
                return;
            }
            await connection.SendAsync("Nickname", Nick);
            Close();
        }
    }
}
