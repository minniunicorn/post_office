using MySql.Data.MySqlClient;
using System;
using System.Windows;
using static Xceed.Wpf.Toolkit.Calculator;

namespace post_office
{
    public partial class MainWindow : Window
    {
        // Строка подключения к базе данных
        public static readonly string connectionString = "Server=localhost;Database=post_office;Uid=root;Pwd=;";
        // Подключение к базе данных MySQL
        public  MySqlConnection selectConnection = new MySqlConnection(connectionString);

        public MainWindow()
        {
            InitializeComponent();
        }

        public int workerId = 0;
        public string fullName = null;
        public string root = null;

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            string username = UsernameTextBox.Text; // Получаем введенное имя пользователя
            string password = PasswordBox.Password; // Получаем введенный пароль
            bool check = Login(username, password);
            if (check)
            {
                Statistic.InsertStatistic("Вход", workerId); // Добавляем данные в таблицу statistic
                Operations operations = new Operations(fullName, root, workerId);
                operations.Show();
                this.Close();
            }
        }

        public bool Login(string username, string password)
        {
            try
            {
                selectConnection.Open(); // Открываем соединение для выборки данных

                // Подготавливаем SQL-запрос для выборки данных о работнике по указанному логину и паролю
                string selectQuery = "SELECT CONCAT(surname, ' ', name, ' ', lastname) AS fullname, root, id FROM workers WHERE login=@username AND password=@password;";
                MySqlCommand selectCommand = new MySqlCommand(selectQuery, selectConnection);
                selectCommand.Parameters.AddWithValue("@username", username);
                selectCommand.Parameters.AddWithValue("@password", password);

                using (MySqlDataReader reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Если запрос вернул результат, извлекаем данные о фамилии, имени и отчестве работника
                        fullName = reader.GetString("fullname");
                        root = reader.GetString("root");
                        workerId = reader.GetInt32("id");
                        return true;
                    }
                    else
                    {
                        // Логин и/или пароль неверны, выводим сообщение об ошибке
                        ErrorLabel.Content = "Неверный логин и/или пароль";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
                return false;
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
