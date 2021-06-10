using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;
using DataLogic.Properties;
using log4net;

namespace DataLogic
{
    public class LoadSignedData
    {
        // Define a static logger variable so that it references the
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string NoteId;
        public static string ClientId;

        private static SqlParameter SetDbNullIfEmpty(string parmName, string parmValue)
        {
            return new SqlParameter(parmName, string.IsNullOrEmpty(parmValue) ? DBNull.Value : (object) parmValue);
        }

        public static void LoadSign()
        {
            Log.Debug("Dbconn called and created for LoadSign()");
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconn"].ConnectionString))

            using (var cmd = conn.CreateCommand())
            {
                var sql = Settings.Default.SqlSignNote;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;

                // set the params for add
                cmd.Parameters.Add(SetDbNullIfEmpty("@NoteId", NoteId));
                cmd.Parameters.Add(SetDbNullIfEmpty("@ClientId", ClientId));

                try
                {
                    conn.Open();

                    // Write a log for auditing
                    Log.Info("Established a SQL Connection (" + conn + ") and opened communication to database for note ID: " + NoteId + " and client ID: " + ClientId);
                    Log.Info("Inserting values into DataControl tables from LoadSignedData.LoadSign() for note collection");

                    // run it 
                    cmd.ExecuteNonQuery();
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
                    Log.Error("Exception Thrown: " + msg + " - for Note ID: " + NoteId);
                }
                finally
                {
                    Log.Info("Closing SQL Connection for LoadSignedData.LoadSign()");
                }
            }
        }
    }
}