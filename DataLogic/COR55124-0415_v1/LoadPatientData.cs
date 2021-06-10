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
    public static class LoadPatientData
    {
        // Define a static logger variable so that it references the
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string PatName;
        public static string PatId;
        public static string Date;
        public static string ObtFrom;

        private static SqlParameter SetDbNullIfEmpty(string parmName, string parmValue)
        {
            return new SqlParameter(parmName, string.IsNullOrEmpty(parmValue) ? DBNull.Value : (object) parmValue);
        }

        public static void LoadNote()
        {
            Log.Debug("Dbconn called and created for LoadNote()");
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconn"].ConnectionString))

            using (var cmd = conn.CreateCommand())
            {
                var preQuery = Settings.Default.SqlLoadNote;
                cmd.CommandText = preQuery;
                cmd.CommandType = CommandType.StoredProcedure;

                // set the params for add
                cmd.Parameters.Add(SetDbNullIfEmpty("@PatName", PatName));
                cmd.Parameters.Add(SetDbNullIfEmpty("@PatId", PatId));
                cmd.Parameters.Add(SetDbNullIfEmpty("@Date", Date));
                cmd.Parameters.Add(SetDbNullIfEmpty("@ObtFrom", ObtFrom));
                try
                {
                    conn.Open();

                    // Write a log for auditing
                    Log.Info("Established a SQL Connection (" + conn + ") and opened communication to database for patient: " + PatId);
                    Log.Info("Inserting values into DataControl tables from NoteDataInsert.LoadNote() for note collection");

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
                    Log.Error("Exception Thrown: " + msg + " - for Patient ID: " + PatId);
                }
                finally
                {
                    Log.Info("Closing SQL Connection for LoadPatientData.LoadNote()");
                }
            }
        }
    }
}