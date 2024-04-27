using Microsoft.Win32;
using PromptDialog;
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

namespace WPF_Commands
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KeyDialog.LicenseType currentLicense;

        public MainWindow()
        {
            InitializeComponent();
            CheckLicenseType();

            CommandBinding command;
            command = new CommandBinding(ApplicationCommands.New);
            command.Executed += Create_Executed;
            CommandBindings.Add(command);
            
            command = new CommandBinding(ApplicationCommands.Open);
            command.Executed += Open_Executed;
            CommandBindings.Add(command);

            command = new CommandBinding(ApplicationCommands.Save);
            command.Executed += Save_Executed;
            command.CanExecute += Save_CanExecute;
            CommandBindings.Add(command);

            command = new CommandBinding(ApplicationCommands.SaveAs);
            command.Executed += SaveAs_Executed;
            command.CanExecute += SaveAs_CanExecute;
            CommandBindings.Add(command);

            command = new CommandBinding(ApplicationCommands.Close);
            command.Executed += Exit_Executed;
            CommandBindings.Add(command);
        }

        private void CheckLicenseType()
        {
            KeyDialog dialog = new KeyDialog();
            if (dialog.ShowDialog() == true)
            {
                currentLicense = dialog.SelectedLicenseType;
                //KeyDialog.LicenseType licenseType = dialog.SelectedLicenseType;
                switch (currentLicense)
                {
                    case KeyDialog.LicenseType.Free:
                        // Без ключа и регистрации: доступ только к редактору
                        // Настроить меню соответственно
                        EnableOnlyWrite();
                        break;

                    case KeyDialog.LicenseType.Trial:
                        // Trial версия: доступ к функциям из меню File (New, Open, Save, Exit)
                        // Настроить меню соответственно
                        EnableFileFeatures();
                        break;

                    case KeyDialog.LicenseType.Pro:
                        // Pro версия: доступ ко всем функциям из меню File и Edit
                        // Настроить меню соответственно
                        EnableAllFeatures();
                        break;
                }
            }
            else
            {
                // Если диалог был закрыт без ввода ключа, по умолчанию предоставить доступ только к редактору 
                EnableOnlyWrite();
            }
        }

        private void EnableOnlyWrite()
        {
            // Отключить все элементы меню кроме Exit
            foreach (object item in menuFile.Items)
            {
                if (item is MenuItem menuItem && menuItem.Header.ToString() != "Exit")
                {
                    menuItem.IsEnabled = false;
                }
            }
            foreach (MenuItem menuItem in menuEdit.Items)
            {
                menuItem.IsEnabled = false;
            }
        }

        private void EnableFileFeatures()
        {
            // Отключить все элементы меню кроме Exit
            foreach (object item in menuFile.Items)
            {
                if (item is MenuItem menuItem)
                {
                    menuItem.IsEnabled = true;
                }
            }
            // Отключить элементы меню из раздела Edit
            foreach (MenuItem menuItem in menuEdit.Items)
            {
                menuItem.IsEnabled = false;
            }
        }

        private void EnableAllFeatures()
        {
            // Включить все элементы меню
            foreach (object item in menuFile.Items)
            {
                if (item is MenuItem menuItem)
                {
                    menuItem.IsEnabled = true;
                }
            }
            foreach (MenuItem menuItem in menuEdit.Items)
            {
                menuItem.IsEnabled = true;
            }
        }

        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void SaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isChanged;
        }

        private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog { Filter = "Text Files | *.txt", DefaultExt = "txt" };
            bool? success = dialog.ShowDialog();
            if (success == true)
            {
                string path = dialog.FileName;
                string fileContent = txtEditor.Text;
                File.WriteAllText(path, fileContent);
            }
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isChanged;
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e) {}

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            isChanged = false;
            OpenFileDialog dialog = new OpenFileDialog { Filter = "Text Files | *.txt", DefaultExt = "txt" };
            bool? success = dialog.ShowDialog();
            if (success == true)
            {
                string path = dialog.FileName;
                string fileContent = File.ReadAllText(path);
                txtEditor.Text = fileContent;
            }
        }

        private void Create_Executed(object sender, ExecutedRoutedEventArgs e) {}

        private bool isChanged = false;
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanged = true;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            CheckLicenseType();
        }
    }
}