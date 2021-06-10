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
    public class RemoveSignature
    {
        //use this to update notes on save IF the note ID already exist. 
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string NoteId;

        private static SqlParameter SetDbNullIfEmpty(string parmName, string parmValue)
        {
            return new SqlParameter(parmName, string.IsNullOrEmpty(parmValue) ? DBNull.Value : (object) parmValue);
        }

        public static void DeleteSign()
        {
            Log.Debug("Dbconn called and created for DeleteSign()");
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconn"].ConnectionString))

            using (var cmd = conn.CreateCommand())
            {
                var sql = Settings.Default.SqlRemoveSign;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;

                // set the params for add
                cmd.Parameters.Add(SetDbNullIfEmpty("@p1", NoteId));
                try
                {
                    conn.Open();

                    // Write a log for auditing
                    Log.Info("Established a SQL Connection (" + conn + ") and opened communication to database for note ID: " + NoteId);
                    Log.Info("Removing values from DataControl table from RemoveSignature.DeleteSign() for note collection");

                    // run it 
                    cmd.ExecuteNonQuery();
                    Log.Debug("Executed Command as: " + sql);
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
                    Log.Info("Closing SQL Connection for RemoveSignature.DeleteSign()");
                }
            }
        }
    }
}