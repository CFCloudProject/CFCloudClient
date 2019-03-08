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
    /// InputWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InputWindow : Window
    {
        public string InputText;

        public InputWindow(string message, string title)
        {
            InitializeComponent();
            this.Title = title;
            Input.Text = message;
        }

        public string getInput()
        {
            bool? result = ShowDialog();
            if (result.HasValue && result.Value)
                return InputText;
            else
                return null;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            InputText = InputBox.Text;
            if (!InputText.Equals(""))
                DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
