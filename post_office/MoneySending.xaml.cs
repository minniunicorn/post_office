﻿using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Input;
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
                // Проверка наличия клиента в базе данных
                string checkQuery = "SELECT COUNT(*) FROM clients WHERE name = @name AND surname = @surname AND lastname = @lastname;";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@name", nameTextBox.Text);
                checkCommand.Parameters.AddWithValue("@surname", surnameTextBox.Text);
                checkCommand.Parameters.AddWithValue("@lastname", lastnameTextBox.Text);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                if (count > 0)
                {
                    return; // Прерывание выполнения метода, чтобы клиент не был добавлен повторно
                }
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

                // Проверка наличия клиента в базе данных
                string checkQuery = "SELECT COUNT(*) FROM address WHERE postal_index = @postal_index AND town = @town AND street = @street AND house = @house AND appart = @appart;";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@postal_index", postalIndexTextBox.Text);
                checkCommand.Parameters.AddWithValue("@town", townTextBox.Text);
                checkCommand.Parameters.AddWithValue("@street", streetTextBox.Text);
                checkCommand.Parameters.AddWithValue("@house", houseTextBox.Text);
                checkCommand.Parameters.AddWithValue("@appart", appartTextBox.Text);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                if (count > 0)
                {
                    return; // Прерывание выполнения метода, чтобы клиент не был добавлен повторно
                }

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

        private int GetSendAddressId()
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

        private void Recipient_fio(int recaddressId)
        {
            using (connection)
            {
                connection.Open();
                // Проверка наличия клиента в базе данных
                string checkQuery = "SELECT COUNT(*) FROM clients WHERE name = @name AND surname = @surname AND lastname = @lastname;";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@surname", r_surnameTextBox.Text);
                checkCommand.Parameters.AddWithValue("@name", r_nameTextBox.Text);
                checkCommand.Parameters.AddWithValue("@lastname", r_lastnameTextBox.Text);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                if (count > 0)
                {
                    return; // Прерывание выполнения метода, чтобы клиент не был добавлен повторно
                }
                string clientsQuery = "INSERT INTO clients (surname, name, lastname, address_id) VALUES (@surname, @name, @lastname, @address_id);";
                MySqlCommand clientsCommand = new MySqlCommand(clientsQuery, connection);
                clientsCommand.Parameters.AddWithValue("@surname", r_surnameTextBox.Text);
                clientsCommand.Parameters.AddWithValue("@name", r_nameTextBox.Text);
                clientsCommand.Parameters.AddWithValue("@lastname", r_lastnameTextBox.Text);
                clientsCommand.Parameters.AddWithValue("@address_id", recaddressId);
                clientsCommand.ExecuteNonQuery();
            }
        }

        private void Recipient_address()
        {
            using (connection)
            {
                connection.Open();

                // Проверка наличия клиента в базе данных
                string checkQuery = "SELECT COUNT(*) FROM address WHERE postal_index = @postal_index AND town = @town AND street = @street AND house = @house AND appart = @appart;";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@postal_index", r_postalIndexTextBox.Text);
                checkCommand.Parameters.AddWithValue("@town", r_townTextBox.Text);
                checkCommand.Parameters.AddWithValue("@street", r_streetTextBox.Text);
                checkCommand.Parameters.AddWithValue("@house", r_houseTextBox.Text);
                checkCommand.Parameters.AddWithValue("@appart", r_appartTextBox.Text);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                if (count > 0)
                {
                    return; // Прерывание выполнения метода, чтобы клиент не был добавлен повторно
                }

                string addressQuery = "INSERT INTO address (postal_index, town, street, house, appart) VALUES (@postal_index, @town, @street, @house, @appart);";
                MySqlCommand addressCommand = new MySqlCommand(addressQuery, connection);
                addressCommand.Parameters.AddWithValue("@postal_index", r_postalIndexTextBox.Text);
                addressCommand.Parameters.AddWithValue("@town", r_townTextBox.Text);
                addressCommand.Parameters.AddWithValue("@street", r_streetTextBox.Text);
                addressCommand.Parameters.AddWithValue("@house", r_houseTextBox.Text);
                addressCommand.Parameters.AddWithValue("@appart", r_appartTextBox.Text);
                addressCommand.ExecuteNonQuery();
            }
        }

        private int GetRecAddressId()
        {
            using (connection)
            {
                connection.Open();

                string getAddressIdQuery = "SELECT id FROM address WHERE postal_index = @postal_index AND town = @town AND street = @street AND house = @house AND appart = @appart;";
                MySqlCommand getAddressIdCommand = new MySqlCommand(getAddressIdQuery, connection);
                getAddressIdCommand.Parameters.AddWithValue("@postal_index", r_postalIndexTextBox.Text);
                getAddressIdCommand.Parameters.AddWithValue("@town", r_townTextBox.Text);
                getAddressIdCommand.Parameters.AddWithValue("@street", r_streetTextBox.Text);
                getAddressIdCommand.Parameters.AddWithValue("@house", r_houseTextBox.Text);
                getAddressIdCommand.Parameters.AddWithValue("@appart", r_appartTextBox.Text);
                int addressId = Convert.ToInt32(getAddressIdCommand.ExecuteScalar());
                return addressId;
            }
        }
        private int GetRecId(int addressId)
        {
            using (connection)
            {
                connection.Open();

                string getRecIdQuery = "SELECT id FROM clients WHERE surname = @surname AND name = @name AND lastname = @lastname AND address_id = @address_id;;";
                MySqlCommand getRecIdCommand = new MySqlCommand(getRecIdQuery, connection);
                getRecIdCommand.Parameters.AddWithValue("@surname", r_surnameTextBox.Text);
                getRecIdCommand.Parameters.AddWithValue("@name", r_nameTextBox.Text);
                getRecIdCommand.Parameters.AddWithValue("@lastname", r_lastnameTextBox.Text);
                getRecIdCommand.Parameters.AddWithValue("@address_id", addressId);
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
                int sendaddressId=GetSendAddressId();
                Sender_fio(sendaddressId);
                int sendId = GetSendId(sendaddressId);


                Recipient_address();
                int recaddressId = GetRecAddressId();
                Recipient_fio(recaddressId);
                int recId = GetRecId(recaddressId);
                
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

        private void All_rem_Click(object sender, RoutedEventArgs e)
        {
            AllMoneySending allMoney = new AllMoneySending(fullname, wor_root, workerId);
            allMoney.Show();
            this.Close();
            Statistic.InsertStatistic("Просмотр всех переводов", workerId);
        }
    }
}
