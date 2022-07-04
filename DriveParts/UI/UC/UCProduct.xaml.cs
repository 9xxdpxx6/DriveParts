using System;
using System.Collections.Generic;
using System.IO;
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
using DriveParts.Entities;

namespace DriveParts.UI.UC
{
    /// <summary>
    /// Логика взаимодействия для UCProduct.xaml
    /// </summary>
    public partial class UCProduct : UserControl
    {
        public UCProduct(Product product)
        {
            InitializeComponent();

            DataContext = product;
            ProductImage image = product.ProductImage.Where(i => i.Id == product.PreviewImageId).ToList().FirstOrDefault() ?? product.ProductImage.FirstOrDefault();

            if (image != null && File.Exists(Properties.Resources.ServerImagePath + image.ImagePath))
                Preview.Source = new BitmapImage(new Uri(Properties.Resources.ServerImagePath + image.ImagePath));
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e) => Manager.MainFrame.Navigate(new PageAddEditProduct((sender as Button).DataContext as Product));

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            DrivePartsEntities data = DrivePartsEntities.GetContext();
            Product productForRemoving = (sender as Button).DataContext as Product;
            if (MessageBox.Show($"Вы точно хотите удалить товар {productForRemoving.ProductName}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (ProductImage image in productForRemoving.ProductImage.ToList())
                        data.ProductImage.Remove(image);
                    data.Product.Remove(productForRemoving);
                    data.SaveChanges();
                    new PageProducts().ListProducts.Items.Remove(new UCProduct(productForRemoving));
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
