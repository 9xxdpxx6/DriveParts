using DriveParts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для PageAddEditProduct.xaml
    /// </summary>
    public partial class PageAddEditProduct : Page
    {
        private Product _product = new Product();
        private List<ProductImage> _images = new List<ProductImage>();

        public PageAddEditProduct(Product product)
        {
            InitializeComponent();

            AddEditManager.WpProductImages = WpImages;
            DrivePartsEntities data = DrivePartsEntities.GetContext();

            ComboBrand.ItemsSource = data.Brand.ToList();
            ComboCategory.ItemsSource = data.Category.ToList();
            
            if (product != null)
            {
                Title = "Редактирование товара";
                _product = product;
                TbtnAvailable.IsChecked = product.Available;
                foreach (ProductImage image in product.ProductImage.ToList())
                    _images.Add(image);
            }
            else
            {
                TbtnAvailable.IsChecked = true;
                WpImages.Children.Add(new UC.UCImageButton(null));
            }

            AddEditManager.ProductImages = _images;
            DataContext = _product;
        }
        
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DrivePartsEntities data = DrivePartsEntities.GetContext();
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_product.ProductName))
                errors.AppendLine("Укажите название товара");
            if (_product.Category == null)
                errors.AppendLine("Выберите категорию");
            if (_product.Brand == null)
                errors.AppendLine("Выберите брэнд");
            if (string.IsNullOrWhiteSpace(_product.VendorCode))
                errors.AppendLine("Укажите артикул");
            if (!Regex.IsMatch(TbPrice.Text, @"^[\d]+[.]?[\d]*$"))
                errors.AppendLine("Цена должна быть задана в числовом формате");
            if (_product.Price < 0)
                errors.AppendLine("Цена не может быть отрицательной");
            if (_product.Amount < 0)
                errors.AppendLine("Количество не может быть отрицательным");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_product.Id == 0)
                data.Product.Add(_product);

            if (_images.Count > 0 || _images.Count != _product.ProductImage.Count)
            {
                try
                {
                    foreach (ProductImage oldImage in _product.ProductImage.ToList())
                    {
                        if (!_images.Contains(oldImage))
                        {
                            data.ProductImage.Remove(oldImage);
                        }
                    }
                    foreach (ProductImage newImage in _images)
                        if (!_product.ProductImage.Contains(newImage))
                            data.ProductImage.Add(newImage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось добавить изображение\n" + ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if (_images.Count > 0 || _product.ProductImage.Count > 0)
                _product.PreviewImageId = _images.FirstOrDefault().Id;

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

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
                UpdateImages();
        }
        private void UpdateImages()
        {
            WpImages.Children.Clear();
            List<ProductImage> allImages = _product.ProductImage.ToList();
            foreach (ProductImage image in allImages)
                WpImages.Children.Add(new UC.UCImageButton(image));
            WpImages.Children.Insert(0, new UC.UCImageButton(null, _product));
        }

        private void TbPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TbPrice.Text.Contains(","))
            {
                TbPrice.Text = TbPrice.Text.Replace(",", ".");
                TbPrice.Select(TbPrice.Text.Length, 0);
            }
        }
    }
}
