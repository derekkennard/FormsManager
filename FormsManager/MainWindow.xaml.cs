// ReSharper disable PossibleLossOfFraction

using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using BusinessLogic;
using FormsManager.PharmForm;
using log4net;
//using WpfCrystalReport.Staging;
//using WpfCrystalReport.Windows;

namespace FormsManager
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        // Define a static logger variable so that it references the
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Ma430Form Ma430 = new Ma430Form();
        private static readonly AbiForm AbiOrder = new AbiForm();
        private static readonly BaxterTipsForm BxtForm = new BaxterTipsForm();
        private static readonly CurlinPcaForm CurlinPca = new CurlinPcaForm();
        private static readonly CurlinTpnForm CurlinTpn = new CurlinTpnForm();
        private static readonly CurlinVarForm CurlinVar = new CurlinVarForm();
        private static readonly CurlinInterForm CurlinInt = new CurlinInterForm();
        private static readonly WashingtonGuidelines Wsg = new WashingtonGuidelines();
        private static readonly InfusionCaregiverGuide Icg = new InfusionCaregiverGuide();
        private static readonly HizentraInfusionGuide Hig = new HizentraInfusionGuide();
        private static readonly HospiraInfusionGuide Hog = new HospiraInfusionGuide();
        private static readonly HomeInfusionSafetyGuide Hsg = new HomeInfusionSafetyGuide();
        private static readonly AdvanceDirectiveMentalHealthTreatment Adm = new AdvanceDirectiveMentalHealthTreatment();
        private static readonly IhpDiabetes7_29_16 Ihp = new IhpDiabetes7_29_16();

        private static readonly NhsContinuingHealthcareDecisionSupportTool Nhs =
            new NhsContinuingHealthcareDecisionSupportTool();

        private static readonly SapphireQuickStartGuide Sap = new SapphireQuickStartGuide();
        private static readonly SkilledNursingNote Srn = new SkilledNursingNote();
        private static readonly HomeInfusionReferralForm Hir = new HomeInfusionReferralForm();
        private static readonly SoluMedrolForm Sol = new SoluMedrolForm();

        private static readonly AetnaIgTherapyPrecertificationRequestForm Aig =
            new AetnaIgTherapyPrecertificationRequestForm();

        private static readonly BcBsIlManual Bil = new BcBsIlManual();
        private static readonly HomeIvServicesPriorAuth Haf = new HomeIvServicesPriorAuth();
        private static readonly MedPharmHitValidation Mpv = new MedPharmHitValidation();
        private static readonly ZoledronicAcidPhysicianOrder Zap = new ZoledronicAcidPhysicianOrder();
        private static readonly HomeInfusionInjectableGuidelines Hii = new HomeInfusionInjectableGuidelines();
        private static readonly HyperemesisReferralForm Her = new HyperemesisReferralForm();
        private static readonly IronPhysicianOrder Ipo = new IronPhysicianOrder();
        private static readonly IvigPhysOrder Igp = new IvigPhysOrder();
        private static readonly RemicadePhysicianOrder Rpo = new RemicadePhysicianOrder();
        private static readonly PlaceHolder Plc = new PlaceHolder();

        public MainWindow()
        {
            InitializeComponent();

            var prePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var logPath = prePath + "\\FormsManager\\Logs";
            var filePath = prePath + "\\FormsManager\\Files";
            LoadPharmForms();
            LblBuildVersion.Content = GetPublishedVersion();
            Log.Info("MainWindow Initialize Component Started by " + Environment.UserName);
            Log.Info("Log File Directory Clean up Task - Check " + logPath +
                     " for log files older than 3 days. Delete when found.");
            var version = PublishVersion;
            LblVersion.Content = "Powered by Razor Sharp Technology: v" + version;
            try
            {
                var systemDate = DateTime.Now;
                const int defaultDay = 3;
                foreach (var fileInformation in
                    from fileInformation in new DirectoryInfo(logPath).GetFiles()
                    let difference = systemDate - fileInformation.LastWriteTime
                    where difference.Days >= defaultDay
                    select fileInformation)
                    File.Delete(fileInformation.FullName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Debug("Log File Delete Failure: " + ex);
            }

            try
            {
                var systemDate = DateTime.Now;
                const int defaultDay = 3;
                foreach (var fileInformation in from fileInformation in new DirectoryInfo(filePath).GetFiles()
                    let difference = systemDate - fileInformation.LastWriteTime
                    where difference.Days >= defaultDay
                    select fileInformation)
                    File.Delete(fileInformation.FullName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Debug("Files Delete Failure: " + ex);
            }
        }

        public string PublishVersion
        {
            get
            {
                string version;
                try
                {
                    //// get deployment version
                    version = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                }
                catch (InvalidDeploymentException)
                {
                    //// you cannot read publish version when app isn't installed 
                    //// (e.g. during debug)
                    version = "not installed";
                }
                return version;
            }
        }

        private static string GetPublishedVersion()
        {
            return ApplicationDeployment.IsNetworkDeployed
                ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
                : "Debug Mode";
        }

        private void ListViewItem_PharmDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var obj = (DependencyObject) e.OriginalSource;

            while (obj != null && !Equals(obj, PharmacyLv))
            {
                if (obj.GetType() == typeof(ListViewItem))
                {
                    // ReSharper disable once InconsistentNaming
                    var Item = (ListViewItem) sender;
                    var pf = (PharmacyForms) Item.Content;
                    // Do something here :)
                    // Actually, do a lot. Build a Message box that switches results from YES/NO
                    // Then do a if/else (well, another switch) to find out WHAT form was called.
                    var msgTitle = "Form Request " + pf.FormNamePharm;
                    var msgCaption = "You requested to open " + pf.FormCodePharm + " - " + pf.FormNamePharm +
                                     ". \nPress OK to continue or CANCEL to select again.";
                    var result = MessageBox.Show(msgCaption, msgTitle, MessageBoxButton.OKCancel,
                        MessageBoxImage.Question);

                    switch (result)
                    {
                        case MessageBoxResult.OK:
                            switch (pf.FormCodePharm)
                            {
                                case "430":
                                    Ma430Struct();
                                    Log.Info("Opening Mass 430 Auth Form");
                                    break;
                                case "ABI-01-2016":
                                    AboOrderStruct();
                                    Log.Info("Opening ABI-01-2016 Form");
                                    break;
                                case "AdvanceDirectiveMentalHealthTreatment":
                                    AdvanceDirectiveMentalHealthTreatmentStruct();
                                    Log.Info("Opening Advance Directive Mental Health Treatment Form");
                                    break;
                                case "AetnaIgTherapyPrecertificationRequestForm":
                                    AetnaIgTherapyPrecertificationRequestFormStruct();
                                    Log.Info("Opening AetnaIgTherapyPrecertificationRequest Form");
                                    break;
                                case "Baxter-6201-Tips":
                                    BxtTipStruct();
                                    Log.Info("Opening Bxt 6201 Form");
                                    break;
                                case "BcBsIlManual":
                                    BcBsIlManualStruct();
                                    Log.Info("Opening BcBsIlManual Form");
                                    break;
                                case "CURLIN_PCA_Therapy":
                                    CurPcaStruct();
                                    Log.Info("Opening CURLIN_PCA_Therapy Form");
                                    break;
                                case "CURLIN_TPN_Therapy":
                                    CurTpnStruct();
                                    Log.Info("Opening CURLIN_TPN_Therapy Form");
                                    break;
                                case "CURLIN_Variable_Therapy":
                                    CurVarStruct();
                                    Log.Info("Opening CURLIN_Variable_Therapy Form");
                                    break;
                                case "Curlin_IntermittentTherapy":
                                    CurIntStruct();
                                    Log.Info("Opening Curlin_IntermittentTherapy Form");
                                    break;
                                case "HizentraInfusionGuide":
                                    HizentraInfusionGuideStruct();
                                    Log.Info("Opening HizentraInfusionGuide Form");
                                    break;
                                case "HomeInfusionInjectableGuidelines":
                                    HomeInfusionInjectableGuidelinesStruct();
                                    Log.Info("Opening HomeInfusionInjectableGuidelines Form");
                                    break;
                                case "HomeIvServicesPriorAuth":
                                    HomeIvServicesPriorAuthStruct();
                                    Log.Info("Opening HomeIvServicesPriorAuthStruct Form");
                                    break;
                                case "HomeInfusionSafetyGuide":
                                    HomeInfusionSafetyGuideStruct();
                                    Log.Info("Opening HomeInfusionSafetyGuideStruct Form");
                                    break;
                                case "HomeInfusionReferralForm":
                                    HomeInfusionReferralFormStruct();
                                    Log.Info("Opening HomeInfusionReferralFormStruct Form");
                                    break;
                                case "HospiraInfusionGuide":
                                    HospiraInfusionGuideStruct();
                                    Log.Info("Opening HospiraInfusionGuideStruct Form");
                                    break;
                                case "HyperemesisReferralForm":
                                    HyperemesisReferralFormStruct();
                                    Log.Info("Opening HyperemesisReferralFormStruct Form");
                                    break;
                                case "InfusionCaregiverGuide":
                                    InfusionCaregiverGuideStruct();
                                    Log.Info("Opening InfusionCaregiverGuide Form");
                                    break;
                                case "IhpDiabetes7_29_16":
                                    IhpDiabetes7_29_16Struct();
                                    Log.Info("Opening IhpDiabetes7_29_16 Form");
                                    break;
                                case "IronPhysicianOrder":
                                    IronPhysicianOrderStruct();
                                    Log.Info("Opening IronPhysicianOrderStruct Form");
                                    break;
                                case "IvigPhysOrder":
                                    IvigPhysOrderStruct();
                                    Log.Info("Opening IvigPhysOrderStruct Form");
                                    break;
                                case "MedPharmHitValidation":
                                    MedPharmHitValidationStruct();
                                    Log.Info("Opening MedPharmHitValidation Form");
                                    break;
                                case "NhsContinuingHealthcareDecisionSupportTool":
                                    NhsContinuingHealthcareDecisionSupportToolStruct();
                                    Log.Info("Opening NhsContinuingHealthcareDecisionSupportToolStruct Form");
                                    break;
                                case "RemicadePhysicianOrder":
                                    RemicadePhysicianOrderStruct();
                                    Log.Info("Opening RemicadePhysicianOrderStruct Form");
                                    break;
                                case "SapphireQuickStartGuide":
                                    SapphireQuickStartGuideStruct();
                                    Log.Info("Opening SapphireQuickStartGuide Form");
                                    break;
                                case "SkilledNursingNote":
                                    SkilledNursingNoteStruct();
                                    Log.Info("Opening SkilledNursingNote Form");
                                    break;
                                case "SoluMedrolForm":
                                    SoluMedrolFormStruct();
                                    Log.Info("Opening SoluMedrolForm Form");
                                    break;
                                case "WashingtonStateInfusionGuideline":
                                    WsgStruct();
                                    Log.Info("Opening WsgStruct()");
                                    break;
                                case "ZoledronicAcidPhysicianOrder":
                                    ZoledronicAcidPhysicianOrderStruct();
                                    Log.Info("Opening ZoledronicAcidPhysicianOrderStruct()");
                                    break;
                                //case "@24_4023":
                                //    Rpt4023();
                                //    Log.Info("Opening Prescriptions/Rx Created Today Report (@24_4023)");
                                //    break;
                                //case "@24_8005":
                                //    Rpt8005InotropicForm();
                                //    Log.Info("Opening Inotropic Clinical Progress Report (@24_8005)");
                                //    break;
                                //case "@24_8006":
                                //    Rpt8006InotropicFormPreview();
                                //    Log.Info("Opening Inotropic Audit Preview Report (@24_8006)");
                                //    break;
                                case "-":
                                    Inspire();
                                    Log.Info("Opening Inspire");
                                    break;
                                case "":
                                    break;
                            }
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                       "\\FormsManager\\Files\\";
            // ReSharper disable once AssignNullToNotNullAttribute
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            GC.Collect();
            CollectionViewSource.GetDefaultView(PharmacyLv.ItemsSource).Filter = UserFilter;
            Log.Info("Window_Loaded and PharmacyLv.ItemsSource Filter created by CollectionViewSource");
        }

        private void LoadPharmForms()
        {
            var items = new List<PharmacyForms>
            {
                new PharmacyForms
                {
                    FormCodePharm = Ma430.Code,
                    FormDatePharm = Ma430.Date,
                    FormNamePharm = Ma430.Name,
                    FormDescPharm = Ma430.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = AbiOrder.Code,
                    FormDatePharm = AbiOrder.Date,
                    FormNamePharm = AbiOrder.Name,
                    FormDescPharm = AbiOrder.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Adm.Code,
                    FormDatePharm = Adm.Date,
                    FormNamePharm = Adm.Name,
                    FormDescPharm = Adm.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Aig.Code,
                    FormDatePharm = Aig.Date,
                    FormNamePharm = Aig.Name,
                    FormDescPharm = Aig.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Bil.Code,
                    FormDatePharm = Bil.Date,
                    FormNamePharm = Bil.Name,
                    FormDescPharm = Bil.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = BxtForm.Code,
                    FormDatePharm = BxtForm.Date,
                    FormNamePharm = BxtForm.Name,
                    FormDescPharm = BxtForm.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = CurlinPca.Code,
                    FormDatePharm = CurlinPca.Date,
                    FormNamePharm = CurlinPca.Name,
                    FormDescPharm = CurlinPca.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = CurlinTpn.Code,
                    FormDatePharm = CurlinTpn.Date,
                    FormNamePharm = CurlinTpn.Name,
                    FormDescPharm = CurlinTpn.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = CurlinVar.Code,
                    FormDatePharm = CurlinVar.Date,
                    FormNamePharm = CurlinVar.Name,
                    FormDescPharm = CurlinVar.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = CurlinInt.Code,
                    FormDatePharm = CurlinInt.Date,
                    FormNamePharm = CurlinInt.Name,
                    FormDescPharm = CurlinInt.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Haf.Code,
                    FormDatePharm = Haf.Date,
                    FormNamePharm = Haf.Name,
                    FormDescPharm = Haf.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Her.Code,
                    FormDatePharm = Her.Date,
                    FormNamePharm = Her.Name,
                    FormDescPharm = Her.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Hig.Code,
                    FormDatePharm = Hig.Date,
                    FormNamePharm = Hig.Name,
                    FormDescPharm = Hig.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Hii.Code,
                    FormDatePharm = Hii.Date,
                    FormNamePharm = Hii.Name,
                    FormDescPharm = Hii.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Hir.Code,
                    FormDatePharm = Hir.Date,
                    FormNamePharm = Hir.Name,
                    FormDescPharm = Hir.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Hsg.Code,
                    FormDatePharm = Hsg.Date,
                    FormNamePharm = Hsg.Name,
                    FormDescPharm = Hsg.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Hog.Code,
                    FormDatePharm = Hog.Date,
                    FormNamePharm = Hog.Name,
                    FormDescPharm = Hog.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Icg.Code,
                    FormDatePharm = Icg.Date,
                    FormNamePharm = Icg.Name,
                    FormDescPharm = Icg.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Ihp.Code,
                    FormDatePharm = Ihp.Date,
                    FormNamePharm = Ihp.Name,
                    FormDescPharm = Ihp.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Ipo.Code,
                    FormDatePharm = Ipo.Date,
                    FormNamePharm = Ipo.Name,
                    FormDescPharm = Ipo.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Igp.Code,
                    FormDatePharm = Igp.Date,
                    FormNamePharm = Igp.Name,
                    FormDescPharm = Igp.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Mpv.Code,
                    FormDatePharm = Mpv.Date,
                    FormNamePharm = Mpv.Name,
                    FormDescPharm = Mpv.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Nhs.Code,
                    FormDatePharm = Nhs.Date,
                    FormNamePharm = Nhs.Name,
                    FormDescPharm = Nhs.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Rpo.Code,
                    FormDatePharm = Rpo.Date,
                    FormNamePharm = Rpo.Name,
                    FormDescPharm = Rpo.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Sap.Code,
                    FormDatePharm = Sap.Date,
                    FormNamePharm = Sap.Name,
                    FormDescPharm = Sap.Desc
                },

                new PharmacyForms
                {
                    FormCodePharm = Sol.Code,
                    FormDatePharm = Sol.Date,
                    FormNamePharm = Sol.Name,
                    FormDescPharm = Sol.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Srn.Code,
                    FormDatePharm = Srn.Date,
                    FormNamePharm = Srn.Name,
                    FormDescPharm = Srn.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Wsg.Code,
                    FormDatePharm = Wsg.Date,
                    FormNamePharm = Wsg.Name,
                    FormDescPharm = Wsg.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Zap.Code,
                    FormDatePharm = Zap.Date,
                    FormNamePharm = Zap.Name,
                    FormDescPharm = Zap.Desc
                },
                new PharmacyForms
                {
                    FormCodePharm = Plc.Code,
                    FormDatePharm = Plc.Date,
                    FormNamePharm = Plc.Name,
                    FormDescPharm = Plc.Desc
                }
            };

            PharmacyLv.ItemsSource = items;
            var view = (CollectionView) CollectionViewSource.GetDefaultView(PharmacyLv.ItemsSource);
            view.Filter = UserFilter;
            Log.Info("LoadPharmForms Initiated");
        }

        private bool UserFilter(object item)
        {
            if (string.IsNullOrEmpty(TBoxFormSearch.Text))
                return true;
            var pharmacyForms = item as PharmacyForms;
            return pharmacyForms != null &&
                   (pharmacyForms.FormNamePharm.IndexOf(TBoxFormSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0) |
                   (pharmacyForms.FormDescPharm.IndexOf(TBoxFormSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void tBoxFormSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(PharmacyLv.ItemsSource).Refresh();
        }

        private static void Ma430Struct()
        {
            Ma430Class.GetForm();
            Log.Info("Initiating GetMa430()");
        }

        private static void AboOrderStruct()
        {
            AbiOrderFormClass.GetForm();
            Log.Info("Initiating GetAbiOrderForm()");
        }

        private static void AdvanceDirectiveMentalHealthTreatmentStruct()
        {
            AdvanceDirectiveMentalHealthTreatmentClass.GetForm();
            Log.Info("Initiating AdvanceDirectiveMentalHealthTreatmentStruct()");
        }

        private static void AetnaIgTherapyPrecertificationRequestFormStruct()
        {
            AetnaIgTherapyPrecertificationRequestFormClass.GetForm();
            Log.Info("Initiating AetnaIgTherapyPrecertificationRequestFormStruct()");
        }

        private static void BcBsIlManualStruct()
        {
            BcBsIlManualClass.GetForm();
            Log.Info("Initiating BcBsIlManualStruct");
        }

        private static void BxtTipStruct()
        {
            Baxter6201Class.GetForm();
            Log.Info("Initiating BxtTipStruct");
        }

        private static void CurPcaStruct()
        {
            CurlinPcaClass.GetForm();
            Log.Info("Initiating CurPcaStruct");
        }

        private static void CurTpnStruct()
        {
            CurlinTpnClass.GetForm();
            Log.Info("Initiating CurTpnStruct");
        }

        private static void CurVarStruct()
        {
            CurlinVariableTherapyClass.GetForm();
            Log.Info("Initiating CurVarStruct");
        }

        private static void CurIntStruct()
        {
            CurlinInterClass.GetForm();
            Log.Info("Initiating CurIntStruct");
        }

        private static void HomeIvServicesPriorAuthStruct()
        {
            HomeIvServicesPriorAuthClass.GetForm();
            Log.Info("Initiating HomeIvServicesPriorAuthStruct");
        }

        private static void HizentraInfusionGuideStruct()
        {
            HizentraInfusionGuideClass.GetForm();
            Log.Info("Initiating HizentraInfusionGuideStruct");
        }

        private static void HomeInfusionInjectableGuidelinesStruct()
        {
            HomeInfusionInjectableGuidelinesClass.GetForm();
            Log.Info("Initiating HomeInfusionInjectableGuidelinesStruct");
        }

        private static void HomeInfusionSafetyGuideStruct()
        {
            HomeInfusionSafetyGuideClass.GetForm();
            Log.Info("Initiating HomeInfusionSafetyGuideStruct");
        }

        private static void HomeInfusionReferralFormStruct()
        {
            HomeInfusionReferralFormClass.GetForm();
            Log.Info("Initiating HomeInfusionReferralFormStruct");
        }

        private static void HospiraInfusionGuideStruct()
        {
            HospiraInfusionGuideClass.GetForm();
            Log.Info("Initiating HospiraInfusionGuideStruct");
        }

        private static void HyperemesisReferralFormStruct()
        {
            HyperemesisReferralFormClass.GetForm();
            Log.Info("Initiating HyperemesisReferralFormStruct");
        }

        private static void InfusionCaregiverGuideStruct()
        {
            InfusionCaregiverGuideClass.GetForm();
            Log.Info("Initiating InfusionCaregiverGuideStruct");
        }

        private static void IhpDiabetes7_29_16Struct()
        {
            IhpDiabetes7_29_16Class.GetForm();
            Log.Info("Initiating IhpDiabetes7_29_16Struct");
        }

        private static void IronPhysicianOrderStruct()
        {
            IronPhysicianOrderClass.GetForm();
            Log.Info("Initiating IronPhysicianOrderStruct");
        }

        private static void IvigPhysOrderStruct()
        {
            IvigPhysOrderClass.GetForm();
            Log.Info("Initiating IvigPhysOrderStruct");
        }

        private static void MedPharmHitValidationStruct()
        {
            MedPharmHitValidationClass.GetForm();
            Log.Info("Initiating MedPharmHitValidationStruct");
        }

        private static void NhsContinuingHealthcareDecisionSupportToolStruct()
        {
            NhsContinuingHealthcareDecisionSupportToolClass.GetForm();
            Log.Info("Initiating NhsContinuingHealthcareDecisionSupportToolStruct");
        }

        private static void RemicadePhysicianOrderStruct()
        {
            RemicadePhysicianOrderClass.GetForm();
            Log.Info("Initiating RemicadePhysicianOrderStruct");
        }

        private static void SapphireQuickStartGuideStruct()
        {
            SapphireQuickStartGuideClass.GetForm();
            Log.Info("Initiating SapphireQuickStartGuideStruct");
        }

        private static void SkilledNursingNoteStruct()
        {
            SkilledNursingNoteClass.GetForm();
            Log.Info("Initiating SkilledNursingNoteStruct");
        }

        private static void SoluMedrolFormStruct()
        {
            SoluMedrolFormClass.GetForm();
            Log.Info("Initiating SoluMedrolFormStruct");
        }

        private static void WsgStruct()
        {
            WashingtonStateGuideClass.GetForm();
            Log.Info("Initiating WsgStruct");
        }

        private static void ZoledronicAcidPhysicianOrderStruct()
        {
            ZoledronicAcidPhysicianOrderClass.GetForm();
            Log.Info("Initiating ZoledronicAcidPhysicianOrderStruct");
        }

        //private static void Rpt4023()
        //{
        //    Rpt244023Deploy.DeployRpt();
        //    var wn1 = new Rpt244023();
        //    wn1.Show();
        //    Log.Info("Initiating Rpt4023()");
        //}

        //private static void Rpt8005InotropicForm()
        //{
        //    Rpt8005InotropicFormDeploy.DeployRpt();
        //    var wn1 = new Rpt8005InotropicForm();
        //    wn1.Show();
        //    Log.Info("Initiating Rpt8005InotropicForm()");
        //}

        //private static void Rpt8006InotropicFormPreview()
        //{
        //    Rpt8006InotropicFormPreviewDeploy.DeployRpt();
        //    var wn1 = new Rpt8006InotropicFormPreview();
        //    wn1.Show();
        //    Log.Info("Initiating Rpt8006InotropicFormPreview()");
        //}

        private static void Inspire()
        {
            var wn3 = new Window2();
            wn3.Show();
            Log.Info("Initiating Inspire()");
        }

        private void Exp1_Expanded(object sender, RoutedEventArgs e)
        {
            TBoxFormSearch.IsReadOnly = false;
            TBoxFormSearch.Focus();
        }

        private void Exp1_Collapsed(object sender, RoutedEventArgs e)
        {
            TBoxFormSearch.Clear();
            TBoxFormSearch.IsReadOnly = true;
        }

        private void CenterWindowOnScreen(object sender, SizeChangedEventArgs e)
        {
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;
            var windowWidth = Width;
            var windowHeight = Height;
            Left = screenWidth / 2 - windowWidth / 2;
            Top = screenHeight / 2 - windowHeight / 2;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scv = (ScrollViewer) sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void Help_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://razorsharptech.com/osticket/");
        }

        [SuppressUnmanagedCodeSecurity]
        internal static class SafeNativeMethods
        {
            [DllImport("Kernel32")]
            internal static extern bool CloseHandle(IntPtr handle);
        }
    }
}