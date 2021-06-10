using System;
using System.Windows;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace CrystalReports.Reports
{
    public partial class CrystalViewer : Form
    {
        public CrystalViewer()
        {
            InitializeComponent();
        }

        private void CrystalViewer_Load(object sender, EventArgs e)
        {
            var report = new ReportDocument();
            report.Load(@"D:\VisualStudioProjects\CoramForms64\CrystalReports\Reports\CrystalReport1.rpt");
            crystalReportViewer1.ReportSource = report;
            crystalReportViewer1.RefreshReport();
        }

        private void CrystalViewer_SizeChanged(object sender, EventArgs e)
        {
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;
            var windowWidth = Width;
            var windowHeight = Height;
            Left = (int) ((screenWidth / 2) - (windowWidth / 2));
            Top = (int) ((screenHeight / 2) - (windowHeight / 2));
        }
    }
}