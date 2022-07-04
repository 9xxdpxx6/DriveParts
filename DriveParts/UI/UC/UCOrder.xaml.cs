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
    /// Логика взаимодействия для UCOrder.xaml
    /// </summary>
    public partial class UCOrder : UserControl
    {
        private Order _order;
        public Visibility IsPaid { get; set; } = Visibility.Hidden;

        public UCOrder(Order order)
        {
            InitializeComponent();

            //IsPaid = order.Paid ? Visibility.Visible : Visibility.Hidden;
            DataContext = order;
            _order = order;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageAddEditOrder((sender as Button).DataContext as Order));
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            DrivePartsEntities data = DrivePartsEntities.GetContext();
            Order orderForRemoving = (sender as Button).DataContext as Order;
            if (MessageBox.Show($"Вы точно хотите удалить заказ № {orderForRemoving.Id}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (Purchase purchase in orderForRemoving.Purchase.ToList())
                        data.Purchase.Remove(purchase);
                    data.Order.Remove(orderForRemoving);
                    data.SaveChanges();
                    new PageProducts().ListProducts.Items.Remove(new UCOrder(orderForRemoving));
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
