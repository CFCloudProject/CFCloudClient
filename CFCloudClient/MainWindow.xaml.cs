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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CFCloudClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string CurrentFolder = null;
        private List<Models.FileSystemItem> items = new List<Models.FileSystemItem>();

        public MainWindow()
        {
            InitializeComponent();
            LoginWindow loginWindow = new LoginWindow();
            bool? result = loginWindow.ShowDialog();
            if (!result.HasValue || !result.Value)
                this.Close();
            this.Title = Properties.Resources.ProgramName + ": " 
                + Util.Global.info.user.FirstName + " "
                + Util.Global.info.user.LastName + " ("
                + Util.Global.info.user.Email + ")";
            this.Closed += OnClose;
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Util.Utils.LockAllFiles(Properties.Settings.Default.Workspace);
            Util.Utils.CheckConsistency(Properties.Settings.Default.Workspace);
            ChangeCurrentFolder(Properties.Settings.Default.Workspace);
            FileGrid.ItemsSource = items;
            RefreshFileGrid();
            Util.Global.FileMonitor = new BackgroundWorks.FileChangeMonitor();
            Util.Global.FileMonitor.OnFileChange += FileMonitor_OnFileChange;
            Util.Global.FileMonitor.Start();
            Util.Global.FileUpdateQueue = new BackgroundWorks.FileChangeQueue();
            Util.Global.updater = new BackgroundWorks.FileUpdater();
            BackgroundWorks.HeartBeat.Start();
        }

        private void FileMonitor_OnFileChange(object sender, EventArgs e)
        {
            RefreshFileGrid();
        }

        private void OnClose(object sender, EventArgs e)
        {
            BackgroundWorks.NetworkManager.Logout();
            Util.Utils.UnLockAllFiles();
            Util.Global.FileMonitor.Stop();
            Util.Global.updater.Destory();
            Util.SqliteHelper.Close();
        }

        private void ChangeCurrentFolder(string folder)
        {
            CurrentFolder = folder;
            if (CurrentFolder.Equals(Properties.Settings.Default.Workspace))
            {
                Upper.Visibility = Visibility.Collapsed;
            }
            else
            {
                Upper.Visibility = Visibility.Visible;
            }
        }

        private void RefreshFileGrid()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                CurrentF.Text = CurrentFolder;
                items.Clear();
                string[] dirs = Directory.GetDirectories(CurrentFolder);
                foreach (string dir in dirs)
                {
                    items.Add(new Models.FileSystemItem(dir, 0));
                }
                string[] files = Directory.GetFiles(CurrentFolder);
                foreach (string file in files)
                {
                    items.Add(new Models.FileSystemItem(file, 1));
                }
                FileGrid.Items.Refresh();
            }));
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshFileGrid();
        }

        private void Upper_Click(object sender, RoutedEventArgs e)
        {
            if (!CurrentFolder.Equals(Properties.Settings.Default.Workspace))
                ChangeCurrentFolder(Directory.GetParent(CurrentFolder).FullName);
            RefreshFileGrid();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            InputWindow inputWindow = new InputWindow(Properties.Resources.InputFolderName, "Create New Folder");
            string folderName = inputWindow.getInput();
            if (folderName != null && !Directory.Exists(CurrentFolder + "\\" + folderName))
            {
                try
                {
                    Directory.CreateDirectory(CurrentFolder + "\\" + folderName);
                }
                catch (IOException)
                {
                    MessageBox.Show(Properties.Resources.PathIllegal, "Create New Folder Error");
                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string src = null;
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                src = ofd.FileName;
            }
            if (src == null)
                return;
            else
            {
                if (!File.Exists(src))
                    MessageBox.Show(Properties.Resources.NotaFile, "Add New File Error");
                else
                {
                    string filename = src.Substring(src.LastIndexOf('\\') + 1);
                    string dst = CurrentFolder + "\\" + filename;
                    if (File.Exists(dst))
                    {
                        if (MessageBox.Show(Properties.Resources.FileExist, "Add New File", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            File.Copy(src, dst, true);
                    }
                    else
                        File.Copy(src, dst);
                }
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Models.FileSystemItem item = (Models.FileSystemItem)((Button)sender).DataContext;
            string filename = CurrentFolder + "\\" + item.Name;
            if (item.Type.Equals("Folder"))
            {
                ChangeCurrentFolder(filename);
                RefreshFileGrid();
            }
            else
            {
                System.Diagnostics.Process.Start(filename);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Models.FileSystemItem item = (Models.FileSystemItem)((Button)sender).DataContext;
            string filename = CurrentFolder + "\\" + item.Name;
            if (!item.Type.Equals("Folder"))
            {
                Models.SQLDataType sdt = Util.SqliteHelper.Select(filename);
                if (sdt.IsShared.Equals("true"))
                {
                    NetworkResults.GetTokenResult gr = BackgroundWorks.NetworkManager.GetToken(Util.Utils.LocalPathtoCloudPath(filename), sdt.Version);
                    if (gr != null && !gr.Succeed)
                    {
                        if (gr.Fail == NetworkResults.GetTokenResult.FailType.OtherHolding)
                            MessageBox.Show(Properties.Resources.GetTokenFail + gr.TokenHolder.TokenHolder.getUserName(), "Get Token Fail");
                        else
                        {
                            Util.Global.FileUpdateQueue.Add(new Models.FileChangeEvent(
                                Models.FileChangeEvent.FileChangeType.ServerChange,
                                filename));
                            MessageBox.Show(Properties.Resources.GetTokenFailUnconsistent, "Get Token Fail");
                        }
                    }
                    else
                    {
                        Util.Utils.UnLockFile(filename);
                        System.Diagnostics.Process.Start(filename);
                    }
                }
                else
                {
                    System.Diagnostics.Process.Start(filename);
                }
            }
        }

        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            Models.FileSystemItem item = (Models.FileSystemItem)((Button)sender).DataContext;
            string oldName = CurrentFolder + "\\" + item.Name;
            Models.SQLDataType sdt = Util.SqliteHelper.Select(oldName);
            if (sdt.IsShared.Equals("true"))
            {
                if (item.Type.Equals("Folder"))
                {
                    NetworkResults.GetFolderTokenResult gfr = BackgroundWorks.NetworkManager.GetFolderToken(Util.Utils.LocalPathtoCloudPath(oldName));
                    if (gfr != null && !gfr.Succeed)
                    {
                        MessageBox.Show(gfr.FailMessage(), "Rename Fail");
                        return;
                    }
                    InputWindow inputWindow = new InputWindow(Properties.Resources.RenameText, "Rename");
                    string newName = inputWindow.getInput();
                    if (newName == null)
                        return;
                    newName = CurrentFolder + "\\" + newName;
                    try
                    {
                        new DirectoryInfo(oldName).MoveTo(newName);
                    }
                    catch (IOException)
                    {
                        MessageBox.Show(Properties.Resources.RenameFail, "Rename Fail");
                    }
                }
                else
                {
                    NetworkResults.GetTokenResult gr = BackgroundWorks.NetworkManager.GetToken(Util.Utils.LocalPathtoCloudPath(oldName), sdt.Version);
                    if (gr != null && !gr.Succeed && gr.Fail == NetworkResults.GetTokenResult.FailType.OtherHolding)
                    {
                        MessageBox.Show(Properties.Resources.GetTokenFail + gr.TokenHolder.TokenHolder.getUserName(), "Rename Fail");
                        return;
                    }
                    InputWindow inputWindow = new InputWindow(Properties.Resources.RenameText, "Rename");
                    string newName = inputWindow.getInput();
                    if (newName == null)
                        return;
                    newName = CurrentFolder + "\\" + newName;
                    Util.Utils.UnLockFile(oldName);
                    new FileInfo(oldName).Attributes = FileAttributes.Normal;
                    try
                    {
                        new FileInfo(oldName).MoveTo(newName);
                    }
                    catch (IOException)
                    {
                        MessageBox.Show(Properties.Resources.RenameFail, "Rename Fail");
                    }
                }
            }
            else
            {
                InputWindow inputWindow = new InputWindow(Properties.Resources.RenameText, "Rename");
                string newName = inputWindow.getInput();
                if (newName == null)
                    return;
                newName = CurrentFolder + "\\" + newName;
                if (item.Type.Equals("Folder"))
                {
                    try
                    {
                        new DirectoryInfo(oldName).MoveTo(newName);
                    }
                    catch (IOException)
                    {
                        MessageBox.Show(Properties.Resources.RenameFail, "Rename Fail");
                    }
                }
                else
                {
                    try
                    {
                        new FileInfo(oldName).MoveTo(newName);
                    }
                    catch (IOException)
                    {
                        MessageBox.Show(Properties.Resources.RenameFail, "Rename Fail");
                    }
                }
            }
        }

        private void Share_Click(object sender, RoutedEventArgs e)
        {
            Models.FileSystemItem item = (Models.FileSystemItem)((Button)sender).DataContext;
            string filename = CurrentFolder + "\\" + item.Name;
            InputWindow inputWindow = new InputWindow(Properties.Resources.ShareText, "Share");
            string Email = inputWindow.getInput();
            if (Email == null)
                return;
            else
            {
                if (BackgroundWorks.NetworkManager.Share(Util.Utils.LocalPathtoCloudPath(filename), Email) != null)
                    MessageBox.Show("Share succeed.", "Share");
                else
                    MessageBox.Show("Share failed.", "Share");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Models.FileSystemItem item = (Models.FileSystemItem)((Button)sender).DataContext;
            string filename = CurrentFolder + "\\" + item.Name;
            Models.SQLDataType sdt = Util.SqliteHelper.Select(filename);
            if (sdt.IsShared.Equals("true"))
            {
                if (item.Type.Equals("Folder"))
                {
                    NetworkResults.GetFolderTokenResult gfr = BackgroundWorks.NetworkManager.GetFolderToken(Util.Utils.LocalPathtoCloudPath(filename));
                    if (gfr != null && !gfr.Succeed)
                    {
                        MessageBox.Show(gfr.FailMessage(), "Delete Fail");
                        return;
                    }
                    if (MessageBox.Show(Properties.Resources.FolderDeleteConfirm, "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Util.Utils.DeleteFolder(filename);
                    }
                    else
                        return;
                }
                else
                {
                    NetworkResults.GetTokenResult gr = BackgroundWorks.NetworkManager.GetToken(Util.Utils.LocalPathtoCloudPath(filename), sdt.Version);
                    if (gr != null && !gr.Succeed && gr.Fail == NetworkResults.GetTokenResult.FailType.OtherHolding)
                    {
                        MessageBox.Show(Properties.Resources.GetTokenFail + gr.TokenHolder.TokenHolder.getUserName(), "Delete Fail");
                        return;
                    }
                    if (MessageBox.Show(Properties.Resources.FileDeleteConfirm, "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Util.Utils.UnLockFile(filename);
                        new FileInfo(filename).Attributes = FileAttributes.Normal;
                        try
                        {
                            File.Delete(filename);
                        }
                        catch (IOException)
                        {
                            MessageBox.Show(Properties.Resources.DeleteFail, "Delete Fail");
                        }
                    }
                    else
                        return;
                }
            }
            else
            {
                if (item.Type.Equals("Folder"))
                {
                    if (MessageBox.Show(Properties.Resources.FolderDeleteConfirm, "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Util.Utils.DeleteFolder(filename);
                    }
                    else
                        return;
                }
                else
                {
                    if (MessageBox.Show(Properties.Resources.FileDeleteConfirm, "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        new FileInfo(filename).Attributes = FileAttributes.Normal;
                        try
                        {
                            File.Delete(filename);
                        }
                        catch (IOException)
                        {
                            MessageBox.Show(Properties.Resources.DeleteFail, "Delete Fail");
                        }
                    }
                    else
                        return;
                }
            }
        }
    }
}
