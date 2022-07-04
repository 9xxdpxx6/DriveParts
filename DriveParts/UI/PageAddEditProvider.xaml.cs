using DriveParts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace DriveParts.UI
{
    /// <summary>
    /// Логика взаимодействия для PageAddEditProvider.xaml
    /// </summary>
    public partial class PageAddEditProvider : Page
    {
        private Provider _provider = new Provider();

        public PageAddEditProvider(Provider provider)
        {
            InitializeComponent();

            if (provider != null)
            {
                Title = "Редактирование поставщика";
                _provider = provider;
            }

            DataContext = _provider;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DrivePartsEntities data = DrivePartsEntities.GetContext();
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_provider.ProviderName))
                errors.AppendLine("Укажите наименование поставщика");
            if (!Regex.IsMatch(TbCode.Text, @"^[A-Z]{3}$"))
                errors.AppendLine("Код поставщика должен быть задам тремя заглавными буквами латинского алфавита");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_provider.Id == 0)
                data.Provider.Add(_provider);

            try
            {
                data.SaveChanges();
                MessageBox.Show("Информация сохранена", "", MessageBoxButton.OK, MessageBoxImage.Information);
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
