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
    /// Логика взаимодействия для PageCategories.xaml
    /// </summary>
    public partial class PageCategories : Page
    {
        public PageCategories()
        {
            InitializeComponent();

            UpdateCategories();
        }

        private void BtnAddCategory_Click(object sender, RoutedEventArgs e) => Manager.MainFrame.Navigate(new PageAddEditCategory(null));

        private void UpdateCategories()
        {
            List<Category> categories = DrivePartsEntities.GetContext().Category.ToList();
            string keyword = TbSearch.Text.ToLower().Trim();
            ListCategories.Items.Clear();

            categories = categories.Where(c => c.CategoryName.ToLower().Contains(keyword)).ToList();

            switch (ComboSortBy.SelectedIndex)
            {
                case 1:
                    categories = categories.OrderBy(c => c.CategoryName).ToList();
                    break;
                case 2:
                    categories = categories.OrderByDescending(c => c.CategoryName).ToList();
                    break;
                default:
                    categories = categories.OrderByDescending(o => o.Id).ToList();
                    break;
            }


            foreach (Category category in categories)
                ListCategories.Items.Add(new UC.UCCategory(category));
        }


        private void ComboSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateCategories();

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e) => UpdateCategories();

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
                UpdateCategories();
        }
    }
}
