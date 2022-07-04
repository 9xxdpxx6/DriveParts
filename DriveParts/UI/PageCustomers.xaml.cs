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
    /// Логика взаимодействия для PageCustomers.xaml
    /// </summary>
    public partial class PageCustomers : Page
    {
        public PageCustomers()
        {
            InitializeComponent();

            UpdateCustomers();
        }

        private void BtnAddCustomer_Click(object sender, RoutedEventArgs e) => Manager.MainFrame.Navigate(new PageAddEditCustomer(null));

        private void UpdateCustomers()
        {
            List<Customer> customers = DrivePartsEntities.GetContext().Customer.ToList();
            string keyword = TbSearch.Text.ToLower().Trim();
            ListCustomers.Items.Clear();

            customers = customers.Where(c => c.FullName.ToLower().Contains(keyword) || c.Phone.ToLower().Contains(keyword) || c.Email.ToLower().Contains(keyword)).ToList();

            switch (ComboSortBy.SelectedIndex)
            {
                case 1:
                    customers = customers.OrderBy(p => p.FullName).ToList();
                    break;
                case 2:
                    customers = customers.OrderByDescending(p => p.FullName).ToList();
                    break;
                default:
                    customers = customers.OrderByDescending(o => o.Id).ToList();
                    break;
            }


            foreach (Customer customer in customers)
                ListCustomers.Items.Add(new UC.UCCustomer(customer));
        }

        private void ComboSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateCustomers();

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e) => UpdateCustomers();

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
                UpdateCustomers();
        }
    }
}
