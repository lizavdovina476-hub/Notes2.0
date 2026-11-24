using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace NotesApp
{
    internal class DatabaseHelper
    {
        private static string connectionString = "Data Source=notes.db;Version=3;";
        public static void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createUsersTable = @"CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PinCode TEXT NOT NULL UNIQUE);";
                string createNotesTable = @"CREATE TABLE IF NOT EXISTS Notes (Id INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId INTEGER NOT NULL, Title TEXT NOT NULL, Content TEXT, IsFavorite INTEGER DEFAULT 0,
                CreatedAt TEXT, UpdatedAt TEXT);";
                using (var command = new SQLiteCommand(createUsersTable, connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SQLiteCommand(createNotesTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
            public static bool RegisterUsers(string pinCode) {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE PinCode = @pin";
                using (var checkCmd = new SQLiteCommand(checkQuery, connection))
                {
                    checkCmd.Parameters.AddWithValue("@pin", pinCode);
                    long count = (long)checkCmd.ExecuteScalar();
                    if (count > 0) return false;
                }
                string insertQuery = "INSERT INTO Users (PinCode) VALUES (@pin)";
                using (var insertCmd = new SQLiteCommand(insertQuery, connection))
                {
                    insertCmd.Parameters.AddWithValue("@pin", pinCode);
                    insertCmd.ExecuteNonQuery();
                    return true;
                }
            }
        }
        public static int LoginUsers(string pinCode) {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id FROM Users WHERE PinCode = @pin";
                using (var command = new SQLiteCommand(query, connection)) {
                    command.Parameters.AddWithValue("@pin", pinCode);
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }

            return -1;

        }
    }
}
