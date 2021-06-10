using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;
using DataLogic.Properties;
using log4net;

namespace DataLogic
{
    public class RetreiveAudit
    {
        // Define a static logger variable so that it references the
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public string AppetiteRet;
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
        public int FatigueRet;
        public int FingerPressRet;
        public string HomeRnVisitRet;
        public string HrRet;
        public string KRet;
        public string LabDateRet;
        public string LabObtRet;
        public string LengthOfStayRet;
        public string MdSchNotSchRet;
        public string MdVisitRet;
        public string MedProChgRet;
        public string MedProChgValRet;
        public string NaRet;
        public string NoteDateRet;
        public string NoteId;
        public int PainRet;
        public string PatId;
        public string PatNameRet;
        public string PrimeReasonErVisitRet;
        public string PrimeReasonRehospRet;
        public string RrRet;
        public string Signed;
        public string SignedBy;
        public int SobWithExtRet;
        public int SobWithoutExtRet;
        public string SwollenRet;
        public string UnschedHospRet;
        public string UrineRet;
        public string VitalDateRet;
        public string WeightRet;
        public string WeightTypeRet;
        // ReSharper disable once FunctionComplexityOverflow
        public void ReturnAuditData()
        {
            var preSql = Settings.Default.SqlAuditData;
            var sql = preSql + "@p1 = " + NoteId;
            Log.Debug("SP: " + sql + " for ReturnAuditData()");
            Log.Debug("Dbconn called and created for ReturnAuditData()");
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconn"].ConnectionString))

            using (var cmd = new SqlCommand(sql, conn))
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PatNameRet = reader[0].ToString();
                            PatId = reader[1].ToString();
                            NoteDateRet = reader[2].ToString();
                            SobWithExtRet = Convert.ToInt32(reader[3].ToString());
                            SobWithoutExtRet = Convert.ToInt32(reader[4].ToString());
                            FatigueRet = Convert.ToInt32(reader[5].ToString());
                            PainRet = Convert.ToInt32(reader[6].ToString());
                            FingerPressRet = Convert.ToInt32(reader[7].ToString());
                            BreathingRet = reader[8].ToString();
                            SwollenRet = reader[9].ToString();
                            AppetiteRet = reader[10].ToString();
                            CoughRet = reader[11].ToString();
                            UrineRet = reader[12].ToString();
                            ConfusionRet = reader[13].ToString();
                            DepressionRet = reader[14].ToString();
                            HrRet = reader[15].ToString();
                            BbRet = reader[16].ToString();
                            VitalDateRet = reader[17].ToString();
                            RrRet = reader[18].ToString();
                            WeightRet = reader[19].ToString();
                            WeightTypeRet = reader[20].ToString();
                            LabObtRet = reader[21].ToString();
                            CreatinineRet = reader[22].ToString();
                            NaRet = reader[23].ToString();
                            LabDateRet = reader[24].ToString();
                            BunRet = reader[25].ToString();
                            KRet = reader[26].ToString();
                            MdVisitRet = reader[27].ToString();
                            MdSchNotSchRet = reader[28].ToString();
                            UnschedHospRet = reader[29].ToString();
                            LengthOfStayRet = reader[30].ToString();
                            PrimeReasonRehospRet = reader[31].ToString();
                            ErVisitRet = reader[32].ToString();
                            ErVisitNumOfVisitsRet = reader[33].ToString();
                            PrimeReasonErVisitRet = reader[34].ToString();
                            HomeRnVisitRet = reader[35].ToString();
                            DoseChangeRet = reader[36].ToString();
                            DoseIncDecRet = reader[37].ToString();
                            DoseChgCommentRet = reader[38].ToString();
                            MedProChgRet = reader[39].ToString();
                            MedProChgValRet = reader[40].ToString();
                            CommentsRet = reader[41].ToString();
                            SignedBy = reader[42].ToString();
                            Signed = reader[43].ToString();
                        }
                        Log.Debug("Command executed and reader completed for ReturnAuditData()");
                    }
                }
                catch (SqlException ex)
                {
                    // Build custom message box for exception upload
                    var msg = "RetreiveAudit.ReturnAuditData() Error: " + Environment.NewLine + ex;
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
                    Log.Error("Exception Thrown: " + msg + " - for Patient ID: " + PatId + " for ReturnAuditData()");
                }
                finally
                {
                    Log.Info("Closing SQL Connection for RetreiveAudit.ReturnAuditData()");
                }
        }
    }
}