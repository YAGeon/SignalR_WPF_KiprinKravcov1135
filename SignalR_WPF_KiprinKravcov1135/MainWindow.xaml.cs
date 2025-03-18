using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SignalR_WPF_KiprinKravcov1135
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        HubConnection _connection;
        private string opponent;
        private bool myTurn;
        string myChar = string.Empty;
        private string nickName;

        public string NickName
        {
            get => nickName;
            set
            {
                nickName = value;
                Signal();
            }
        }

        public string Opponent
        {
            get => opponent;
            set
            {
                opponent = value;
                Signal();
            }
        }
        public bool MyTurn
        {
            get => myTurn;
            set
            {
                myTurn = value;
                Signal();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void Signal([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public MainWindow()
        {
            InitializeComponent();

            CreateConnection();
            DataContext = this;
        }

        string gameid = string.Empty;

        private void CreateConnection()
        {
            var win = new WinOptions();
            if (win.ShowDialog() != true)
            {
                Close();
            }
            string address = win.Address;
            _connection = new HubConnectionBuilder().
                            AddJsonProtocol(s =>
                            {
                                s.PayloadSerializerOptions.ReferenceHandler =
                                System.Text.Json.Serialization.ReferenceHandler.Preserve;
                            }
                            ).
                        WithUrl(address + "/game").
                        Build();

            _connection.StartAsync();

            Unloaded += async (s, e) => await _connection.StopAsync();
        }


        private async void MakeTurn(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button.Content == null)
            {
                button.Content = myChar;
                MyTurn = false;
                await _connection.SendAsync("MakeTurn",
                    new Turn
                    {
                        GameId = gameid,
                        Button = button.Name,
                        Char = myChar
                    });
            }
        }

        private void Letter_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            foreach (var item in OwnedWindows)
            {
                if (button.Content.ToString() == item)
                {
                    return;
                }
            }
            YakubovichWord.Text = "Нет такой буквы :(";
            button.Background = new SolidColorBrush(Colors.Red);
        }

        private void FullWord_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены?", "Предупреждение!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (QuestionBlock.Text == FullAnswerBlock.Text)
                {
                    YakubovichWord.Text = "Ты молодец! Держи!";
                    if (MessageBox.Show("Хотите ещё?", "Вы победили!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                        this.Close();
                }
                else
                {
                    YakubovichWord.Text = "И это не правильный ответ! Вы проиграли :_(";
                    if (MessageBox.Show("Попробовать снова?", "Вы проиграли!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                        this.Close();
                }
            }
        }
    }
}