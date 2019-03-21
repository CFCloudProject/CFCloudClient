using System;
using System.Collections.Generic;
using System.IO;
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
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.Title = Properties.Resources.ProgramName + " Login";
            emailBox.Text = Properties.Settings.Default.Email;
            passwordBox.Password = Properties.Settings.Default.Password;
        }

        private void Button_Register(object sender, RoutedEventArgs e)
        {
            this.Hide();
            RegisterWindow registerWindow = new RegisterWindow();
            bool? result = registerWindow.ShowDialog();
            this.ShowDialog();
        }

        private void Button_Login(object sender, RoutedEventArgs e)
        {
            Models.User user = new Models.User();
            user.Email = emailBox.Text;
            user.Password = passwordBox.Password;

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

            NetworkResults.LoginResult lr = BackgroundWorks.NetworkManager.Login(user);

            if (lr != null && lr.Succeed)
            {
                if (!user.Email.Equals(Properties.Settings.Default.Email) || !Directory.Exists(Properties.Settings.Default.Workspace))
                {
                    WorkspaceWindow workspaceWindow = new WorkspaceWindow();
                    bool? result = workspaceWindow.ShowDialog();
                    if (!result.HasValue && !result.Value)
                        return;
                    Util.SqliteHelper.Init(lr.user.FirstName + lr.user.LastName);
                }
                Properties.Settings.Default.Email = user.Email;
                Properties.Settings.Default.Password = user.Password;
                Properties.Settings.Default.Save();
                Util.Global.user = lr.user;
                Util.SqliteHelper.Connect(lr.user.FirstName + lr.user.LastName);
                this.DialogResult = true;
            }
            else
            {
                if (lr == null || lr.Fail == NetworkResults.LoginResult.FailType.Unknown)
                    MessageBox.Show(Properties.Resources.LoginFailUnknown, Properties.Resources.ProgramName);
                else if (lr.Fail == NetworkResults.LoginResult.FailType.EmailNotExist)
                    MessageBox.Show(Properties.Resources.LoginFailEmail, Properties.Resources.ProgramName);
                else if (lr.Fail == NetworkResults.LoginResult.FailType.PwdError)
                    MessageBox.Show(Properties.Resources.LoginFailPassword, Properties.Resources.ProgramName);
                return;
            }
        }
    }
}
