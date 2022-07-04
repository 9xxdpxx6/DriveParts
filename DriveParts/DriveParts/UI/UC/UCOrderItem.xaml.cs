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
    /// Логика взаимодействия для UCOrderItem.xaml
    /// </summary>
    public partial class UCOrderItem : UserControl
    {
        //private Product _product = new Product();
        private Purchase _purchase = new Purchase();
        //private Order _order = new Order();

        public UCOrderItem(Product product) //Product or Purchase
        {
            InitializeComponent();

            DrivePartsEntities data = DrivePartsEntities.GetContext();
            ProductImage image = product.ProductImage.Where(i => i.Id == product.PreviewImageId).FirstOrDefault() ?? product.ProductImage.FirstOrDefault();
            
            if (image != null)
                Preview.Source = new BitmapImage(new Uri(image.ImagePath));

            ComboProvider.ItemsSource = data.Provider.ToList();

            if (product != null)
            {
                _purchase.ProductId = product.Id;
                _purchase.Price = product.Price;
                _purchase.Amount = 1;
                _purchase.Provider = data.Provider.Where(p => p.Code == "nal").FirstOrDefault();
            }
            UpdateSumPrice();
            //_purchase = product;
            //_order = order
            DataContext = product;
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            AddEditManager.RemoveProductFromOrder(_purchase, this);
        }

        private void ComboProvider_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void UpdateSumPrice()
        {
            decimal price = 0;
            int amount = 0;
            bool isDouble = decimal.TryParse(TbPrice.Text, out price);
            bool isInt = int.TryParse(TbAmount.Text, out amount);
            if (isDouble && isInt)
                TextSumPrice.Text = (price * amount).ToString();
        }

        private void TbPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSumPrice();
        }

        private void TbAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSumPrice();
        }

        private void TpPurchasePrice_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
