using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace post_office
{
    /// <summary>
    /// Логика взаимодействия для ShipmentsWindow.xaml
    /// </summary>
    public partial class ShipmentsWindow : Window
    {
        static readonly string connectionString = "Server=localhost;Database=post_office;Uid=root;Pwd=;";
        readonly MySqlConnection connection = new MySqlConnection(connectionString);
        readonly DataTable dataTable = new DataTable();
        readonly string fullname = "";
        readonly string wor_root = "";
        readonly int workerId = 0;

        public ShipmentsWindow(string name, string root, int id)
        {
            InitializeComponent();
            searchBox.TextChanged += SearchBox_TextChanged;
            LoadData();
            fullname = name;
            wor_root = root;

            workerId = id;
        }
        private void LoadData()
        {
            try
            {
                string query = "SELECT package.id, package.weight, package.status, package.type, package.pac_rank, package.invoice_id, " +
                "CONCAT(clients.surname, ' ', clients.name, ' ', clients.lastname) AS client_name, storage.number AS storage_number " +
                "FROM post_office.package " +
                "JOIN post_office.storage ON package.storage_id = storage.id " +
                "JOIN post_office.clients ON package.clients_id = clients.id " +
                "WHERE package.status IN ('В пути', 'Ожидает вручения')" + 
                "ORDER BY package.status DESC;";
                //id, surname, name, lastname, phone, adress_id 
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dataTable);
                dataGrid.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

            private void Button_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    DataRowView rowView = (DataRowView)dataGrid.SelectedItem;
                    if (rowView != null)
                    {
                        connection.Open();
                        int id = Convert.ToInt32(rowView["id"]);
                        string query = "UPDATE package SET status='Выдана' WHERE id=@id AND status='Ожидает вручения'";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                        connection.Close();
                        dataTable.Clear();
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Выберите посылку для выдачи.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        private void Gotomain_Click(object sender, RoutedEventArgs e)
        {
            Operations operations = new Operations(fullname, wor_root, workerId);
            operations.Show();
            this.Close();
        }
        private void Donepack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataTable.Clear();
                string query = "SELECT package.id, package.weight, package.status, package.type, package.pac_rank, package.invoice_id, " +
                "CONCAT(clients.surname, ' ', clients.name, ' ', clients.lastname) AS client_name, storage.number AS storage_number " +
                "FROM post_office.package " +
                "JOIN post_office.storage ON package.storage_id = storage.id " +
                "JOIN post_office.clients ON package.clients_id = clients.id " +
                "WHERE package.status IN ('Выдана')" +
                "ORDER BY package.status DESC;";
                //id, surname, name, lastname, phone, adress_id 
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dataTable);
                dataGrid.ItemsSource = dataTable.DefaultView;
                donepack.Content = "Все посылки";
                donepack.Click -= Donepack_Click;
                donepack.Click += Allpack_Click;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Allpack_Click(object sender, RoutedEventArgs e)
        {
            dataTable.Clear();
            LoadData();
            donepack.Content = "Выданные посылки";
            donepack.Click += Donepack_Click;
            donepack.Click -= Allpack_Click;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchBox.Text))
                {
                    dataTable.Clear();
                    LoadData();
                }
                else
                {
                    string searchQuery = "SELECT package.id, package.weight, package.status, package.type, package.pac_rank, package.invoice_id, " +
                                         "CONCAT(clients.surname, ' ', clients.name, ' ', clients.lastname) AS client_name, storage.number AS storage_number " +
                                         "FROM post_office.package " +
                                         "JOIN post_office.storage ON package.storage_id = storage.id " +
                                         "JOIN post_office.clients ON package.clients_id = clients.id " +
                                         "WHERE CONCAT(clients.surname, ' ', clients.name, ' ', clients.lastname) LIKE CONCAT('%', @searchText, '%') " +
                                         "OR package.id LIKE CONCAT('%', @searchText, '%');";

                    MySqlCommand command = new MySqlCommand(searchQuery, connection);
                    command.Parameters.AddWithValue("@searchText", searchBox.Text);

                    DataTable dataTable = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    adapter.Fill(dataTable);
                    dataGrid.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
