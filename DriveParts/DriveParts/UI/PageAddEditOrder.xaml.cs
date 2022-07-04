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
    /// Логика взаимодействия для PageAddEditOrder.xaml
    /// </summary>
    public partial class PageAddEditOrder : Page
    {
        private Order _order = new Order();
        private List<Purchase> _products = new List<Purchase>();

        public PageAddEditOrder(Entities.Order order)
        {
            InitializeComponent();

            AddEditManager.ListOrderItems = ListOrderItems;
            DrivePartsEntities data = DrivePartsEntities.GetContext();

            ComboStatus.ItemsSource = data.OrderStatus.ToList();

            if (order != null)
            {
                Title = "Редактирование товара";
                _order = order;
                tbSearch.CurrentOrder = order;
                //foreach (Product product in order.Purchase.ToList())
                //    _products.Add(product);
            }

            AddEditManager.OrderItems = _products;
            DataContext = _order;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DrivePartsEntities data = DrivePartsEntities.GetContext();
            StringBuilder errors = new StringBuilder();

            if (_order.Customer == null)
                errors.AppendLine("Укажите покупателя");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_order.Id == 0)
                data.Order.Add(_order);

            if (_products.Count > 0 || _products.Count != _order.Purchase.Count)
            {
                try
                {
                    foreach (Purchase oldOrderItem in _order.Purchase.ToList())
                    {
                        if (!_products.Contains(oldOrderItem))
                        {
                            data.Purchase.Remove(oldOrderItem);
                        }
                    }
                    foreach (Purchase newOrderItem in _products)
                        if (!_order.Purchase.Contains(newOrderItem))
                            data.Purchase.Add(newOrderItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось добавить товар\n" + ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

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

        private void ComboStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
                UpdateOrderItems();
        }
        private void UpdateOrderItems()
        {
            ListOrderItems.Items.Clear();
            List<Purchase> allOrderItems = DrivePartsEntities.GetContext().Purchase.Where(p => p.OrderId == _order.Id).ToList();
            foreach (Purchase purchase in allOrderItems)
                ListOrderItems.Items.Add(new UC.UCOrderItem(purchase));
        }
    }
}
