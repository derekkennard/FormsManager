using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;
using DataLogic.Properties;
using log4net;

namespace DataLogic
{
    public class UpdateNote
    {
        //use this to update notes on save IF the note ID already exist. 
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string PatNameRet;
        public static string PatIdRet;
        public static string PatDateRet;
        public static string ObtFromRet;
        public static int SobWithExertion;
        public static int SobWithoutExertion;
        public static int Fatigue;
        public static int Pain;
        public static int FingerPress;
        public static string BreathingProblems;
        public static string SwollenAbdomen;
        public static string LossOfAppetite;
        public static string Cough;
        public static string Urination;
        public static string Confusion;
        public static string Depression;
        public static string Hr;
        public static string Bb;
        public static string VitalDate;
        public static string Rr;
        public static string Weight;
        public static string WeightType;
        public static string LabsObtained;
        public static string Creatinine;
        public static string Na;
        public static string LabDate;
        public static string Bun;
        public static string K;
        public static string MdVisit;
        public static string MdScheduled;
        public static string Rehospitalization;
        public static string LengthOfStay;
        public static string PrimaryReasonReHosp;
        public static string ErVisit;
        public static string NumOfErVisits;
        public static string PrimaryReasonEr;
        public static string HomeRnVisit;
        public static string DoseChange;
        public static string IncreaseDecrease;
        public static string MedChange;
        public static string MedChangeComment;
        public static string Comments;
        public static string DoseChgComment;

        public static void BuildUpdateStmt()
        {
            Log.Debug("Dbconn called and created for BuildUpdateStmt()");
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconn"].ConnectionString))

            using (var cmd = conn.CreateCommand())
            {
                var sql = Settings.Default.SqlUpdateForm;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;

                //set params for sp
                cmd.Parameters.Add("@PatName", SqlDbType.VarChar).Value = PatNameRet;
                cmd.Parameters.Add("@PatId", SqlDbType.VarChar).Value = PatIdRet;
                cmd.Parameters.Add("@Date", SqlDbType.VarChar).Value = PatDateRet;
                cmd.Parameters.Add("@ObtFrom", SqlDbType.VarChar).Value = ObtFromRet;
                cmd.Parameters.Add("@SobWithExertion", SqlDbType.SmallInt).Value = SobWithExertion;
                cmd.Parameters.Add("@SobWithoutExertion", SqlDbType.SmallInt).Value = SobWithoutExertion;
                cmd.Parameters.Add("@Fatigue", SqlDbType.SmallInt).Value = Fatigue;
                cmd.Parameters.Add("@Pain", SqlDbType.SmallInt).Value = Pain;
                cmd.Parameters.Add("@Swelling", SqlDbType.SmallInt).Value = FingerPress;
                cmd.Parameters.Add("@BreathingProblems", SqlDbType.VarChar).Value = BreathingProblems;
                cmd.Parameters.Add("@SwollenAbdomen", SqlDbType.VarChar).Value = SwollenAbdomen;
                cmd.Parameters.Add("@LossOfAppetite", SqlDbType.VarChar).Value = LossOfAppetite;
                cmd.Parameters.Add("@Cough", SqlDbType.VarChar).Value = Cough;
                cmd.Parameters.Add("@Urination", SqlDbType.VarChar).Value = Urination;
                cmd.Parameters.Add("@Confusion", SqlDbType.VarChar).Value = Confusion;
                cmd.Parameters.Add("@Depression", SqlDbType.VarChar).Value = Depression;
                cmd.Parameters.Add("@Hr", SqlDbType.VarChar).Value = Hr;
                cmd.Parameters.Add("@Bb", SqlDbType.VarChar).Value = Bb;
                cmd.Parameters.Add("@VitalDate", SqlDbType.DateTime).Value = VitalDate;
                cmd.Parameters.Add("@Rr", SqlDbType.VarChar).Value = Rr;
                cmd.Parameters.Add("@Weight", SqlDbType.VarChar).Value = Weight;
                cmd.Parameters.Add("@WeightType", SqlDbType.VarChar).Value = WeightType;
                cmd.Parameters.Add("@LabsObtained", SqlDbType.Char).Value = LabsObtained;
                cmd.Parameters.Add("@Creatinine", SqlDbType.VarChar).Value = Creatinine;
                cmd.Parameters.Add("@Na", SqlDbType.VarChar).Value = Na;
                cmd.Parameters.Add("@LabDate", SqlDbType.DateTime).Value = LabDate;
                cmd.Parameters.Add("@Bun", SqlDbType.VarChar).Value = Bun;
                cmd.Parameters.Add("@K", SqlDbType.VarChar).Value = K;
                cmd.Parameters.Add("@MdVisit", SqlDbType.Char).Value = MdVisit;
                cmd.Parameters.Add("@MdScheduled", SqlDbType.VarChar).Value = MdScheduled;
                cmd.Parameters.Add("@Rehospitalization", SqlDbType.Char).Value = Rehospitalization;
                cmd.Parameters.Add("@LengthOfStay", SqlDbType.VarChar).Value = LengthOfStay;
                cmd.Parameters.Add("@PrimaryReasonReHosp", SqlDbType.VarChar).Value = PrimaryReasonReHosp;
                cmd.Parameters.Add("@ErVisit", SqlDbType.VarChar).Value = ErVisit;
                cmd.Parameters.Add("@NumOfErVisits", SqlDbType.VarChar).Value = NumOfErVisits;
                cmd.Parameters.Add("@PrimaryReasonEr", SqlDbType.VarChar).Value = PrimaryReasonEr;
                cmd.Parameters.Add("@HomeRnVisit", SqlDbType.VarChar).Value = HomeRnVisit;
                cmd.Parameters.Add("@DoseChange", SqlDbType.Char).Value = DoseChange;
                cmd.Parameters.Add("@IncreaseDecrease", SqlDbType.VarChar).Value = IncreaseDecrease;
                cmd.Parameters.Add("@MedChange", SqlDbType.VarChar).Value = MedChange;
                cmd.Parameters.Add("@MedChangeComment", SqlDbType.VarChar).Value = MedChangeComment;
                cmd.Parameters.Add("@Comments", SqlDbType.VarChar).Value = Comments;
                cmd.Parameters.Add("@DoseChgComment", SqlDbType.VarChar).Value = DoseChgComment;
                try
                {
                    conn.Open();

                    // Write a log for auditing
                    Log.Info("Established a SQL Connection (" + conn + ") and opened communication to database for Patient ID: " + PatIdRet);
                    Log.Info("Inserting values into DataControl tables from UpdateNote.UpdateDatabase() for note updates");

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
                    Log.Error("Exception Thrown: " + msg + " - for Note ID: " + PatIdRet);
                }
                finally
                {
                    Log.Info("Closing SQL Connection for UpdateNote.UpdateDatabase()");
                }
            }
        }
    }
}