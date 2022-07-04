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
    /// Логика взаимодействия для UCAddItemSearch.xaml
    /// </summary>
    public partial class UCAddItemSearch : UserControl
    {
        public UCAddItemSearch()
        {
            InitializeComponent();
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TbSearch.Text))
            {
                Popup.IsOpen = true;
                ListFindedItems.Items.Clear();
                List<Product> products = DrivePartsEntities.GetContext().Product.ToList();
                string keyword = TbSearch.Text.ToLower().Trim();
                products = products.Where(p => p.ProductName.ToLower().Contains(keyword) || p.VendorCode.ToLower().Contains(keyword)).ToList();
                if (products.Count > 0)
                {
                    TextNothingFound.Visibility = Visibility.Collapsed;
                    foreach (Product product in products)
                        ListFindedItems.Items.Add(new UCFindedOrderItem(product));
                }
                else
                    TextNothingFound.Visibility = Visibility.Visible;
            }
            else
                Popup.IsOpen = false;
        }

        private void TbSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            Popup.IsOpen = false;
            TbSearch.Text = "";
        }
    }
}
