using PromptDialog;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Commands
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
                KeyDialog.LicenseType licenseType = dialog.SelectedLicenseType;

                switch (licenseType)
                {
                    case KeyDialog.LicenseType.Free:
                        // Без ключа и регистрации: доступ только к Exit
                        // Настроить меню соответственно
                        DisableAllMenuItemsExceptExit();
                        break;

                    case KeyDialog.LicenseType.Trial:
                        // Trial версия: доступ к функциям из меню File (New, Open, Save, Exit)
                        // Настроить меню соответственно
                        DisableEditMenuItems();
                        break;

                    case KeyDialog.LicenseType.Pro:
                        // Pro версия: доступ ко всем функциям из меню File и Edit
                        // Настроить меню соответственно
                        EnableAllMenuItems();
                        break;
                }
            }
            else
            {
                // Если диалог был закрыт без ввода ключа, по умолчанию предоставить доступ только к Exit
                DisableAllMenuItemsExceptExit();
            }
        }

        private void DisableAllMenuItemsExceptExit()
        {
            // Отключить все элементы меню кроме Exit
            foreach (object item in menuFile.Items)
            {
                if (item is MenuItem menuItem && menuItem.Header.ToString() != "Exit")
                menuItem.IsEnabled = false;
            }
            foreach (MenuItem menuItem in menuEdit.Items)
            {
                menuItem.IsEnabled = false;
            }
        }

        private void DisableEditMenuItems()
        {
            // Отключить элементы меню из раздела Edit
            foreach (MenuItem menuItem in menuEdit.Items)
            {
                menuItem.IsEnabled = false;
            }
        }

        private void EnableAllMenuItems()
        {
            // Включить все элементы меню
            foreach (MenuItem menuItem in menuFile.Items)
            {
                menuItem.IsEnabled = true;
            }
            foreach (MenuItem menuItem in menuEdit.Items)
            {
                menuItem.IsEnabled = true;
            }
        }


        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            isChanged = false;
            Close();
        }

        private void SaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isChanged;
        }

        private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            isChanged = false;
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isChanged;
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            isChanged = false;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            isChanged = false;
        }

        private void Create_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            isChanged = false;
        }

        private bool isChanged = false;
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanged = true;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            KeyDialog dialog = new KeyDialog();
            dialog.Show();
        }
    }
}