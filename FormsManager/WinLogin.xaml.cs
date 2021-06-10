using System;
using System.Windows;
using BusinessLogic;

namespace FormsManager
{
    public partial class WinLogin
    {
        public string Domain;
        public string FullName;

        public WinLogin()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TxtDomain.Text = Environment.UserDomainName.ToUpper();
        }

        private void WinLoginLogin_Click(object sender, RoutedEventArgs e)
        {
            var ad = new AdHelper();

            if (ad.AuthenticateUser(TxtDomain.Text, TxtUserName.Text, TxtPassword.Password))
            {
                DialogResult = true;
            }
            else
                MessageBox.Show("Unable to Authenticate Using the Supplied Credentials");
        }

        public void ReturnNames()
        {
            FullName = TxtUserName.Text;
            Domain = TxtDomain.Text;
        }
    }
}