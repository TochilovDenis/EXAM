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
            return conn;
        }
    }
}