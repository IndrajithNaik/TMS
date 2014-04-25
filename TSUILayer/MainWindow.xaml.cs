using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TSUILayer.Views;
using WPF.MDI;
using System.Xml.Linq;
using TSUILayer.Views.Admin;
using System.Configuration;
using TSUILayer.Views.PopUps;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Reflection;

namespace TSUILayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow _mainInstance = null;

        public MainWindow()
        {
            InitializeComponent();
            _mainInstance = this;


            this.WindowStyle = WindowStyle.ToolWindow;
            MainMenu.Visibility = Visibility.Collapsed;
            this.Width = 475;
            this.Height = 425;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }

        private void GoToPageExecuteHandler(object sender, ExecutedRoutedEventArgs e)
        {
            frmContent.NavigationService.Navigate(new Uri((string)e.Parameter, UriKind.Relative));
        }

        private void GoToPageCanExecuteHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            if (LogIn())
            {
                CloseLogInForm();
                SetDefaultPage();
                StockReportWindowPopUp srPP = new StockReportWindowPopUp();
                srPP.ShowDialog();
            }
        }

        private bool LogIn()
        {
            UserDetails userDetails = new UserDetails(cboUserType.Text);

            if (txtUid.Text != userDetails.UserName || !userDetails.IsValid(txtPwd.Password))
            {
                qctErrorMessage_login.Visibility = Visibility.Visible;
                qctErrorMessage_login.Content = "Invalid Crdentials";
                return false;
            }

            tractorSale.IsEnabled = tractorPurchase.IsEnabled = userDetails.UserType.Equals("Admin") ? true : false;

            return true;
        }

        private void CloseLogInForm()
        {

            qctErrorMessage_login.Content = string.Empty;
            logInGrid.Visibility = Visibility.Hidden;
            this.WindowState = System.Windows.WindowState.Maximized;
            MainMenu.Visibility = Visibility.Visible;
            this.WindowStyle = WindowStyle.ThreeDBorderWindow;
        }

        public void SetDefaultPage()
        {
            frmContent.NavigationService.Navigate(new Uri("DefaultUserControl.xaml", UriKind.Relative));
            frmContent.Visibility = Visibility.Visible;
        }

        private void logOff_Click(object sender, RoutedEventArgs e)
        {
            this.Width = 475;
            this.Height = 425;
            frmContent.Visibility = Visibility.Hidden;
            this.WindowState = System.Windows.WindowState.Normal;
            this.WindowStyle = WindowStyle.ToolWindow;
            MainMenu.Visibility = Visibility.Collapsed;
            logInGrid.Visibility = Visibility.Visible;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //MainMenu.Style = (Style)FindResource("MyMenuBoxStyle");
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword cPV = new ChangePassword(cboUserType.Text);
            cPV.Show();
        }
    }

    public class UserDetails : IDisposable
    {
        XElement userCredentials = null;
        XElement userTypeDetails = null;

        public UserDetails(string userType)
        {           
            userCredentials = XElement.Load("AdminDetails.xml");
            userTypeDetails = userCredentials.Elements().FirstOrDefault(s => s.Element("Type").Value.Equals(userType));
        }

        public string UserType { get { return userTypeDetails.Element("Type").Value; } }

        private string _userName;

        public string UserName
        {
            get { return userTypeDetails.Element("UserName").Value; ; }
            set { _userName = value; }
        }

        private string _password;

        public string Password
        {
            get { return userTypeDetails.Element("Password").Value; }
            set { _password = Encryptdata(value); }
        }

        string EncryptPassword(string plainText)
        {
            var encrypted = ProtectedData.Protect(Encoding.Unicode.GetBytes(plainText), null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encrypted);
        }

        string DecryptPassword()
        {
            byte[] decrypted = ProtectedData.Unprotect(Convert.FromBase64String(this._password), null, DataProtectionScope.CurrentUser);
            return Encoding.Unicode.GetString(decrypted);
        }

        public bool IsValid(string password)
        {
            return this.Password.Equals(this.Encryptdata(password));
        }

        private string Encryptdata(string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        }

        private string Decryptdata()
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(this._password);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }

        public void Dispose()
        {
            if (_password != null)
            {
                userTypeDetails.Element("Password").Value = this._password;
                userCredentials.Save("AdminDetails.xml");
            }
        }
    }
}
