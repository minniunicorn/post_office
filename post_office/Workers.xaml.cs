using MySql.Data.MySqlClient;
using post_office.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для Workers.xaml
    /// </summary>
    public partial class Workers : Window
    {
        static readonly string connectionString = "Server=localhost;Database=post_office;Uid=root;Pwd=;";
        readonly MySqlConnection connection = new MySqlConnection(connectionString);

        readonly string fullname = "";
        readonly string wor_root = "";
        readonly int workerId = 0;

        public Workers(string name, string root, int id)
        {
            InitializeComponent();
            fullname = name;
            wor_root = root;
            workerId = id;
            LoadEmployees();
        }
        private void OpenLogButton_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT statistic.id, statistic.datetime, statistic.action, CONCAT(workers.surname, ' ', workers.name, ' ', workers.lastname) AS worker_name " +
                           "FROM statistic " +
                           "JOIN workers ON statistic.workers_id = workers.id";

            try
            {
                using (connection)
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    logsDataGrid.ItemsSource = dataTable.DefaultView;
                    openLogButton.Content = "Работники";
                    openLogButton.Click -= OpenLogButton_Click;
                    openLogButton.Click += Emp_Click;
                    logsDataGrid.Visibility=Visibility.Visible;
                    employeeDataGrid.Visibility=Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                // Обработка исключения, если произошла ошибка
                MessageBox.Show(ex.Message);
            }
        }

        private void Emp_Click(object sender, RoutedEventArgs e)
        {
            LoadEmployees();
            openLogButton.Content = "Открыть журнал";
            openLogButton.Click += OpenLogButton_Click;
            openLogButton.Click -= Emp_Click;
            logsDataGrid.Visibility = Visibility.Hidden;
            employeeDataGrid.Visibility = Visibility.Visible;
        }

        private void Gotomain_Click(object sender, RoutedEventArgs e)
        {
            Operations operations = new Operations(fullname, wor_root, workerId);
            operations.Show();
            this.Close();
        }
        private void LoadEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (connection)
            {
                string query = "SELECT * FROM workers";

                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Employee employee = new Employee()
                        {
                            ID = reader.GetInt32("id"),
                            Surname = reader.GetString("surname"),
                            Name = reader.GetString("name"),
                            Lastname = reader.GetString("lastname"),
                            Position = reader.GetString("position"),
                            Phone = reader.GetString("phone"),
                            Login = reader.GetString("login"),
                            Password = reader.GetString("password"),
                            Root = reader.GetString("root")
                        };

                        employees.Add(employee);
                    }
                }
            }

            employeeDataGrid.ItemsSource = employees;
        }
    }
    
    

}