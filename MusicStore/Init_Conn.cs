using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace MusicStore
{
    class Init_Conn
    {
        private readonly SqlConnection conn = null;
        public static void InitializeConnection()
        {
            try
            {
                var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MusicConn"].ConnectionString);
                conn.StateChange += Connection_StateChange;
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show($"Ошибка конфигурации: {ex.Message}", "Ошибка",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
        private static void Connection_StateChange(object sender, StateChangeEventArgs e)
        {
            Debug.WriteLine($"Состояние соединения изменилось на: {e.CurrentState}");
        }
    }
}
