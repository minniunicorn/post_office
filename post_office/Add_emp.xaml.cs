using MySql.Data.MySqlClient;
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
        readonly MySqlConnection connection = new MySqlConnection(connectionString); // Подключение к базе данных

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
                connection.Open(); // Открытие соединения с базой данных

                using (MySqlCommand command = connection.CreateCommand())
                {
                    // Вставка записи в таблицу "address" с использованием параметров
                    command.CommandText = "INSERT INTO address (postal_index, town, street, house, appart) VALUES " +
                        "(@postal_index, @town, @street, @house, @apart);";
                    command.Parameters.AddWithValue("@postal_index", postal_index);
                    command.Parameters.AddWithValue("@town", town);
                    command.Parameters.AddWithValue("@street", street);
                    command.Parameters.AddWithValue("@house", house);
                    command.Parameters.AddWithValue("@apart", apart);
                    command.ExecuteNonQuery();

                    // Вставка записи в таблицу "clients" с использованием параметров
                    command.CommandText = "INSERT INTO clients (name, surname, lastname, phone, address_id) VALUES " +
                        "(@firstname, @surname, @lastname, @phone, LAST_INSERT_ID()) ";
                    command.Parameters.AddWithValue("@firstname", firstname);
                    command.Parameters.AddWithValue("@surname", surname);
                    command.Parameters.AddWithValue("@lastname", lastname);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Клиент успешно добавлен в базу данных!"); // Вывод сообщения об успешном добавлении клиента в базу данных

                ClientsWindow employeesWindow = new ClientsWindow(fullname, wor_root, workerId); // Создание экземпляра окна "EmployeesWindow"
                employeesWindow.Show(); // Отображение окна "EmployeesWindow"
                Close(); // Закрытие текущего окна
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Ошибка при добавлении клиента в базу данных: {ex.Message}"); // Вывод сообщения об ошибке при добавлении клиента в базу данных
            }
            finally
            {
                connection.Close(); // Закрытие соединения с базой данных
            }
        }

        // Обработчик события нажатия кнопки "Отмена"
        public void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClientsWindow clientsWindow = new ClientsWindow(fullname, wor_root, workerId); // Создание экземпляра окна "EmployeesWindow"
            clientsWindow.Show(); // Отображение окна "EmployeesWindow"
            Close(); // Закрытие текущего окна
        }
    }
}
