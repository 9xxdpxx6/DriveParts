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

namespace DriveParts.UI.UC
{
    /// <summary>
    /// Логика взаимодействия для UCProvider.xaml
    /// </summary>
    public partial class UCProvider : UserControl
    {
        public UCProvider(Provider provider)
        {
            InitializeComponent();

            DataContext = provider;
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e) => Manager.MainFrame.Navigate(new PageAddEditProvider((sender as Button).DataContext as Provider));

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            DrivePartsEntities data = DrivePartsEntities.GetContext();
            Provider providerForRemoving = (sender as Button).DataContext as Provider;
            if (MessageBox.Show($"Вы точно хотите удалить данные о поставщике {providerForRemoving.ProviderName}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    if (providerForRemoving.Purchase.Count > 0)
                    {
                        MessageBox.Show("Невозможно удалить объект из-за наличия ссылок на него в базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    data.Provider.Remove(providerForRemoving);
                    data.SaveChanges();
                    new PageProviders().ListProviders.Items.Remove(new UCProvider(providerForRemoving));
                    MessageBox.Show("Данные удалены", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            Manager.MainFrame.Refresh();
        }
    }
}
