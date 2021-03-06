// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Code Analysis results, point to "Suppress Message", and click 
// "In Suppression File".
// You do not need to add suppressions to this file manually.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "DataLogic.GetMaxNote.#ReturnMaxNote()")]
[assembly:
                    SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member",
                                        Target = "DataLogic.RetrieveFormData.#ReturnFormData()")]
[assembly: SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "DataLogic.RemoveSignature.#DeleteSign()")]
[assembly: SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "DataLogic.PatientSearch.#GetPatient()")]
[assembly: SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "DataLogic.LoadPatientData.#LoadNote()")]
[assembly: SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "DataLogic.LoadSignedData.#LoadSign()")]
[assembly: SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "DataLogic.LoadSubTables.#BuildInsertStmt()")]
[assembly:
                    SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member",
                                        Target = "DataLogic.MessageBoxManager.#SetWindowsHookEx(System.Int32,DataLogic.MessageBoxManager+HookProc,System.IntPtr,System.Int32)")]
[assembly:
                    SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member",
                                        Target = "DataLogic.MessageBoxManager.#UnhookWindowsHookEx(System.IntPtr)")]
[assembly:
                    SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member",
                                        Target = "DataLogic.MessageBoxManager.#CallNextHookEx(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr)")]
[assembly:
                    SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member",
                                        Target = "DataLogic.MessageBoxManager.#GetWindowTextLength(System.IntPtr)")]
[assembly:
                    SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member",
                                        Target = "DataLogic.MessageBoxManager.#EnumChildWindows(System.IntPtr,DataLogic.MessageBoxManager+EnumChildProc,System.IntPtr)")]
[assembly:
                    SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member",
                                        Target = "DataLogic.MessageBoxManager.#GetClassName(System.IntPtr,System.Text.StringBuilder,System.Int32)")]
[assembly: SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member", Target = "DataLogic.MessageBoxManager.#GetDlgCtrlID(System.IntPtr)")]
[assembly:
                    SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member",
                                        Target = "DataLogic.MessageBoxManager.#GetDlgItem(System.IntPtr,System.Int32)")]
[assembly:
                    SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member",
                                        Target = "DataLogic.MessageBoxManager.#SetWindowText(System.IntPtr,System.String)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "DataLogic.UpdateNote.#BuildUpdateStmt()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "DataLogic.RetreiveAudit.#ReturnAuditData()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "DataLogic.LoadAuditData.#BuildUpdateStmt()")]
