using MySql.Data.MySqlClient;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Windows;

public static class Statistic
{
    // Строка подключения к базе данных
    static readonly string connectionString = "Server=localhost;Database=post_office;Uid=root;Pwd=;";

    // Метод для вставки записи в таблицу statistic
    public static void InsertStatistic(string action, int workerId)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                // Получаем текущую дату и время
                DateTime datetime = DateTime.Now;

                // Запрос на вставку записи в таблицу statistic
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