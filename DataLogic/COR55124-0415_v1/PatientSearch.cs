using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;
using DataLogic.Properties;
using log4net;

namespace DataLogic
{
    public class PatientSearch
    {
        // Define a static logger variable so that it references the
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string PatId;
        public static string PatName;
        public static string PatSys;
        //public static string PatAge;
        //public static string PatWgt;
        //private readonly DataTable _dt = new DataTable();
        public static void GetPatient()
        {
            var preQuery = Settings.Default.SqlGetPatient;
            var queryString = preQuery + "@PatId = '" + PatId + "'";
            Log.Debug("Inotropic Therapy Note Lookup as: " + queryString);
            Log.Debug("Dbconn called and created for GetPatient()");
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconn"].ConnectionString))

            using (var cmd = new SqlCommand(queryString, conn))
            {
                try
                {
                    conn.Open();
                    Log.Info("Established a SQL Connection (" + conn + ") and opened communication to database for patient: " + PatId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PatName = reader[0] as string;
                            PatId = reader[1] as string;
                            PatSys = reader[2] as string;
                            //PatAge = reader[3] as string;
                            //PatWgt = reader[4] as string;
                        }
                        Log.Debug("Command executed and reader completed for GetPatient()");
                    }
                    switch (PatName)
                    {
                        case null:
                            var msg = Resources.NullPatNameReturn;
                            var title = Resources.NullPatNameTitle;
                            var result = MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            switch (result)
                            {
                                case DialogResult.OK:
                                    PatId = null;
                                    PatName = null;
                                    PatSys = null;
                                    break;
                            }

                            break;
                    }
                    Log.Info("Passing values to Cor551240415V1: Patient ID: " + PatId + ", Patient Name: " + PatName + ", Patient SYS_ID: " + PatSys + " - END");
                }

                catch (SqlException ex)
                {
                    // Build custom message box for exception upload
                    var msg = ex.ToString();
                    var title = Resources.InotropeTitle;
                    MessageBoxManager.Ok = "Copy";
                    MessageBoxManager.Cancel = "Ignore";
                    MessageBoxManager.Register();
                    var result = MessageBox.Show(msg, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);
                    switch (result)
                    {
                        case DialogResult.OK:
                            Clipboard.SetText(msg);
                            // Process.Start("Web Address Here");
                            break;
                        case DialogResult.Cancel:
                            break;
                    }
                    MessageBoxManager.Unregister();
                    Log.Error("Exception Thrown: " + msg + " - for Patient ID: " + PatId);
                }
                finally
                {
                    Log.Info("Closing SQL Connection for PatientSearch.GetPatient()");
                }
            }
        }
    }
}