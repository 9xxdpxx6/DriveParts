using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DriveParts.Entities
{
    class AddEditManager
    {
        public static WrapPanel WpProductImages { get; set; }
        public static ListView ListOrderItems { get; set; }

        public static List<ProductImage> ProductImages { get; set; }
        public static List<Purchase> OrderItems { get; set; }
        
        public static void AddImageToProduct(ProductImage image)
        {
            ProductImages.Add(image);
            WpProductImages.Children.Add(new UI.UC.UCImageButton(image));
        }
        public static void RemoveImageFromProduct(ProductImage image, UI.UC.UCImageButton imageBtn)
        {
            ProductImages.Remove(image);
            WpProductImages.Children.Remove(imageBtn);
        }

        public static void AddProductToOrder(Purchase purchase)
        {
            OrderItems.Add(purchase);
            ListOrderItems.Items.Add(new UI.UC.UCOrderItem(purchase));
        }
        public static void RemoveProductFromOrder(Purchase purchase, UI.UC.UCOrderItem orderItem)
        {
            OrderItems.Remove(purchase);
            ListOrderItems.Items.Remove(orderItem);
        }
    }
}
