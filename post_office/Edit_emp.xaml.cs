﻿using MySql.Data.MySqlClient;
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

            try
            {
                connection.Open();

                // Обновить запись выбранного клиента в базе данных
                string clientsQuery = "UPDATE clients " +
                    "SET name = @firstname, surname = @surname, lastname = @lastname, phone = @phone " +
                    "WHERE id = @id";
                string addressQuery = "UPDATE address " +
                    "SET postal_index = @postal_index, town = @town, street = @street, house = @house, appart = @appart " +
                    "WHERE id = (SELECT address_id FROM clients WHERE id = @id)";
                using (MySqlCommand clientsCommand = new MySqlCommand(clientsQuery, connection))
                {
                    // Параметры команды SQL для обновления таблицы "clients"
                    clientsCommand.Parameters.AddWithValue("@firstname", firstname);
                    clientsCommand.Parameters.AddWithValue("@surname", surname);
                    clientsCommand.Parameters.AddWithValue("@lastname", lastname);
                    clientsCommand.Parameters.AddWithValue("@phone", phone);
                    clientsCommand.Parameters.AddWithValue("@id", id_cl);
                    clientsCommand.ExecuteNonQuery();
                }

                using (MySqlCommand addressCommand = new MySqlCommand(addressQuery, connection))
                {
                    // Параметры команды SQL для обновления таблицы "address"
                    addressCommand.Parameters.AddWithValue("@postal_index", postal_index);
                    addressCommand.Parameters.AddWithValue("@town", town);
                    addressCommand.Parameters.AddWithValue("@street", street);
                    addressCommand.Parameters.AddWithValue("@house", house);
                    addressCommand.Parameters.AddWithValue("@appart", appart);
                    addressCommand.Parameters.AddWithValue("@id", id_cl);
                    addressCommand.ExecuteNonQuery();
                }
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
