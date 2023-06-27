using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace post_office
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Operations : Window
    {
        readonly DispatcherTimer timer = new DispatcherTimer();
        //static readonly string connectionString = "Server=localhost;Database=Kursovaya_rabota;Uid=root;Pwd=;";
        //readonly MySqlConnection connection = new MySqlConnection(connectionString);

        readonly string fullname = "";
        readonly string wor_root = "";
        readonly int workerId = 0;
        public Operations(string name, string root, int id)
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            fio.Content = name;
            fullname = name;
            wor_root = root;
            workerId = id;
        }
        
        private void Timer_Tick(object sender, EventArgs e)
        {
            time.Content = DateTime.Now.ToString("HH:mm");
        }
        private void Clients_Click(object sender, RoutedEventArgs e)
        {
            ClientsWindow employeesWindow = new ClientsWindow(fullname, wor_root, workerId);
            employeesWindow.Show();
            Close();
            Statics.InsertStatistic("Просмотр клиентов", workerId);
        }

        private void BtnShipments_Click(object sender, RoutedEventArgs e)
        {
            ShipmentsWindow shipmentsWindow = new ShipmentsWindow(fullname, wor_root, workerId);
            shipmentsWindow.Show();
            Close();
            Statics.InsertStatistic("Просмотр посылок", workerId);
        }

        private void BtnMailReceiving_Click(object sender, RoutedEventArgs e)
        {
            MailReceivingWindow mailReceivingWindow = new MailReceivingWindow(fullname, wor_root, workerId);
            mailReceivingWindow.Show();
            Close();
            Statics.InsertStatistic("Прием почты", workerId);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
            Statics.InsertStatistic("Выход", workerId);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (wor_root == "a")
            {
                Button button = new Button
                {
                    Content = "Данные о работниках"
                };
                button.Click += Workers_Click;
                // Установка свойств Grid.Column и Grid.Row для кнопки
                Grid.SetColumnSpan(button, 3); // Устанавливаем третий столбец (индексация начинается с 0)
                Grid.SetRow(button, 0); // Устанавливаем первую строку (индексация начинается с 0)
                button.HorizontalAlignment = HorizontalAlignment.Right;
                button.VerticalAlignment = VerticalAlignment.Center;
                button.FontSize = 16;
                button.Margin = new Thickness(10);
                button.Padding = new Thickness(5);
                button.FontFamily = new FontFamily("Montserrat Alternates");
                button.Background = new SolidColorBrush(Color.FromRgb(43, 146, 205)); // Задаем цвет фона
                button.Foreground = Brushes.White; // Задаем цвет текста
                button.BorderBrush = new SolidColorBrush(Color.FromRgb(43, 146, 205)); // Задаем цвет границы
                // Добавление кнопки в элемент Grid
                grid.Children.Add(button);

            }            
        }
        private void Workers_Click(object sender, RoutedEventArgs e)
        {
            Workers workers = new Workers(fullname, wor_root, workerId);
            workers.Show();
            this.Close();
            Statics.InsertStatistic("Просмотр работников", workerId);
        }

        private void Money_Click(object sender, RoutedEventArgs e)
        {
            MoneySending moneySending = new MoneySending(fullname, wor_root, workerId);
            moneySending.Show();
            this.Close();
            Statics.InsertStatistic("Денежные переводы", workerId);

        }
    }
}
