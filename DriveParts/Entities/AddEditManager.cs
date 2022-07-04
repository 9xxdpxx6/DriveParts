using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DriveParts.Entities
{
    class AddEditManager
    {
        public static WrapPanel WpProductImages { get; set; }
        public static ListView ListOrderItems { get; set; }

        public static List<ProductImage> ProductImagePats { get; set; }
        public static List<Purchase> OrderItems { get; set; }

        public static TextBlock TextPurchaseTotal { get; set; }
        public static TextBlock TextSubTotal { get; set; }

        public static decimal PurchaseTotal { get; set; }
        public static decimal SubTotal { get; set; }
        
        public static void AddImageToProduct(ProductImage imagePath)
        {
            ProductImagePats.Add(imagePath);
            WpProductImages.Children.Add(new UI.UC.UCImageButton(imagePath));
        }
        public static void RemoveImageFromProduct(ProductImage imagePath, UI.UC.UCImageButton imageBtn)
        {
            ProductImagePats.Remove(imagePath);
            WpProductImages.Children.Remove(imageBtn);
        }

        public static void AddProductToOrder(Purchase purchase)
        {
            OrderItems.Add(purchase);
            ListOrderItems.Items.Add(new UI.UC.UCOrderItem(purchase));
            UpdateTotals(purchase, false);
        }
        public static void RemoveProductFromOrder(Purchase purchase, UI.UC.UCOrderItem orderItem)
        {
            UpdateTotals(purchase, true);
            OrderItems.Remove(purchase);
            ListOrderItems.Items.Remove(orderItem);
        }

        public static void UpdateTotals()
        {
            TextPurchaseTotal.Text = string.Format("{0:### ### ### ### ##0.00} руб", PurchaseTotal);
            TextSubTotal.Text = string.Format("{0:### ### ### ### ##0.00} руб", SubTotal);
        }

        public static void UpdateTotals(Purchase purchase, bool removable)
        {
            int amount = purchase.Amount;
            decimal price = purchase.Price;
            decimal purchasePrice = purchase.PurchasePrice;
            if (removable)
            {
                price *= -1;
                purchasePrice *= -1;
            }

            SubTotal += price * amount;
            PurchaseTotal += purchasePrice * amount;
            TextPurchaseTotal.Text = string.Format("{0:### ### ### ### ##0.00} руб", PurchaseTotal);
            TextSubTotal.Text = string.Format("{0:### ### ### ### ##0.00} руб", SubTotal);
        }

        public static void UpdateTotals(Order order)
        {
            SubTotal = 0;
            PurchaseTotal = 0;
            foreach (Purchase purchase in order.Purchase)
            {
                int amount = purchase.Amount;
                decimal price = purchase.Price;
                decimal purchasePrice = purchase.PurchasePrice;
            
                SubTotal += price * amount;
                PurchaseTotal += purchasePrice * amount;
                TextPurchaseTotal.Text = string.Format("{0:### ### ### ### ##0.00} руб", PurchaseTotal);
                TextSubTotal.Text = string.Format("{0:### ### ### ### ##0.00} руб", SubTotal);
            }
        }
    }
}
