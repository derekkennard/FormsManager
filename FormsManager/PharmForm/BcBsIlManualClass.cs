using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using FormsManager.Properties;
using log4net;

namespace FormsManager.PharmForm
{
    public class BcBsIlManualClass
    {
        // Define a static logger variable so that it references the
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void GetForm()
        {
            var doc = Resources.BcBsIlManual;
            var ms = new MemoryStream(doc);
            var prePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var postPath = prePath + "\\FormsManager\\Files\\";
            var fileName = Path.Combine(postPath, "BcBsIlManual.pdf");
            try
            {
                //Create PDF File From Binary of resources folders <name>.pdf
                var f = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                //Write Bytes into Our Created <name>.pdf
                ms.WriteTo(f);
                f.Close();
                ms.Close();
            }
            catch (Exception e)
            {
                const string errorName = "BcBsIlManual Form";
                var errorMsg = e.StackTrace;
                const string msgTitle = "Error IOException in Form/App Launch";
                var msgCaption =
                    "You requested a file or program that is already open on your desktop. Please close all files and programs associated with the application and try again.\n\nThis Error is a result of File: " +
                    errorName + " is already open.\n\nStack Trace for IT: " + errorMsg;
                MessageBox.Show(msgCaption, msgTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                Log.Debug("Error from :" + Environment.UserName + " for " + errorName + "with error message: " +
                          e.StackTrace);
            }
            // Finally Show the Created PDF from resources
            finally
            {
                Process.Start(fileName);
            }
        }
    }
}