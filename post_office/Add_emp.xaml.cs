using MySql.Data.MySqlClient;
using System;
using System.Windows;

namespace post_office
{
    /// <summary>
    /// Логика взаимодействия для Add_emp.xaml
    /// </summary>
    public partial class Add_emp : Window
    {
        // Необходимые глобалиные переменные
        readonly string fullname = ""; // Полное имя работника
        readonly string wor_root = ""; // Права работника
        readonly int workerId = 0; // Идентификатор работника
        static readonly string connectionString = "Server=localhost;Database=post_office;Uid=root;Pwd=;"; // Строка подключения к базе данных

        public Add_emp(string name, string root, int id)
        {
            InitializeComponent();
            fullname = name;
            wor_root = root;
            workerId = id;
        }

        // Обработчик события нажатия кнопки "Сохранить"
        public void SaveButton_Click(object sender, RoutedEventArgs e)
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
            string apart = Apart.Text; // Квартира

            try
            {
                ClientsWindow clientsWindow = new ClientsWindow(fullname, wor_root, workerId);
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Проверка наличия клиента в базе данных
                    string checkQuery = "SELECT COUNT(*) FROM clients c JOIN address a ON c.address_id = a.id " +
                                        "WHERE c.name = @firstname AND c.surname = @surname AND c.lastname = @lastname AND a.town = @town AND AND a.street = @street AND a.house = @house AND a.appart = @appart";
                    MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@firstname", firstname);
                    checkCommand.Parameters.AddWithValue("@surname", surname);
                    checkCommand.Parameters.AddWithValue("@lastname", lastname);
                    checkCommand.Parameters.AddWithValue("@town", town);
                    checkCommand.Parameters.AddWithValue("@street", street);
                    checkCommand.Parameters.AddWithValue("@house", house);
                    checkCommand.Parameters.AddWithValue("@apart", apart);

                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Клиент уже существует в базе данных!");
                        clientsWindow.LoadData();
                        clientsWindow.Show();
                        Close();
                        return; // Прерывание выполнения метода, чтобы клиент не был добавлен повторно
                    }

                    // Вставка записи в таблицу "address" с использованием параметров
                    string addressInsertQuery = "INSERT INTO address (postal_index, town, street, house, appart) VALUES " +
                        "(@postal_index, @town, @street, @house, @apart)";
                    MySqlCommand addressInsertCommand = new MySqlCommand(addressInsertQuery, connection);
                    addressInsertCommand.Parameters.AddWithValue("@postal_index", postal_index);
                    addressInsertCommand.Parameters.AddWithValue("@town", town);
                    addressInsertCommand.Parameters.AddWithValue("@street", street);
                    addressInsertCommand.Parameters.AddWithValue("@house", house);
                    addressInsertCommand.Parameters.AddWithValue("@apart", apart);
                    addressInsertCommand.ExecuteNonQuery();

                    // Вставка записи в таблицу "clients" с использованием параметров
                    string clientsInsertQuery = "INSERT INTO clients (name, surname, lastname, phone, address_id) VALUES " +
                        "(@firstname, @surname, @lastname, @phone, LAST_INSERT_ID())";
                    MySqlCommand clientsInsertCommand = new MySqlCommand(clientsInsertQuery, connection);
                    clientsInsertCommand.Parameters.AddWithValue("@firstname", firstname);
                    clientsInsertCommand.Parameters.AddWithValue("@surname", surname);
                    clientsInsertCommand.Parameters.AddWithValue("@lastname", lastname);
                    clientsInsertCommand.Parameters.AddWithValue("@phone", phone);
                    clientsInsertCommand.ExecuteNonQuery();

                    // Вставка в статистику
                    string getLastInsertIdQuery = "SELECT LAST_INSERT_ID()";
                    MySqlCommand getLastInsertIdCommand = new MySqlCommand(getLastInsertIdQuery, connection);
                    int result = Convert.ToInt32(getLastInsertIdCommand.ExecuteScalar());
                    string action = "Добавлен клиент #" + result;
                    Statistic.InsertStatistic(action, workerId);
                }

                MessageBox.Show("Клиент успешно добавлен в базу данных!");
                clientsWindow.LoadData();
                clientsWindow.Show();
                Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Ошибка при добавлении клиента в базу данных: {ex.Message}");
            }
        }


        // Обработчик события нажатия кнопки "Отмена"
        public void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClientsWindow clientsWindow = new ClientsWindow(fullname, wor_root, workerId); 
            clientsWindow.Show(); // Отображение окна ClientsWindow
            Close(); // Закрытие текущего окна
        }
    }
}
