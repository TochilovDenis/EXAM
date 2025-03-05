using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace MusicStore
{
    public class Init_Conn
    {
        private static readonly string _connectionString;

        static Init_Conn()
        {
            try
            {
                _connectionString = ConfigurationManager.ConnectionStrings["MusicConn"].ConnectionString;
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show($"Ошибка конфигурации: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.StateChange += Connection_StateChange;
            Debug.WriteLine($"Соединение установлено: {CheckConn(conn)}");
            Debug.WriteLine($"Текущее состояние: {conn.State}");
            return conn;
        }

        private static void Connection_StateChange(object sender, StateChangeEventArgs e)
        {
            Debug.WriteLine($"Состояние соединения изменилось на: {e.CurrentState}");
        }

        public static bool CheckConn(SqlConnection conn)
        {
            if (conn == null || conn.State != ConnectionState.Open)
            {
                try
                {
                    if (conn != null && conn.State != ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Open();
                    }

                    using (var cmd = new SqlCommand("SELECT 1", conn))
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine($"Ошибка соединения: {ex.Message}");
                    return false;
                }
            }
            return true;
        }
    }
}