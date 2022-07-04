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
                ImageSource = new BitmapImage(new Uri(image.ImagePath));

            if (product != null)
                _productOfImage = product;
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            AddEditManager.RemoveImageFromProduct(_image, this);
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image |*.png; *.jpg; *.jpeg";
            ProductImage productImage = new ProductImage();
            if (dialog.ShowDialog().Value)
            {
                //productImage.ImagePath = "D:/Frolov/DriveParts/DriveParts/img/brands/" + dialog.SafeFileName;
                productImage.ImagePath = "C:/Users/User/Pictures/okna/" + dialog.SafeFileName;
                if (_productOfImage.Id != 0)
                    productImage.ProductId = _productOfImage.Id;
                AddEditManager.AddImageToProduct(productImage);
            }
        }
    }
}
