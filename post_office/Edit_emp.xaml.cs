using MySql.Data.MySqlClient;
using System;
using System.Windows;

namespace post_office
{
    /// <summary>
    /// Логика взаимодействия для Edit_emp.xaml
    /// </summary>
    public partial class Edit_emp : Window
    {
        // Поля класса
        static readonly string connectionString = "Server=localhost;Database=post_office;Uid=root;Pwd=;"; // Строка подключения к базе данных
        readonly MySqlConnection connection = new MySqlConnection(connectionString); // Подключение к базе данных
        // Необходимые глобалиные переменные
        readonly string fullname = ""; // Полное имя работника
        readonly string wor_root = ""; // Права работника
        readonly int workerId = 0; // Идентификатор работника
        private readonly int id_cl; // Идентификатор клиента 

        public Edit_emp(int id, string name, string root, int work_id)
        {
            InitializeComponent();
            fullname = name;
            id_cl = id;
            wor_root = root;
            workerId = work_id;

            // Запросить данные выбранного клиента из базы данных
            string query = "SELECT clients.*, address.* " +
               "FROM clients " +
               "JOIN address ON clients.address_id = address.id " +
               "WHERE clients.id = " + id_cl;

            MySqlCommand command = new MySqlCommand(query, connection);

            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                // Заполнить поля формы WPF данными выбранного клиента
                if (reader.Read())
                {
                    Name_cl.Text = reader["name"].ToString();
                    SurName.Text = reader["surname"].ToString();
                    LastName.Text = reader["lastname"].ToString();
                    Phone.Text = reader["phone"].ToString();
                    Postal_index.Text = reader["postal_index"].ToString();
                    Town.Text = reader["town"].ToString();
                    Street.Text = reader["street"].ToString();
                    House.Text = reader["house"].ToString();
                    Appart.Text = reader["appart"].ToString();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Ошибка при запросе данных клиента из базы данных: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        // Обработчик события нажатия кнопки "Сохранить"
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение значений полей из текстовых полей
            string firstname = Name_cl.Text; // Имя
            string surname = SurName.Text; // Фамилия
            string lastname = LastName.Text; // Отчество
            string phone = Phone.Text; // Телефон
            string postal_index = Postal_index.Text; // Почтовый индекс
            string town = Town.Text; // Город
            string street = Street.Text; // Улица
            string house = House.Text; // Дом
            string appart = Appart.Text; // Квартира

            // Обновить запись выбранного клиента в базе данных
            string query = "UPDATE clients " +
                "JOIN address ON clients.address_id = address.id " +
                "SET clients.name = @firstname, clients.surname = @surname, clients.lastname = @lastname, " +
                "clients.phone = @phone, address.postal_index = @postal_index, address.town = @town, " +
                "address.street = @street, address.house = @house, address.appart = @appart " +
                "WHERE clients.id = @id";
            MySqlCommand command = new MySqlCommand(query, connection);

            // Параметры команды SQL
            command.Parameters.AddWithValue("@firstname", firstname);
            command.Parameters.AddWithValue("@surname", surname);
            command.Parameters.AddWithValue("@lastname", lastname);
            command.Parameters.AddWithValue("@phone", phone);
            command.Parameters.AddWithValue("@postal_index", postal_index);
            command.Parameters.AddWithValue("@town", town);
            command.Parameters.AddWithValue("@street", street);
            command.Parameters.AddWithValue("@house", house);
            command.Parameters.AddWithValue("@appart", appart);
            command.Parameters.AddWithValue("@id", id_cl);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                MessageBox.Show("Данные клиента успешно обновлены в базе данных!");

                string action = "Изменены данные клиента #" + id_cl;
                Statistic.InsertStatistic(action, workerId);

                // Создание экземпляра окна "EmployeesWindow" и отображение его
                ClientsWindow clientsWindow = new ClientsWindow(fullname, wor_root, workerId);
                clientsWindow.Show();
                Close(); // Закрытие текущего окна
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных клиента в базе данных: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        // Обработчик события нажатия кнопки "Отмена"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Создание экземпляра окна "EmployeesWindow" и отображение его
            ClientsWindow clientsWindow = new ClientsWindow(fullname, wor_root, workerId);
            clientsWindow.Show();
            Close(); // Закрытие текущего окна
        }
    }
}
