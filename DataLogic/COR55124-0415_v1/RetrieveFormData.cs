using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;
using DataLogic.Properties;
using log4net;

// ReSharper disable FunctionComplexityOverflow

namespace DataLogic
{
    public class RetrieveFormData
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
        public string ErVisitLenStayRet;
        public string ErVisitRet;
        public int FatigueRet;
        public int FingerPressRet;
        public string HomeRnVisitRet;
        public string HrRet;
        public string IdNumRet;
        public string KRet;
        public DateTime? LabDateRet;
        public string LabObtRet;
        public string MdSchNotSchRet;
        public string MdVisitRet;
        public string MedProChgRet;
        public string MedProChgValRet;
        public string NaRet;
        public DateTime? NoteDateRet;
        public string ObtFromRet;
        public int PainRet;
        public string PatId;
        public string PatNameRet;
        public string PrimeReasonErVisitRet;
        public string PrimeReasonRehospRet;
        public string RrRet;
        public int SobWithExtRet;
        public int SobWithoutExtRet;
        public string SwollenRet;
        public string UnschedHospReasonRet;
        public string UnschedHospRet;
        public string UrineRet;
        public DateTime? VitalDateRet;
        public string WeightRet;
        public string WeightTypeRet;

        public void ReturnFormData()
        {
            var preSql = Settings.Default.SqlRetrieveData;
            var sql = preSql + "@p1 = " + PatId;
            Log.Debug("SP: " + sql + " for ReturnFormData()");
            Log.Debug("Dbconn called and created for ReturnFormData()");
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
                            IdNumRet = reader[1].ToString();
                            NoteDateRet = (DateTime?) reader[2];
                            ObtFromRet = reader[3].ToString();
                            SobWithExtRet = Convert.ToInt32(reader[4].ToString());
                            SobWithoutExtRet = Convert.ToInt32(reader[5].ToString());
                            FatigueRet = Convert.ToInt32(reader[6].ToString());
                            PainRet = Convert.ToInt32(reader[7].ToString());
                            FingerPressRet = Convert.ToInt32(reader[8].ToString());
                            BreathingRet = reader[9].ToString();
                            SwollenRet = reader[10].ToString();
                            AppetiteRet = reader[11].ToString();
                            CoughRet = reader[12].ToString();
                            UrineRet = reader[13].ToString();
                            ConfusionRet = reader[14].ToString();
                            DepressionRet = reader[15].ToString();
                            HrRet = reader[16].ToString();
                            BbRet = reader[17].ToString();
                            VitalDateRet = (DateTime?) reader[18];
                            RrRet = reader[19].ToString();
                            WeightRet = reader[20].ToString();
                            WeightTypeRet = reader[21].ToString();
                            LabObtRet = reader[22].ToString();
                            CreatinineRet = reader[23].ToString();
                            NaRet = reader[24].ToString();
                            LabDateRet = (DateTime?) reader[25];
                            BunRet = reader[26].ToString();
                            KRet = reader[27].ToString();
                            MdVisitRet = reader[28].ToString();
                            MdSchNotSchRet = reader[29].ToString();
                            UnschedHospRet = reader[30].ToString();
                            UnschedHospReasonRet = reader[31].ToString();
                            PrimeReasonRehospRet = reader[32].ToString();
                            ErVisitRet = reader[33].ToString();
                            ErVisitLenStayRet = reader[34].ToString();
                            PrimeReasonErVisitRet = reader[35].ToString();
                            HomeRnVisitRet = reader[36].ToString();
                            DoseChangeRet = reader[37].ToString();
                            DoseIncDecRet = reader[38].ToString();
                            DoseChgCommentRet = reader[39].ToString();
                            MedProChgRet = reader[40].ToString();
                            MedProChgValRet = reader[41].ToString();
                            CommentsRet = reader[42].ToString();
                        }
                        Log.Debug("Command executed and reader completed for ReturnFormData()");
                    }
                }
                catch (SqlException ex)
                {
                    // Build custom message box for exception upload
                    var msg = "RetrieveFormData.ReturnFormData() Error: " + Environment.NewLine + ex;
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
                    Log.Info("Closing SQL Connection for RetrieveFormData.ReturnFormData()");
                }
        }
    }
}