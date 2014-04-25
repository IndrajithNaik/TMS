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
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Security.Cryptography;

namespace TSUILayer.Views.PopUps
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        string _userType = string.Empty;

        public ChangePassword(string userType)
        {
            InitializeComponent();
            _userType = userType;
        }

        private void btnChangePassword_Click_1(object sender, RoutedEventArgs e)
        {
            using (UserDetails userDetails = new UserDetails(_userType))
            {
                if (!userDetails.IsValid(txtOldPassword.Password))
                {
                    MessageBox.Show("Please Enter the correct Old Password");
                }
                else
                {
                    if (txtNewPassword.Password == txtConfirmedNewPW.Password)
                    {
                        userDetails.Password = txtNewPassword.Password;
                        MessageBox.Show("The Password has been changed Succesfully");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please enter New password and Confirmation Password Same");
                    }
                }
            }
        }
    }
}
