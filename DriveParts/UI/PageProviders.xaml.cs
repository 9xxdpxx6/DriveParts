using DriveParts.Entities;
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

namespace DriveParts.UI
{
    /// <summary>
    /// Логика взаимодействия для PageProviders.xaml
    /// </summary>
    public partial class PageProviders : Page
    {
        public PageProviders()
        {
            InitializeComponent();

            UpdateProviders();
        }

        private void BtnAddProvider_Click(object sender, RoutedEventArgs e) => Manager.MainFrame.Navigate(new PageAddEditProvider(null));

        private void UpdateProviders()
        {
            List<Provider> providers = DrivePartsEntities.GetContext().Provider.ToList();
            string keyword = TbSearch.Text.ToLower().Trim();
            ListProviders.Items.Clear();

            providers = providers.Where(p => p.ProviderName.ToLower().Contains(keyword) || p.Code.ToLower().Contains(keyword)).ToList();

            switch (ComboSortBy.SelectedIndex)
            {
                case 1:
                    providers = providers.OrderBy(p => p.ProviderName).ToList();
                    break;
                case 2:
                    providers = providers.OrderByDescending(p => p.ProviderName).ToList();
                    break;
                default:
                    providers = providers.OrderByDescending(p => p.Id).ToList();
                    break;
            }


            foreach (Provider provider in providers)
                ListProviders.Items.Add(new UC.UCProvider(provider));
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e) => UpdateProviders();

        private void ComboSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateProviders();

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
                UpdateProviders();
        }
    }
}
