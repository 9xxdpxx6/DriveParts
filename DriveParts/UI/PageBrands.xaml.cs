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
    /// Логика взаимодействия для PageBrands.xaml
    /// </summary>
    public partial class PageBrands : Page
    {
        public PageBrands()
        {
            InitializeComponent();

            UpdateBrands();
        }

        private void BtnAddBrand_Click(object sender, RoutedEventArgs e) => Manager.MainFrame.Navigate(new PageAddEditBrand(null));

        private void UpdateBrands()
        {
            List<Brand> brands = DrivePartsEntities.GetContext().Brand.ToList();
            string keyword = TbSearch.Text.ToLower().Trim();
            ListBrands.Items.Clear();

            brands = brands.Where(b => b.BrandName.ToLower().Contains(keyword)).ToList();

            switch (ComboSortBy.SelectedIndex)
            {
                case 1:
                    brands = brands.OrderBy(b => b.BrandName).ToList();
                    break;
                case 2:
                    brands = brands.OrderByDescending(b => b.BrandName).ToList();
                    break;
                default:
                    brands = brands.OrderByDescending(b => b.Id).ToList();
                    break;
            }


            foreach (Brand brand in brands)
                ListBrands.Items.Add(new UC.UCBrand(brand));
        }

        private void ComboSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateBrands();

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e) => UpdateBrands();
        
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
                UpdateBrands();
        }
    }
}
