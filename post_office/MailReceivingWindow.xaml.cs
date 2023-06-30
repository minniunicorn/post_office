using MySql.Data.MySqlClient;
using System;
using System.IO.Packaging;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace post_office
{
    /// <summary>
    /// Логика взаимодействия для MailReceivingWindow.xaml
    /// </summary>
    public partial class MailReceivingWindow : Window
    {
        readonly DispatcherTimer timer = new DispatcherTimer();
        // Строка подключения к базе данных
        static readonly string connectionString = "Server=localhost;Database=post_office;Uid=root;Pwd=;";
        // Подключение к базе данных MySQL
        readonly MySqlConnection connection = new MySqlConnection(connectionString);
        readonly string fullname = "";
        readonly string wor_root = "";
        readonly int workerId = 0;

        public MailReceivingWindow(string name, string root, int id)
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

        // Обновление времени на экране каждую секунду
        private void Timer_Tick(object sender, EventArgs e)
        {
            time.Content = DateTime.Now.ToString("HH:mm");
        }

        // Переход на главное окно
        private void Gotomain_Click(object sender, RoutedEventArgs e)
        {
            Operations operations = new Operations(fullname, wor_root, workerId);
            operations.Show();
            this.Close();
        }

        // Показать форму для добавления типа посылки
        private void Rankbtn_Click(object sender, RoutedEventArgs e)
        {
            rank.Visibility = Visibility.Visible;
            infpack.Visibility = Visibility.Collapsed;
            infclient.Visibility = Visibility.Collapsed;
            invoice.Visibility = Visibility.Collapsed;
        }

        // Показать форму для информации о посылке
        private void Infpackbtn_Click(object sender, RoutedEventArgs e)
        {
            rank.Visibility = Visibility.Collapsed;
            infpack.Visibility = Visibility.Visible;
            infclient.Visibility = Visibility.Collapsed;
            invoice.Visibility = Visibility.Collapsed;
        }

        // Показать форму для информации о клиенте
        private void Infclientbtn_Click(object sender, RoutedEventArgs e)
        {
            rank.Visibility = Visibility.Collapsed;
            infpack.Visibility = Visibility.Collapsed;
            infclient.Visibility = Visibility.Visible;
            invoice.Visibility = Visibility.Collapsed;
        }

        // Показать форму для создания накладной
        private void Invoice_Click(object sender, RoutedEventArgs e)
        {
            rank.Visibility = Visibility.Collapsed;
            infpack.Visibility = Visibility.Collapsed;
            infclient.Visibility = Visibility.Collapsed;
            invoice.Visibility = Visibility.Visible;
        }

        // Добавление адреса в таблицу "address"
        private void AddAddress()
        {
            /*using (connection)
            {
                

                string addressQuery = "INSERT INTO address (postal_index, town, street, house, appart) VALUES (@postal_index, @town, @street, @house, @appart);";
                MySqlCommand addressCommand = new MySqlCommand(addressQuery, connection);
                addressCommand.Parameters.AddWithValue("@postal_index", postalIndexTextBox.Text);
                addressCommand.Parameters.AddWithValue("@town", townTextBox.Text);
                addressCommand.Parameters.AddWithValue("@street", streetTextBox.Text);
                addressCommand.Parameters.AddWithValue("@house", houseTextBox.Text);
                addressCommand.Parameters.AddWithValue("@appart", appartTextBox.Text);
                addressCommand.ExecuteNonQuery();
            }*/
            try
            {
                connection.Open();

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
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении операции: " + ex.ToString());
            }
            finally
            {
                connection.Close();
            }

        }

        // Получение ID адреса из таблицы "address"
        private int GetAddressId()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
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

        // Добавление клиента в таблицу "clients"
        private void AddClient(int addressId)
        {
            try
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM clients WHERE surname = @surname AND name = @name AND lastname = @lastname AND address_id = @address_id;";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@surname", surnameTextBox.Text);
                checkCommand.Parameters.AddWithValue("@name", nameTextBox.Text);
                checkCommand.Parameters.AddWithValue("@lastname", lastnameTextBox.Text);
                checkCommand.Parameters.AddWithValue("@address_id", addressId);
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
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении операции: " + ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        // Получение ID клиента из таблицы "clients"
        private int GetClientId(int addressId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string getClientsIdQuery = "SELECT id FROM clients WHERE surname = @surname AND name = @name AND lastname = @lastname AND address_id = @address_id;";
                MySqlCommand getClientsIdCommand = new MySqlCommand(getClientsIdQuery, connection);
                getClientsIdCommand.Parameters.AddWithValue("@surname", surnameTextBox.Text);
                getClientsIdCommand.Parameters.AddWithValue("@name", nameTextBox.Text);
                getClientsIdCommand.Parameters.AddWithValue("@lastname", lastnameTextBox.Text);
                getClientsIdCommand.Parameters.AddWithValue("@address_id", addressId);
                int clientsId = Convert.ToInt32(getClientsIdCommand.ExecuteScalar());
                return clientsId;
            }
        }

        // Добавление места хранения в таблицу "storage"
        private void AddStorage()
        {
            try
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM storage WHERE number = @number;";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@number", storageTextBox.Text);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    return; // Прерывание выполнения метода, чтобы клиент не был добавлен повторно
                }

                string storageQuery = "INSERT INTO storage (number) VALUES (@number);";
                MySqlCommand storageCommand = new MySqlCommand(storageQuery, connection);
                storageCommand.Parameters.AddWithValue("@number", storageTextBox.Text);
                storageCommand.ExecuteNonQuery();
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

        // Получение ID места хранения из таблицы "storage"
        private int GetStorageId()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string getStorageIdQuery = "SELECT id FROM storage WHERE number = @number;";
                MySqlCommand getStorageIdCommand = new MySqlCommand(getStorageIdQuery, connection);
                getStorageIdCommand.Parameters.AddWithValue("@number", storageTextBox.Text);
                int storageId = Convert.ToInt32(getStorageIdCommand.ExecuteScalar());
                return storageId;
            }
        }

        // Добавление накладной в таблицу "invoice"
        private void AddInvoice()
        {
            try
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM invoice WHERE id = @id AND workers_id = @workers_id;";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@id", num_inv.Text);
                checkCommand.Parameters.AddWithValue("@workers_id", workerId);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    return; // Прерывание выполнения метода, чтобы клиент не был добавлен повторно
                }

                string invoiceQuery = "INSERT INTO invoice (id, date, workers_id) VALUES (@id, @date, @workers_id);";
                MySqlCommand invoiceCommand = new MySqlCommand(invoiceQuery, connection);
                invoiceCommand.Parameters.AddWithValue("@id", num_inv.Text);
                invoiceCommand.Parameters.AddWithValue("@date", DateTime.Now);
                invoiceCommand.Parameters.AddWithValue("@workers_id", workerId);
                invoiceCommand.ExecuteNonQuery();
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

        // Добавление посылки в таблицу "package"
        private void AddPackage(int clientsId, int storageId)
        {
            try
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM package WHERE weight = @weight AND type = @type AND pac_rank = @pac_rank AND clients_id = @clients_id;";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@weight", weightTextBox.Text);
                string selectedType = ((ComboBoxItem)typeComboBox.SelectedItem).Content.ToString();
                checkCommand.Parameters.AddWithValue("@type", selectedType);
                string selectedRank = ((ComboBoxItem)rankComboBox.SelectedItem).Content.ToString();
                checkCommand.Parameters.AddWithValue("@pac_rank", selectedRank);
                checkCommand.Parameters.AddWithValue("@clients_id", clientsId);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    int packageId = GetPackageId(clientsId);
                    string updateQuery = "UPDATE package SET status='Ожидает вручения' WHERE id=@pack_id";
                    MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@pack_id", packageId);
                    MessageBox.Show("Статус существующей посылки был изменен, ID:" + packageId);
                    updateCommand.ExecuteNonQuery();
                    OpenWindow(clientsId, packageId);
                    return; // Прерывание выполнения метода, чтобы клиент не был добавлен повторно
                }

                string packageQuery = "INSERT INTO package (weight, status, type, pac_rank, storage_id, invoice_id, clients_id) VALUES (@weight, @status, @type, @pac_rank, @storage_id, @invoice_id, @clients_id);";
                MySqlCommand packageCommand = new MySqlCommand(packageQuery, connection);
                packageCommand.Parameters.AddWithValue("@weight", weightTextBox.Text);
                packageCommand.Parameters.AddWithValue("@status", "Ожидает вручения");
                selectedType = ((ComboBoxItem)typeComboBox.SelectedItem).Content.ToString();
                packageCommand.Parameters.AddWithValue("@type", selectedType);
                selectedRank = ((ComboBoxItem)rankComboBox.SelectedItem).Content.ToString();
                packageCommand.Parameters.AddWithValue("@pac_rank", selectedRank);
                packageCommand.Parameters.AddWithValue("@storage_id", storageId);
                packageCommand.Parameters.AddWithValue("@invoice_id", num_inv.Text);
                packageCommand.Parameters.AddWithValue("@clients_id", clientsId);
                packageCommand.ExecuteNonQuery();
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

        // Получение ID посылки из таблицы "package"
        private int GetPackageId(int clientsId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Получение ID добавленной записи в таблице "package"
                string getPackageIdQuery = "SELECT id FROM package WHERE weight = @weight AND type = @type AND pac_rank = @pac_rank AND clients_id = @clients_id;"; 
                MySqlCommand getPackageIdCommand = new MySqlCommand(getPackageIdQuery, connection);
                getPackageIdCommand.Parameters.AddWithValue("@weight", weightTextBox.Text);
                string selectedType = ((ComboBoxItem)typeComboBox.SelectedItem).Content.ToString();
                getPackageIdCommand.Parameters.AddWithValue("@type", selectedType);
                string selectedRank = ((ComboBoxItem)rankComboBox.SelectedItem).Content.ToString();
                getPackageIdCommand.Parameters.AddWithValue("@pac_rank", selectedRank);
                getPackageIdCommand.Parameters.AddWithValue("@clients_id", clientsId);
                int packageId = Convert.ToInt32(getPackageIdCommand.ExecuteScalar());
                return packageId;
            }
        }
        // Обработчик кнопки "Next"
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Добавление адреса
                AddAddress();
                int addressId = GetAddressId();

                // Добавление клиента
                AddClient(addressId);
                int clientsId = GetClientId(addressId);

                // Добавление места хранения
                AddStorage();
                int storageId = GetStorageId();

                // Добавление накладной
                AddInvoice();

                // Добавление посылки
                AddPackage(clientsId, storageId);
                int packageId = GetPackageId(clientsId);

                // Открытие окна подтверждения
                // Проверка открытия окна MailOK
                if (!Application.Current.Windows.OfType<MailOK>().Any())
                {
                    OpenWindow(clientsId, packageId);
                }
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
        private void OpenWindow(int clientsId, int packageId)
        {
            MailOK mailOK = new MailOK(fullname, wor_root, workerId, packageId, clientsId);
            mailOK.Show();
            this.Close();
        }
    }
}