﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Xml.Linq;


namespace post_office
{
    /// <summary>
    /// Логика взаимодействия для EmployeesWindow.xaml
    /// </summary>
    public partial class EmployeesWindow : Window
    {
        readonly string fullname = "";
        readonly string wor_root = "";
        readonly int workerId = 0;

        public EmployeesWindow(string name, string root, int id)
        {
            InitializeComponent();
            fullname = name;
            wor_root = root;
            workerId = id;
            LoadData();
        }
        
        static public string connectionString = "Server=localhost;Database=post_office;Uid=root;Pwd=;";
        public MySqlConnection connection = new MySqlConnection(connectionString);


        public void LoadData()
        {
            string query = "SELECT clients.*, CONCAT(address.postal_index, ', г. ', address.town, ', ул. ', address.street, ', д. ', address.house, ', кв. ', address.appart) AS full_address " +
                           "FROM clients " +
                           "JOIN address ON clients.address_id = address.id";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Add_emp add_Emp = new Add_emp(fullname, wor_root, workerId);
            add_Emp.Show();
            this.Close();
        }

        private void Gotomain_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1(fullname, wor_root, workerId);
            window1.Show();
            this.Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count > 0)
            {
                try
                {
                    connection.Open();
                    // Получить выбранную строку из datagrid
                    DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;

                    // Получить ID выбранной строки
                    int id = Convert.ToInt32(selectedRow["ID"]);

                    MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранную строку?",
                                                      "Подтверждение удаления", MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question);

                    // Если пользователь подтверждает удаление строки, то выполнить удаление
                    if (result == MessageBoxResult.Yes)
                    {
                        // Выполнить запрос на удаление строки из базы данных
                        string query = "DELETE FROM clients WHERE id = " + id;
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.ExecuteNonQuery();

                        // Обновить datagrid
                        LoadData();
                    }
                }
                catch (Exception ex) 
                { 
                    MessageBox.Show($"Ошибка при добавлении клиента в базу данных: {ex.Message}"); 
                }
                finally 
                {
                    connection.Close();
                }      
            }
        }

        

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;

            // Получить ID выбранной строки
            int id = Convert.ToInt32(selectedRow["ID"]);
            Edit_emp edit_Emp = new Edit_emp(id, fullname, wor_root, workerId);
            edit_Emp.Show();
            this.Close();
        }
    }
}

