namespace DataLogic
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Reflection;
    using System.Windows.Forms;
    using DataLogic.Properties;
    using log4net;

    public class LoadAuditData
    {
        // Define a logger variable so that it references the
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public string AppetiteRet;
        public string AuditedByRet;
        public DateTime? AuditedOnRet;
        public string BbRet;
        public string BreathingRet;
        public string BunRet;
        public string CommentsRet;
        public string ConfusionRet;
        public string CoughRet;
        public string CreatinineRet;
        public string DepressionRet;
        public string DoseChangeRet;
        public string DoseChgCommentRet;
        public string DoseIncDecRet;
        public string ErVisitNumOfVisitsRet;
        public string ErVisitRet;
        public string FatigueRet;
        public string FingerPressRet;
        public string HomeRnVisitRet;
        public string HrRet;
        public string KRet;
        public string LabObtRet;
        public string LengthOfStayRet;
        public string MdSchNotSchRet;
        public string MdVisitRet;
        public string MedProChgRet;
        public string MedProChgValRet;
        public string NaRet;
        public string NoteDateRet;
        public string NoteId;
        public string PainRet;
        public string PatId;
        public string PatNameRet;
        public string PrimeReasonErVisitRet;
        public string PrimeReasonRehospRet;
        public string RrRet;
        public string SignedBy;
        public string SobWithExtRet;
        public string SobWithoutExtRet;
        public string SwollenRet;
        public string UnschedHospRet;
        public string UrineRet;
        public string WeightRet;
        public string WeightTypeRet;

        public void BuildUpdateStmt()
        {
            _log.Debug("Dbconn called and created for BuildUpdateStmt()");
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconn"].ConnectionString))

            using (var cmd = conn.CreateCommand())
            {
                var sql = Settings.Default.SqlUpdateAuditData;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;

                //set params for sp
                cmd.Parameters.Add("@p1", SqlDbType.VarChar).Value = NoteId;
                cmd.Parameters.Add("@SobWithExertion", SqlDbType.SmallInt).Value = SobWithExtRet;
                cmd.Parameters.Add("@SobWithoutExertion", SqlDbType.SmallInt).Value = SobWithoutExtRet;
                cmd.Parameters.Add("@Fatigue", SqlDbType.SmallInt).Value = FatigueRet;
                cmd.Parameters.Add("@Pain", SqlDbType.SmallInt).Value = PainRet;
                cmd.Parameters.Add("@Swelling", SqlDbType.SmallInt).Value = FingerPressRet;
                cmd.Parameters.Add("@BreathingProblems", SqlDbType.VarChar).Value = BreathingRet;
                cmd.Parameters.Add("@SwollenAbdomen", SqlDbType.VarChar).Value = SwollenRet;
                cmd.Parameters.Add("@LossOfAppetite", SqlDbType.VarChar).Value = AppetiteRet;
                cmd.Parameters.Add("@Cough", SqlDbType.VarChar).Value = CoughRet;
                cmd.Parameters.Add("@Urination", SqlDbType.VarChar).Value = UrineRet;
                cmd.Parameters.Add("@Confusion", SqlDbType.VarChar).Value = ConfusionRet;
                cmd.Parameters.Add("@Depression", SqlDbType.VarChar).Value = DepressionRet;
                cmd.Parameters.Add("@Hr", SqlDbType.VarChar).Value = HrRet;
                cmd.Parameters.Add("@Bb", SqlDbType.VarChar).Value = BbRet;
                cmd.Parameters.Add("@Rr", SqlDbType.VarChar).Value = RrRet;
                cmd.Parameters.Add("@Weight", SqlDbType.VarChar).Value = WeightRet;
                cmd.Parameters.Add("@WeightType", SqlDbType.VarChar).Value = WeightTypeRet;
                cmd.Parameters.Add("@LabsObtained", SqlDbType.Char).Value = LabObtRet;
                cmd.Parameters.Add("@Creatinine", SqlDbType.VarChar).Value = CreatinineRet;
                cmd.Parameters.Add("@Na", SqlDbType.VarChar).Value = NaRet;
                cmd.Parameters.Add("@Bun", SqlDbType.VarChar).Value = BunRet;
                cmd.Parameters.Add("@K", SqlDbType.VarChar).Value = KRet;
                cmd.Parameters.Add("@MdVisit", SqlDbType.Char).Value = MdVisitRet;
                cmd.Parameters.Add("@MdScheduled", SqlDbType.VarChar).Value = MdSchNotSchRet;
                cmd.Parameters.Add("@Rehospitalization", SqlDbType.Char).Value = UnschedHospRet;
                cmd.Parameters.Add("@LengthOfStay", SqlDbType.VarChar).Value = LengthOfStayRet;
                cmd.Parameters.Add("@PrimaryReasonReHosp", SqlDbType.VarChar).Value = PrimeReasonRehospRet;
                cmd.Parameters.Add("@ErVisit", SqlDbType.VarChar).Value = ErVisitRet;
                cmd.Parameters.Add("@NumOfErVisits", SqlDbType.VarChar).Value = ErVisitNumOfVisitsRet;
                cmd.Parameters.Add("@PrimaryReasonEr", SqlDbType.VarChar).Value = PrimeReasonErVisitRet;
                cmd.Parameters.Add("@HomeRnVisit", SqlDbType.VarChar).Value = HomeRnVisitRet;
                cmd.Parameters.Add("@DoseChange", SqlDbType.Char).Value = DoseChangeRet;
                cmd.Parameters.Add("@IncreaseDecrease", SqlDbType.VarChar).Value = DoseIncDecRet;
                cmd.Parameters.Add("@MedChange", SqlDbType.VarChar).Value = MedProChgRet;
                cmd.Parameters.Add("@MedChangeComment", SqlDbType.VarChar).Value = MedProChgValRet;
                cmd.Parameters.Add("@Comments", SqlDbType.VarChar).Value = CommentsRet;
                cmd.Parameters.Add("@DoseChgComment", SqlDbType.VarChar).Value = DoseChgCommentRet;
                cmd.Parameters.Add("@SignedBy", SqlDbType.VarChar).Value = SignedBy;
                cmd.Parameters.Add("@AuditedBy", SqlDbType.VarChar).Value = AuditedByRet;
                cmd.Parameters.Add("@AuditedOn", SqlDbType.DateTime).Value = AuditedOnRet;

                try
                {
                    try
                    {
                        conn.Open();

                        // Write a log for auditing
                        _log.Info("Established a SQL Connection (" + conn + ") and opened communication to database for Note ID: " + NoteId);
                        _log.Info("Updating values into the tables from LoadAuditData.BuildUpdateStmt() for note updates");

                        // run it 
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        var msg = @"A SQL Exception occured during the insert. No data was committed. Please try again." + Environment.NewLine + Environment.NewLine + e;
                        var title = "Database Update Error";
                        MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
                    _log.Error("Exception Thrown: " + msg + " - for Note ID: " + NoteId);
                }
                finally
                {
                    _log.Info("Closing SQL Connection for LoadSubTables.BuildInsertStmt()");
                }
            }
        }
    }
}