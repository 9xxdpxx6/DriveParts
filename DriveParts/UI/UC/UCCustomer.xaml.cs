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
    /// Логика взаимодействия для UCCustomer.xaml
    /// </summary>
    public partial class UCCustomer : UserControl
    {
        public UCCustomer(Customer customer)
        {
            InitializeComponent();

            DataContext = customer;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e) => Manager.MainFrame.Navigate(new PageAddEditCustomer((sender as Button).DataContext as Customer));

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            DrivePartsEntities data = DrivePartsEntities.GetContext();
            Customer customerForRemoving = (sender as Button).DataContext as Customer;
            if (MessageBox.Show($"Вы точно хотите удалить данные о клиенте {customerForRemoving.FullName}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    if (customerForRemoving.Order.Count > 0)
                    {
                        MessageBox.Show("Невозможно удалить объект из-за наличия ссылок на него в базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    data.Customer.Remove(customerForRemoving);
                    data.SaveChanges();
                    new PageProviders().ListProviders.Items.Remove(new UCCustomer(customerForRemoving));
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
