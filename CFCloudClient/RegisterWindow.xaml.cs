using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CFCloudClient
{
    /// <summary>
    /// RegisterWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            this.Title = Properties.Resources.ProgramName + " Register";
        }

        private void Button_Register(object sender, RoutedEventArgs e)
        {
            Models.User user = new Models.User();
            user.FirstName = FirstNameBox.Text;
            user.LastName = LastNameBox.Text;
            user.Email = emailBox.Text;
            user.Password = passwordBox.Password;

            if (user.FirstName.Length == 0)
            {
                MessageBox.Show(Properties.Resources.FirstNameEmpty, Properties.Resources.ProgramName);
                return;
            }
            if (user.LastName.Length == 0)
            {
                MessageBox.Show(Properties.Resources.LastNameEmpty, Properties.Resources.ProgramName);
                return;
            }
            if (user.Email.Length == 0)
            {
                MessageBox.Show(Properties.Resources.EmailEmpty, Properties.Resources.ProgramName);
                return;
            }
            if (user.Password.Length == 0)
            {
                MessageBox.Show(Properties.Resources.PasswordEmpty, Properties.Resources.ProgramName);
                return;
            }
            if (!user.Password.Equals(ConfirmpasswordBox.Password))
            {
                MessageBox.Show(Properties.Resources.PasswordConfirmError, Properties.Resources.ProgramName);
                return;
            }

            NetworkResults.RegisterResult rr = BackgroundWorks.NetworkManager.Register(user);
            if (rr != null && rr.Succeed)
            {
                this.DialogResult = true;
            }
            else
            {
                if (rr == null || rr.Fail == NetworkResults.RegisterResult.FailType.Unknown)
                    MessageBox.Show(Properties.Resources.RegisterFailUnknown, Properties.Resources.ProgramName);
                else
                    MessageBox.Show(Properties.Resources.RegisterFailEmail, Properties.Resources.ProgramName);
                return;
            }
        }
    }
}
