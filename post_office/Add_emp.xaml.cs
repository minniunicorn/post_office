using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
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

namespace post_office
{
    /// <summary>
    /// Логика взаимодействия для Add_emp.xaml
    /// </summary>
    public partial class Add_emp : Window
    {
        readonly string fullname = "";
        readonly string wor_root = "";
        readonly int workerId = 0;
        static readonly string connectionString = "Server=localhost;Database=post_office;Uid=root;Pwd=;";
        readonly MySqlConnection connection = new MySqlConnection(connectionString);

        public Add_emp(string name, string root, int id)
        {
            InitializeComponent();
            fullname = name;
            wor_root = root;
            workerId = id;
        }

        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string firstname = Name_cl.Text;
            string surname = SurName.Text;
            string lastname = LastName.Text;
            string phone = Phone.Text;
            string postal_index = Postal_index.Text;
            string town = Town.Text;
            string street = Street.Text;
            string house = House.Text;
            string apart = Apart.Text;

            try
            {
                connection.Open();

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO address (postal_index, town, street, house, appart) VALUES " +
                        "(@postal_index, @town, @street, @house, @apart);";
                    command.Parameters.AddWithValue("@postal_index", postal_index);
                    command.Parameters.AddWithValue("@town", town);
                    command.Parameters.AddWithValue("@street", street);
                    command.Parameters.AddWithValue("@house", house);
                    command.Parameters.AddWithValue("@apart", apart);
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO clients (name, surname, lastname, phone, address_id) VALUES " +
                        "(@firstname, @surname, @lastname, @phone, LAST_INSERT_ID()) ";
                    command.Parameters.AddWithValue("@firstname", firstname);
                    command.Parameters.AddWithValue("@surname", surname);
                    command.Parameters.AddWithValue("@lastname", lastname);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Клиент успешно добавлен в базу данных!");
                EmployeesWindow employeesWindow = new EmployeesWindow(fullname, wor_root, workerId);
                employeesWindow.Show();
                Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Ошибка при добавлении клиента в базу данных: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        public void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeesWindow employeesWindow = new EmployeesWindow(fullname, wor_root, workerId);
            employeesWindow.Show();
            Close();
        }
    }

}

