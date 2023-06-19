using MySql.Data.MySqlClient;
using System;
using System.Windows;

public static class Statics
{
    // Строка подключения к базе данных
    static readonly string connectionString = "Server=localhost;Database=post_office;Uid=root;Pwd=;";

    public static MySqlConnection GetConnection()
    {
        MySqlConnection connection = new MySqlConnection(connectionString);
        return connection;
    }

    public static void InsertStatistic(string action, int workerId)
    {
        using (MySqlConnection connection = GetConnection())
        {
            try
            {
                connection.Open();

                DateTime datetime = DateTime.Now;
                string insertQuery = "INSERT INTO statistic (datetime, action, workers_id) VALUES (@datetime, @action, @workers_id);";
                MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@datetime", datetime);
                insertCommand.Parameters.AddWithValue("@action", action);
                insertCommand.Parameters.AddWithValue("@workers_id", workerId);
                insertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
