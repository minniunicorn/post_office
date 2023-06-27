using MySql.Data.MySqlClient;
using System;
using System.Windows;

namespace post_office
{
    public partial class MainWindow : Window
    {
        // Строка подключения к базе данных
        static readonly string connectionString = "Server=localhost;Database=post_office;Uid=root;Pwd=;";
        // Подключение к базе данных MySQL
        readonly MySqlConnection selectConnection = new MySqlConnection(connectionString);

        public MainWindow()
        {
            InitializeComponent();
        }

        int workerId = 0;

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text; // Получаем введенное имя пользователя
            string password = PasswordBox.Password; // Получаем введенный пароль

            try
            {
                selectConnection.Open(); // Открываем соединение для выборки данных

                // Подготавливаем SQL-запрос для выборки данных о работнике по указанному логину и паролю
                string selectQuery = "SELECT CONCAT(surname, ' ', name, ' ', lastname) AS fullname, root, id FROM workers WHERE login=@username AND password=@password;";
                MySqlCommand selectCommand = new MySqlCommand(selectQuery, selectConnection);
                selectCommand.Parameters.AddWithValue("@username", username);
                selectCommand.Parameters.AddWithValue("@password", password);

                string fullName = null;
                string root = null;

                using (MySqlDataReader reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Если запрос вернул результат, извлекаем данные о фамилии, имени и отчестве работника
                        fullName = reader.GetString("fullname");
                        root = reader.GetString("root");
                        workerId = reader.GetInt32("id");
                        Statistic.InsertStatistic("Вход", workerId); // Добавляем данные в таблицу statistic
                        // Логин и пароль верны, открываем новое окно Window1 и передаем в него полное имя работника
                        Operations operations = new Operations(fullName, root, workerId);
                        operations.Show();
                        this.Close();
                    }
                    else
                    {
                        // Логин и/или пароль неверны, выводим сообщение об ошибке
                        ErrorLabel.Content = "Неверный логин и/или пароль";
                    }
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
            }
            finally
            {
                selectConnection.Close();
            }
        }
        //Обработчик кнопки закрытия программы
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
