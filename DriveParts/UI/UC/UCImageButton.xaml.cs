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
using System.IO;
using Microsoft.Win32;
using System.Security.AccessControl;

namespace DriveParts.UI.UC
{
    /// <summary>
    /// Логика взаимодействия для UCImageButton.xaml
    /// </summary>
    public partial class UCImageButton : UserControl
    {
        private ProductImage _image;
        private Product _productOfImage;

        public ImageSource ImageSource { get; set; }
        public Visibility IsEmpty => ImageSource != null ? Visibility.Visible : Visibility.Hidden;

        public UCImageButton(ProductImage image, Product product = null)
        {
            InitializeComponent();

            _image = image;
            DataContext = this;

            if (image != null)
            {
                ImageSource = new BitmapImage(new Uri(Properties.Resources.ServerImagePath + image.ImagePath));
            }

            if (product != null)
                _productOfImage = product;
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            AddEditManager.RemoveImageFromProduct(_image, this);
            try
            {
                string path = Properties.Resources.ServerImagePath + _image.ImagePath;
                _image = null;
                ImageSource = null;
                File.Delete(path);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image |*.png; *.jpg; *.jpeg";
            ProductImage productImage = new ProductImage();
            if (dialog.ShowDialog().Value)
            {
                try
                {
                    //  D:/Frolov/DriveParts/DriveParts/img/products/
                    //  G:\Programs\C#\WPF\DriveParts\img\products/
                    File.Copy(dialog.FileName, Properties.Resources.ServerImagePath + dialog.SafeFileName);
                    productImage.ImagePath = dialog.SafeFileName;
                    if (_productOfImage.Id != 0)
                        productImage.ProductId = _productOfImage.Id;
                    AddEditManager.AddImageToProduct(productImage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось добавить изображение\n" + ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }
    }
}
