using MySql.Data.MySqlClient;
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
    /// Логика взаимодействия для AllMoneySending.xaml
    /// </summary>
    public partial class AllMoneySending : Window
    {
        static readonly string connectionString = "Server=localhost;Database=post_office;Uid=root;Pwd=;";
        readonly MySqlConnection connection = new MySqlConnection(connectionString);
        readonly DataTable dataTable = new DataTable();
        readonly string fullname = "";
        readonly string wor_root = "";
        readonly int workerId = 0;

        public AllMoneySending(string name, string root, int id)
        {
            InitializeComponent();
            LoadData();
            fullname = name;
            wor_root = root;
            workerId = id;
        }
        private void LoadData()
        {
            try
            {
                string query = "SELECT r.id AS remittance_id, r.total_sum, r.money_sum, " +
                               "CONCAT(s.surname, ' ', s.name, ' ', COALESCE(s.lastname, '')) AS sender_name, " +
                               "CONCAT(sa.postal_index, ', ', sa.town, ', ', sa.street, ', ', sa.house, COALESCE(CONCAT(', ', sa.appart), '')) AS sender_address, " +
                               "CONCAT(r2.surname, ' ', r2.name, ' ', COALESCE(r2.lastname, '')) AS recipient_name, " +
                               "CONCAT(ra.postal_index, ', ', ra.town, ', ', ra.street, ', ', ra.house, COALESCE(CONCAT(', ', ra.appart), '')) AS recipient_address " +
                               "FROM remittance r " +
                               "JOIN clients s ON r.sender_id = s.id " +
                               "JOIN clients r2 ON r.recipient_id = r2.id " +
                               "JOIN address sa ON s.address_id = sa.id " +
                               "JOIN address ra ON r2.address_id = ra.id ";
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

        
        private void Gotomain_Click(object sender, RoutedEventArgs e)
        {
            Operations operations = new Operations(fullname, wor_root, workerId);
            operations.Show();
            this.Close();
        }
        

        
    }
}
