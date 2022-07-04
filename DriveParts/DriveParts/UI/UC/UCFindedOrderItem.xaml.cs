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
    /// Логика взаимодействия для UCFindedOrderItem.xaml
    /// </summary>
    public partial class UCFindedOrderItem : UserControl
    {
        private Product _product;
        public UCFindedOrderItem(Product product)
        {
            InitializeComponent();

            ProductImage image = product.ProductImage.Where(i => i.Id == product.PreviewImageId).ToList().FirstOrDefault() ?? product.ProductImage.FirstOrDefault();

            if (image != null)
                Preview.Source = new BitmapImage(new Uri(image.ImagePath));

            DataContext = product;
            _product = product;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Just a border MouseUp event");
            Purchase purchase = new Purchase();
            //if (CurrentOrder != null)
            //    purchase.OrderId = CurrentOrder.Id;
            purchase.ProductId = _product.Id;
            purchase.Amount = 1;
            purchase.Price = _product.Price;
            AddEditManager.AddProductToOrder(purchase);
        }
    }
}
