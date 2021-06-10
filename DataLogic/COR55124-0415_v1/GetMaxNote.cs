using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;
using DataLogic.Properties;
using log4net;

namespace DataLogic
{
    public class GetMaxNote
    {
        // Define a static logger variable so that it references the
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string NoteId;
        public static string PatNameRet;
        public static string PatIdRet;
        public static string PatDateRet;
        public static string ObtFromRet;

        public static void ReturnMaxNote()
        {
            var preSql = Settings.Default.SqlGetId;
            var sql = preSql + "@PatName = '" + PatNameRet + "', @PatId = '" + PatIdRet + "', @Date = '" + PatDateRet + "', @ObtFrom = '" + ObtFromRet + "'";
            Log.Debug("SP: " + sql + " for ReturnMaxNote()");
            Log.Debug("Dbconn called and created for ReturnMaxNote()");
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconn"].ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NoteId = reader[0].ToString();
                        }
                    }
                    Log.Debug("Command executed and reader completed for ReturnAuditData()");
                }
                catch (SqlException ex)
                {
                    // Build custom message box for exception upload
                    var msg = ex.ToString();
                    var title = Resources.CustomMsgBxTitle;
                    MessageBoxManager.Ok = "Copy";
                    MessageBoxManager.Cancel = "Ignore";
                    MessageBoxManager.Register();
                    var result = MessageBox.Show(msg, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);
                    switch (result)
                    {
                        case DialogResult.OK:
                            Clipboard.SetText(msg);
                            break;
                        case DialogResult.Cancel:
                            break;
                    }
                    MessageBoxManager.Unregister();
                    Log.Error("Exception Thrown: " + msg + " - for Patient ID: " + PatIdRet + " while calling uspGet_MaxNoteId_Cor551240415V1");
                }
                finally
                {
                    Log.Info("Closing SQL Connection for GetMaxNote.ReturnMaxNote()");
                }
            }
        }
    }
}