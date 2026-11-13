
using MySql.Data.MySqlClient;
using CyberSafeAwarenessBot2.Data;
using CyberSafeAwarenessBot2.Models;
using System;
using System.Collections.Generic;

namespace CyberSafeAwarenessBot2.Repositories
{
    public class TaskRepository
    {
        public void AddTask(CyberTask task)
        {
            using var connection = new MySqlConnection(DatabaseConfig.ConnectionString);
            connection.Open();

            string sql = @"INSERT INTO cyber_tasks
                (title, description, reminder_date)
                VALUES (@title, @description, @reminder)";

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@title", task.Title);
            command.Parameters.AddWithValue("@description", task.Description);
            command.Parameters.AddWithValue("@reminder",
                task.ReminderDate.HasValue
                    ? task.ReminderDate.Value
                    : DBNull.Value);

            command.ExecuteNonQuery();
        }

        public List<CyberTask> GetTasks()
        {
            var tasks = new List<CyberTask>();

            using var connection = new MySqlConnection(DatabaseConfig.ConnectionString);
            connection.Open();

            string sql = @"SELECT task_id, title, description,
                reminder_date, is_completed, created_at
                FROM cyber_tasks ORDER BY created_at DESC";

            using var command = new MySqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tasks.Add(new CyberTask
                {
                    TaskId = reader.GetInt32("task_id"),
                    Title = reader.GetString("title"),
                    Description = reader.GetString("description"),
                    ReminderDate = reader.IsDBNull(reader.GetOrdinal("reminder_date"))
                        ? null
                        : reader.GetDateTime("reminder_date"),
                    IsCompleted = reader.GetBoolean("is_completed"),
                    CreatedAt = reader.GetDateTime("created_at")
                });
            }

            return tasks;
        }

        public void CompleteTask(int taskId)
        {
            using var connection = new MySqlConnection(DatabaseConfig.ConnectionString);
            connection.Open();

            using var command = new MySqlCommand(
                "UPDATE cyber_tasks SET is_completed = TRUE WHERE task_id = @id",
                connection);

            command.Parameters.AddWithValue("@id", taskId);
            command.ExecuteNonQuery();
        }

        public void DeleteTask(int taskId)
        {
            using var connection = new MySqlConnection(DatabaseConfig.ConnectionString);
            connection.Open();

            using var command = new MySqlCommand(
                "DELETE FROM cyber_tasks WHERE task_id = @id",
                connection);

            command.Parameters.AddWithValue("@id", taskId);
            command.ExecuteNonQuery();
        }
    }
}
