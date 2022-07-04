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

namespace DriveParts.UI.UC
{
    /// <summary>
    /// Логика взаимодействия для UCOrderItem.xaml
    /// </summary>
    public partial class UCOrderItem : UserControl
    {
        private Purchase _purchase = new Purchase();

        public UCOrderItem(Purchase purchase, Product product = null)
        {
            InitializeComponent();

            DrivePartsEntities data = DrivePartsEntities.GetContext();
            ComboProvider.ItemsSource = data.Provider.ToList();
            ProductImage image = null;
            
            if (product != null)
            {
                _purchase.ProductId = product.Id;
                _purchase.Price = product.Price;
                _purchase.Amount = 1;
                _purchase.Provider = data.Provider.Where(p => p.Code == "nal").FirstOrDefault();
                image = product.ProductImage.Where(i => i.Id == product.PreviewImageId).FirstOrDefault() ?? product.ProductImage.FirstOrDefault();
            }
            else if (purchase != null)
            {
                _purchase = purchase;
                image = purchase.Product.ProductImage.FirstOrDefault(i => i.Id == purchase.Product.PreviewImageId) ?? purchase.Product.ProductImage.FirstOrDefault();
            }

            if (image != null)
                Preview.Source = new BitmapImage(new Uri(Properties.Resources.ServerImagePath + image.ImagePath));

            TextSumPrice.Text = string.Format("{0:### ### ### ### ##0.00}", _purchase.Price * _purchase.Amount);
            DataContext = purchase;
        }

        public Purchase GetContext() => _purchase;

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            AddEditManager.RemoveProductFromOrder(_purchase, this);
        }

        private void UpdateSumPrice()
        {
            DrivePartsEntities data = DrivePartsEntities.GetContext();
            StringBuilder errors = new StringBuilder();

            if (!Regex.IsMatch(TbPrice.Text, @"^[\d]+[.]?[\d]*$"))
            {
                TbPrice.Undo();
                errors.AppendLine("Цена должна быть задана в числовом формате");
            }
            if (!Regex.IsMatch(TbPurchasePrice.Text, @"^[\d]+[.]?[\d]*$"))
            {
                TbPurchasePrice.Undo();
                errors.AppendLine("Закупочная цена должна быть задана в числовом формате");
            }
            if (!uint.TryParse(TbAmount.Text, out _))
            {
                TbAmount.Undo();
                errors.AppendLine("Количество должно быть задано в целочисленном формате");
            } 
            else if (int.Parse(TbAmount.Text) > _purchase.Product.Amount)
            {
                TbAmount.Undo();
                errors.AppendLine("Товар отсутствует в указанном количестве");
            }


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AddEditManager.SubTotal -= _purchase.Price * _purchase.Amount;
            AddEditManager.PurchaseTotal -= _purchase.PurchasePrice * _purchase.Amount;
            AddEditManager.SubTotal += decimal.Parse(TbAmount.Text) * decimal.Parse(TbPrice.Text.Replace(".", ","));
            AddEditManager.PurchaseTotal += decimal.Parse(TbAmount.Text) * decimal.Parse(TbPurchasePrice.Text.Replace(".", ","));
            AddEditManager.UpdateTotals();

            TextSumPrice.Text = string.Format("{0:### ### ### ### ##0.00}", decimal.Parse(TbAmount.Text) * decimal.Parse(TbPrice.Text.Replace(".", ",")));
        }

        private void TbAmount_LostFocus(object sender, RoutedEventArgs e) => UpdateSumPrice();

        private void TbPrice_LostFocus(object sender, RoutedEventArgs e) => UpdateSumPrice();

        private void TpPurchasePrice_LostFocus(object sender, RoutedEventArgs e) => UpdateSumPrice();
    }
}
