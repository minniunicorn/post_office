using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Threading;


namespace post_office
{
    /// <summary>
    /// Логика взаимодействия для MoneySending.xaml
    /// </summary>
    public partial class MoneySending : Window
    {
        readonly DispatcherTimer timer = new DispatcherTimer();
        readonly string fullname = "";
        readonly string wor_root = "";
        readonly int workerId = 0;
        // Строка подключения к базе данных
        static readonly string connectionString = "Server=localhost;Database=post_office;Uid=root;Pwd=;";
        // Подключение к базе данных MySQL
        readonly MySqlConnection connection = new MySqlConnection(connectionString);

        public MoneySending(string name, string root, int id)
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            fio.Content = name;
            fullname = name;
            wor_root = root;
            workerId = id;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            time.Content = DateTime.Now.ToString("HH:mm");
        }

        private void Gotomain_Click(object sender, RoutedEventArgs e)
        {
            Operations operations = new Operations(fullname, wor_root, workerId);
            operations.Show();
            this.Close();
        }

        private void Sender_fio(int addressId) 
        {
            using (connection)
            {
                connection.Open();

                string clientsQuery = "INSERT INTO clients (surname, name, lastname, address_id) VALUES (@surname, @name, @lastname, @address_id);";
                MySqlCommand clientsCommand = new MySqlCommand(clientsQuery, connection);
                clientsCommand.Parameters.AddWithValue("@surname", surnameTextBox.Text);
                clientsCommand.Parameters.AddWithValue("@name", nameTextBox.Text);
                clientsCommand.Parameters.AddWithValue("@lastname", lastnameTextBox.Text);
                clientsCommand.Parameters.AddWithValue("@address_id", addressId);
                clientsCommand.ExecuteNonQuery();
            }
        }

        private void Sender_address()
        {
            using (connection)
            {
                connection.Open();
                string addressQuery = "INSERT INTO address (postal_index, town, street, house, appart) VALUES (@postal_index, @town, @street, @house, @appart);";
                MySqlCommand addressCommand = new MySqlCommand(addressQuery, connection);
                addressCommand.Parameters.AddWithValue("@postal_index", postalIndexTextBox.Text);
                addressCommand.Parameters.AddWithValue("@town", townTextBox.Text);
                addressCommand.Parameters.AddWithValue("@street", streetTextBox.Text);
                addressCommand.Parameters.AddWithValue("@house", houseTextBox.Text);
                addressCommand.Parameters.AddWithValue("@appart", appartTextBox.Text);
                addressCommand.ExecuteNonQuery();
            }
        }

        private int GetAddressId()
        {
            using (connection)
            {
                connection.Open();

                string getAddressIdQuery = "SELECT id FROM address WHERE postal_index = @postal_index AND town = @town AND street = @street AND house = @house AND appart = @appart;";
                MySqlCommand getAddressIdCommand = new MySqlCommand(getAddressIdQuery, connection);
                getAddressIdCommand.Parameters.AddWithValue("@postal_index", postalIndexTextBox.Text);
                getAddressIdCommand.Parameters.AddWithValue("@town", townTextBox.Text);
                getAddressIdCommand.Parameters.AddWithValue("@street", streetTextBox.Text);
                getAddressIdCommand.Parameters.AddWithValue("@house", houseTextBox.Text);
                getAddressIdCommand.Parameters.AddWithValue("@appart", appartTextBox.Text);
                int addressId = Convert.ToInt32(getAddressIdCommand.ExecuteScalar());
                return addressId;
            }
        }


        private void Recipient_inf()
        {
            using (connection)
            {
                connection.Open();

                string query = "INSERT INTO recipient (surname, name, lastname, postal_index, town, street, house, appart) " +
                               "VALUES (@surname, @name, @lastname, @postal_index, @town, @street, @house, @appart);";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@surname", r_surnameTextBox.Text);
                command.Parameters.AddWithValue("@name", r_nameTextBox.Text);
                command.Parameters.AddWithValue("@lastname", r_lastnameTextBox.Text);
                command.Parameters.AddWithValue("@postal_index", r_postalIndexTextBox.Text);
                command.Parameters.AddWithValue("@town", r_townTextBox.Text);
                command.Parameters.AddWithValue("@street", r_streetTextBox.Text);
                command.Parameters.AddWithValue("@house", r_houseTextBox.Text);
                command.Parameters.AddWithValue("@appart", r_appartTextBox.Text);

                command.ExecuteNonQuery();
            }
        }
        private int GetRecId()
        {
            using (connection)
            {
                connection.Open();

                string getRecIdQuery = "SELECT id FROM recipient WHERE surname = @surname AND name = @name AND lastname = @lastname AND postal_index = @postal_index AND town = @town AND street = @street AND house = @house AND appart = @appart;";
                MySqlCommand getRecIdCommand = new MySqlCommand(getRecIdQuery, connection);
                getRecIdCommand.Parameters.AddWithValue("@surname", r_surnameTextBox.Text);
                getRecIdCommand.Parameters.AddWithValue("@name", r_nameTextBox.Text);
                getRecIdCommand.Parameters.AddWithValue("@lastname", r_lastnameTextBox.Text);
                getRecIdCommand.Parameters.AddWithValue("@postal_index", r_postalIndexTextBox.Text);
                getRecIdCommand.Parameters.AddWithValue("@town", r_townTextBox.Text);
                getRecIdCommand.Parameters.AddWithValue("@street", r_streetTextBox.Text);
                getRecIdCommand.Parameters.AddWithValue("@house", r_houseTextBox.Text);
                getRecIdCommand.Parameters.AddWithValue("@appart", r_appartTextBox.Text);
                int recId = Convert.ToInt32(getRecIdCommand.ExecuteScalar());
                return recId;
            }
        }

        private int GetSendId(int addressId)
        {
            using (connection)
            {
                connection.Open();

                string getSendIdQuery = "SELECT id FROM clients WHERE surname = @surname AND name = @name AND lastname = @lastname AND address_id = @address_id;";
                MySqlCommand getSensIdCommand = new MySqlCommand(getSendIdQuery, connection);
                getSensIdCommand.Parameters.AddWithValue("@surname", surnameTextBox.Text);
                getSensIdCommand.Parameters.AddWithValue("@name", nameTextBox.Text);
                getSensIdCommand.Parameters.AddWithValue("@lastname", lastnameTextBox.Text);
                getSensIdCommand.Parameters.AddWithValue("@address_id", addressId);
                int sendId = Convert.ToInt32(getSensIdCommand.ExecuteScalar());
                return sendId;
            }
        }

        private void Money_sum(int sendId, int recId)
        {
            using (connection)
            {
                connection.Open();

                string query = "INSERT INTO remittance (money_sum, recipient_id, sender_id) " +
                               "VALUES (@money_sum, @recipient_id, @sender_id);";

                MySqlCommand command = new MySqlCommand(query, connection);
                decimal moneySum = Convert.ToDecimal(sumTextBox.Text);
                command.Parameters.AddWithValue("@money_sum", moneySum);
                command.Parameters.AddWithValue("@recipient_id", recId);
                command.Parameters.AddWithValue("@sender_id", sendId);

                command.ExecuteNonQuery();
            }
        }


        private int GetMoneyId(int sendId, int recId)
        {
            using (connection)
            {
                connection.Open();

                string getMoneyIdQuery = "SELECT id FROM remittance WHERE money_sum = @money_sum AND recipient_id = @recipient_id AND sender_id = @sender_id;";
                MySqlCommand command = new MySqlCommand(getMoneyIdQuery, connection);
                command.Parameters.AddWithValue("@money_sum", sumTextBox.Text);
                command.Parameters.AddWithValue("@recipient_id", recId);
                command.Parameters.AddWithValue("@sender_id", sendId);
                int moneyId = Convert.ToInt32(command.ExecuteScalar());

                return moneyId;
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Sender_address();
                int addressId=GetAddressId();
                Sender_fio(addressId);
                int sendId = GetSendId(addressId);
                Recipient_inf();
                int recId = GetRecId();
                Money_sum(sendId, recId);
                int moneyId = GetMoneyId(sendId, recId);
                MoneyOK moneyOK = new MoneyOK(fullname, wor_root, workerId, moneyId);
                moneyOK.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении операции: " + ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }
        private void Sender_inf_Click(object sender, RoutedEventArgs e)
        {
            send.Visibility = Visibility.Visible;
            recip.Visibility = Visibility.Collapsed;
            money.Visibility = Visibility.Collapsed;
        }

        private void Recip_inf_Click(object sender, RoutedEventArgs e)
        {
            send.Visibility = Visibility.Collapsed;
            recip.Visibility = Visibility.Visible;
            money.Visibility = Visibility.Collapsed;
        }

        private void Money_inf_Click(object sender, RoutedEventArgs e)
        {
            send.Visibility = Visibility.Collapsed;
            recip.Visibility = Visibility.Collapsed;
            money.Visibility = Visibility.Visible;

        }

        
    }
}
