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
    /// WorkspaceWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WorkspaceWindow : Window
    {
        public WorkspaceWindow()
        {
            InitializeComponent();
            this.Title = Properties.Resources.ProgramName + " Workspace Setting";
            WorkspaceBox.Text = Properties.Settings.Default.Workspace;
            if (WorkspaceBox.Text.Equals(""))
                WorkspaceBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\" + Properties.Resources.ProgramName;
        }

        private void Set_Click(object sender, RoutedEventArgs e)
        {
            string workspace = WorkspaceBox.Text;
            if (Directory.Exists(workspace) && Directory.GetFileSystemEntries(workspace).Length != 0)
            {
                if (MessageBox.Show(Properties.Resources.WorkspaceExist, this.Title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DeleteFolder(workspace);
                    if (!Directory.Exists(workspace))
                        Directory.CreateDirectory(workspace);
                }
                else
                    return;
            }
            if (!Directory.Exists(workspace))
            {
                try
                {
                    Directory.CreateDirectory(workspace);
                }
                catch (IOException)
                {
                    MessageBox.Show(Properties.Resources.PathIllegal, this.Title);
                }
            }
            Properties.Settings.Default.Workspace = workspace;
            Properties.Settings.Default.Save();
            this.DialogResult = true;
        }

        private void WorkspaceView_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Workspace.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void DeleteFolder(string path)
        {
            string[] dirs = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);
            foreach (string dir in dirs)
            {
                DeleteFolder(dir);
            }
            foreach (string file in files)
            {
                new FileInfo(file).Attributes = FileAttributes.Normal;
                File.Delete(file);
            }
            Directory.Delete(path);
        }
    }
}
