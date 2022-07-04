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
    /// Логика взаимодействия для PageOrders.xaml
    /// </summary>
    public partial class PageOrders : Page
    {
        public PageOrders()
        {
            InitializeComponent();

            //Order order = new Order();
            //order.Id = 13131;
            //order.StatusId = 2;
            //order.Paid = false;
            //order.Date = DateTime.Today;
            //order.PaymentDate = DateTime.Today;
            //order.TotalPrice = 1488;
            //ListOrders.Items.Add(new UC.UCOrder(order));
            //order.Paid = false;
            //ListOrders.Items.Add(new UC.UCOrder(order));

            DrivePartsEntities data = DrivePartsEntities.GetContext();
            foreach (Order order in data.Order.OrderByDescending(o => o.Id).ToList())
                ListOrders.Items.Add(new UC.UCOrder(order));

            List<OrderStatus> allStatuses = data.OrderStatus.ToList();
            allStatuses.Insert(0, new OrderStatus { StatusName = "Все" });
            ComboStatus.ItemsSource = allStatuses;
        }

        private void BtnAddOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateOrders()
        {
            List<Order> orders = DrivePartsEntities.GetContext().Order.ToList();
            string keyword = TbSearch.Text.ToLower().Trim();
            ListOrders.Items.Clear();

            ///     fix code below
            ///
            orders = orders.Where(o => o.Customer.FullName.ToLower().Contains(keyword) || o.Customer.Phone.ToLower().Contains(keyword) || o.Customer.Email.ToLower().Contains(keyword) || o.VIN.ToLower().Contains(keyword) || new Product().ProductName.ToLower().Contains(keyword)).ToList();

            if (ComboStatus.SelectedIndex > 0)
                orders = orders.Where(o => o.OrderStatus == ComboStatus.SelectedItem).ToList();
            if (DpFrom.SelectedDate != null)
                orders = orders.Where(o => o.Date >= DpFrom.SelectedDate).ToList();
            if (DpTo.SelectedDate != null)
                orders = orders.Where(o => o.Date <= DpTo.SelectedDate).ToList();

            orders = orders.OrderByDescending(o => o.Id).ToList();

            foreach (Order order in orders)
                ListOrders.Items.Add(new UC.UCOrder(order));
        }

        private void ComboStatus_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateOrders();

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e) => UpdateOrders();

        private void DpFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e) => UpdateOrders();

        private void DpTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e) => UpdateOrders();

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
                UpdateOrders();
        }
    }
}
