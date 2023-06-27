using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Threading;
using static Xceed.Wpf.Toolkit.Calculator;

namespace post_office
{
    public partial class MailOK : Window
    {
        // Переменные
        readonly string fullname;
        readonly string wor_root;
        readonly int workerId;
        readonly int pack_id;
        readonly int client_id;
        readonly DispatcherTimer timer = new DispatcherTimer();
        // Строка подключения к базе данных MySQL
        private readonly string connectionString = "Server=localhost;Database=post_office;Uid=root;Pwd=;";

        // Конструктор окна MailOK
        public MailOK(string name, string root, int id, int id_pack, int id_client)
        {
            InitializeComponent();
            fullname = name;
            wor_root = root;
            workerId = id;
            pack_id = id_pack;
            client_id = id_client;
            SetPackageNumber(pack_id);
            SetTypeAndRank(pack_id);
            SetPackageInfo(pack_id);
            SetStorageInfo(pack_id);
            SetClientInfo(client_id);
            SetClientAdressInfo(client_id);
            InsertStatic(id_pack);
            fio.Content = name;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        // Обработчик события таймера для отображения текущего времени
        private void Timer_Tick(object sender, EventArgs e)
        {
            time.Content = DateTime.Now.ToString("HH:mm");
        }

        // Вставка записи в таблицу статистики
        private void InsertStatic(int id_pack)
        {
            string action = $"Добавлено отправление #{id_pack}";
            Statistic.InsertStatistic(action, workerId);
        }

        // Обработчик кнопки "На главную"
        private void Gotomain_Click(object sender, RoutedEventArgs e)
        {
            Operations operations = new Operations(fullname, wor_root, workerId);
            operations.Show();
            Close();
        }

        // Установка номера посылки
        private void SetPackageNumber(int packageId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id FROM package WHERE id = @packageId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@packageId", packageId);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    num_pack.Content = "Номер посылки: " + result.ToString();
                }
            }
        }

        // Установка типа и разряда посылки
        private void SetTypeAndRank(int packageId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT type, pac_rank FROM package WHERE id = @packageId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@packageId", packageId);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string type = reader["type"].ToString();
                    string rank = reader["pac_rank"].ToString();
                    type_rank.Content = $"Тип: {type}, Разряд: {rank}";
                }
                reader.Close();
            }
        }

        // Установка информации о посылке
        private void SetPackageInfo(int packageId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT weight FROM package WHERE id = @packageId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@packageId", packageId);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string weight = reader["weight"].ToString();
                    string packageInfo = $"Вес: {weight} кг";
                    package_inf.Content = packageInfo;
                }
                reader.Close();
            }
        }

        // Установка информации о месте хранения
        private void SetStorageInfo(int packageID)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT s.number FROM package p JOIN storage s ON p.storage_id = s.id WHERE p.id = @packageId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@packageId", packageID);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string number = reader["number"].ToString();
                    string storageInfo = $", Место хранения: {number}";
                    package_inf.Content += storageInfo;
                }
                reader.Close();
            }
        }

        // Установка информации о клиенте
        private void SetClientInfo(int clientId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT CONCAT(surname, ' ', name, ' ', lastname) AS client_name FROM clients WHERE id = @clientId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@clientId", clientId);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string fullName = reader["client_name"].ToString();
                    string clientInfo = $"{fullName}";
                    client_inf.Content = clientInfo;
                }
                reader.Close();
            }
        }

        // Установка информации об адресе клиента
        private void SetClientAdressInfo(int clientId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT CONCAT('\n', a.postal_index, ', г.', a.town, ', ул.', a.street, ', д.', a.house, ', кв.', a.appart) AS full_address " +
                    "FROM clients c JOIN address a ON c.address_id = a.id WHERE c.id = @clientId;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@clientId", clientId);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string fullAddress = reader["full_address"].ToString();
                    client_inf.Content += fullAddress;
                }
                reader.Close();
            }
        }

        // Обработчик кнопки "Новая посылка"
        private void NewPack_Click(object sender, RoutedEventArgs e)
        {
            MailReceivingWindow mailReceivingWindow = new MailReceivingWindow(fullname, wor_root, workerId);
            mailReceivingWindow.Show();
            Close();
        }
    }
}
