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
    /// Логика взаимодействия для PageGoods.xaml
    /// </summary>
    public partial class PageProducts : Page
    {
        public PageProducts()
        {
            InitializeComponent();

            UpdateProducts();
            DrivePartsEntities data = DrivePartsEntities.GetContext();

            List<Brand> allBrands = data.Brand.ToList();
            allBrands.Insert(0, new Brand { BrandName = "Все брэнды" });
            ComboBrand.ItemsSource = allBrands;

            List<Category> allCategories = data.Category.ToList();
            allCategories.Insert(0, new Category { CategoryName = "Все категории" });
            ComboCategory.ItemsSource = allCategories;
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e) => Manager.MainFrame.Navigate(new PageAddEditProduct(null));

        private void UpdateProducts()
        {
            List<Product> products = DrivePartsEntities.GetContext().Product.ToList();
            string keyword = TbSearch.Text.ToLower().Trim();
            ListProducts.Items.Clear();

            products = products.Where(p => p.ProductName.ToLower().Contains(keyword) || p.VendorCode.ToLower().Contains(keyword)).ToList();

            if (ComboBrand.SelectedIndex > 0)
                products = products.Where(p => p.Brand == ComboBrand.SelectedItem).ToList();
            if (ComboCategory.SelectedIndex > 0)
                products = products.Where(p => p.Category == ComboCategory.SelectedItem).ToList();

            switch (ComboSortBy.SelectedIndex)
            {
                case 1:
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                case 2:
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
                case 3:
                    products = products.OrderBy(p => p.ProductName).ToList();
                    break;
                case 4:
                    products = products.OrderByDescending(p => p.ProductName).ToList();
                    break;
                default:
                    products = products.OrderByDescending(p => p.Id).ToList();
                    break;
            }

            foreach (Product product in products)
                ListProducts.Items.Add(new UC.UCProduct(product));
        }

        private void ComboBrand_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateProducts();

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e) => UpdateProducts();

        private void ComboSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateProducts();

        private void ComboCategory_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateProducts();

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
                UpdateProducts();
        }
    }
}
