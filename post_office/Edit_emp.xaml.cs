using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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

namespace post_office
{
    /// <summary>
    /// Логика взаимодействия для Edit_emp.xaml
    /// </summary>
    public partial class Edit_emp : Window
    {
        static readonly string connectionString = "Server=localhost;Database=post_office;Uid=root;Pwd=;";
        readonly MySqlConnection connection = new MySqlConnection(connectionString);
        readonly string fullname = "";
        readonly string wor_root = "";
        readonly int workerId = 0;
        private readonly int id_cl;

        public Edit_emp(int id, string name, string root, int work_id)
        {
            InitializeComponent();
            fullname = name;
            id_cl = id;
            wor_root = root;
            workerId= work_id;
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string firstname = Name_cl.Text;
            string surname = SurName.Text;
            string lastname = LastName.Text;
            string phone = Phone.Text;
            string postal_index = Postal_index.Text;
            string town = Town.Text;
            string street = Street.Text;
            string house = House.Text;
            string appart = Appart.Text;

            // Обновить запись выбранного клиента в базе данных
            string query = "UPDATE clients " +
                "JOIN address ON clients.address_id = address.id " +
                "SET clients.name = @firstname, clients.surname = @surname, clients.lastname = @lastname, " +
                "clients.phone = @phone, address.postal_index = @postal_index, address.town = @town, " +
                "address.street = @street, address.house = @house, address.appart = @appart " +
                "WHERE clients.id = @id";
            MySqlCommand command = new MySqlCommand(query, connection);
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
                EmployeesWindow employeesWindow = new EmployeesWindow(fullname, wor_root, workerId);
                employeesWindow.Show();
                Close();
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeesWindow employeesWindow = new EmployeesWindow(fullname, wor_root, workerId);
            employeesWindow.Show();
            Close();
        }
    }
}
