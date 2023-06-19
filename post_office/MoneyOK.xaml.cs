using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Threading;

namespace post_office
{
    /// <summary>
    /// Логика взаимодействия для MoneyOK.xaml
    /// </summary>
    public partial class MoneyOK : Window
    {
        readonly string fullname; // Хранит полное имя работника
        readonly string wor_root; // Хранит уровень доступа
        readonly int workerId; // Хранит идентификатор работника
        readonly DispatcherTimer timer = new DispatcherTimer(); // Таймер для обновления отображения времени
        static readonly string connectionString = "Server=localhost;Database=post_office;Uid=root;Pwd=;"; // Строка подключения для соединения с базой данных
        readonly MySqlConnection connection = new MySqlConnection(connectionString);

        public MoneyOK(string name, string root, int id, int id_mon)
        {
            InitializeComponent();
            fullname = name;
            wor_root = root;
            workerId = id;

            InsertStatic(id_mon); // Вставка статистики о добавлении перевода
            SetMoneyNumber(id_mon); // Установка номера перевода
            decimal sum = GetMoneyInfo(id_mon); // Получение информации о сумме перевода
            SetMoneyInfo(id_mon, sum); // Установка информации о сумме перевода
            SetSenderInfo(id_mon); // Получение информации об отправителе
            GetRecInfo(id_mon); // Получение информации о получателе
            fio.Content = name; // Установка имени пользователя
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            time.Content = DateTime.Now.ToString("HH:mm"); // Обновление отображения текущего времени
        }

        private void InsertStatic(int id_mon)
        {
            string action = $"Добавлен перевод #{id_mon}";
            Statics.InsertStatistic(action, workerId); // Вставка статистики о добавлении перевода
        }

        //Обработчик кнопки возвращения на главное окно
        private void Gotomain_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1(fullname, wor_root, workerId);
            window1.Show();
            Close();
        }

        //Установка номера перевода
        private void SetMoneyNumber(int moneyId)
        {
            using (connection)
            {
                connection.Open();
                string query = "SELECT id FROM remittance WHERE id = @moneyId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@moneyId", moneyId);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    num_mon.Content = "Номер перевода: " + result.ToString();
                }
            }
        }

        //Выбор суммы перевода
        private decimal GetMoneyInfo(int moneyId)
        {
            using (connection)
            {
                connection.Open();
                string query = "SELECT money_sum FROM remittance WHERE id = @moneyId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@moneyId", moneyId);
                decimal sum = Convert.ToDecimal(command.ExecuteScalar());
                return sum;
            }
        }

        //Установка информации о переводе
        private void SetMoneyInfo(int moneyId, decimal sum)
        {
            using (connection)
            {
                decimal percentage = 1.77m;
                decimal tarif = sum * (percentage / 100m);
                if (tarif < 29.50m)
                {
                    tarif = 29.50m;
                }

                decimal total_sum = Math.Round(tarif + sum, 2);
                connection.Open();

                string query = "UPDATE remittance SET total_sum = @total_sum WHERE id = @moneyId;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@moneyId", moneyId);
                command.Parameters.AddWithValue("@total_sum", total_sum);
                command.ExecuteNonQuery();
                totalsum.Content = "Сумма перевода: " + sum + " руб., Итоговая сумма: " + total_sum;
            }
        }

        //Установка информации об отправителе
        private void SetSenderInfo(int moneyId)
        {
            using (connection)
            {
                connection.Open();
                string query = "SELECT sender_id FROM remittance WHERE id = @moneyId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@moneyId", moneyId);
                int senderId = Convert.ToInt32(command.ExecuteScalar());

                string senderQuery = "SELECT CONCAT(surname, ' ', name, ' ', lastname) AS client_name FROM clients WHERE id = @senderId";
                MySqlCommand senderCommand = new MySqlCommand(senderQuery, connection);
                senderCommand.Parameters.AddWithValue("@senderId", senderId);
                MySqlDataReader reader = senderCommand.ExecuteReader();

                if (reader.Read())
                {
                    string fullName = reader.GetString("client_name");
                    send_inf.Content = fullName;
                }
            }
        }

        //Установка информации о получателе
        private void GetRecInfo(int moneyId)
        {
            using (connection)
            {
                connection.Open();
                string query = "SELECT recipient_id FROM remittance WHERE id = @moneyId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@moneyId", moneyId);
                int recId = Convert.ToInt32(command.ExecuteScalar());

                string recQuery = "SELECT CONCAT(surname, ' ', name, ' ', lastname) AS rec_name FROM recipient WHERE id = @recipient_id";
                MySqlCommand recCommand = new MySqlCommand(recQuery, connection);
                recCommand.Parameters.AddWithValue("@recipient_id", recId);
                MySqlDataReader reader = recCommand.ExecuteReader();

                if (reader.Read())
                {
                    string fullName = reader.GetString("rec_name");
                    rec_inf.Content = fullName;
                }
            }
        }
    }
}
