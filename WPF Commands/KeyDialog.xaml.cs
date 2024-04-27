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

namespace WPF_Commands
{
    /// <summary>
    /// Interaction logic for KeyDialog.xaml
    /// </summary>
    public partial class KeyDialog : Window
    {
        public enum LicenseType
        {
            Free,   // Без ключа и регистрации
            Trial,  // Trial версия
            Pro     // Pro версия
        }

        public LicenseType SelectedLicenseType { get; private set; }

        public KeyDialog()
        {
            InitializeComponent();
            RegisterBtn.IsEnabled = false;
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            // Здесь можно добавить логику проверки ключа и определения типа лицензии
            // Например, предположим, что ввод "pro" даёт полный доступ, "trial" - ограниченный доступ, иначе - только Exit.
            string enteredKey = KeyTxtBox.Text.ToLower();
            if (enteredKey == "pro")
            {
                SelectedLicenseType = LicenseType.Pro;
            }
            else if (enteredKey == "trial")
            {
                SelectedLicenseType = LicenseType.Trial;
            }
            else if (enteredKey != "pro" || enteredKey != "trial")
            {
                MessageBox.Show("Wrong key!");
                return;
            }
            else
            {
                SelectedLicenseType = LicenseType.Free;
            }

            DialogResult = true;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void KeyTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RegisterBtn.IsEnabled = KeyTxtBox.Text.Length >= 3;
        }
    }
}