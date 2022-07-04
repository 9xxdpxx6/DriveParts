using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DriveParts.Entities
{
    class DBInteraction
    {
        public static void RemoveImage(WrapPanel container, object DataContext)
        {
            var imageForRemoving = DataContext as ProductImage;
            if (MessageBox.Show($"Вы точно хотите удалить изображение?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    //DrivePartsEntities.GetContext().ProductImage.Remove(imageForRemoving);
                    //DrivePartsEntities.GetContext().SaveChanges();
                    container.Children.Remove(new UI.UC.UCImageButton(imageForRemoving));
                    MessageBox.Show("Данные удалены!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
        public static void AddImage(List<ProductImage> list, ProductImage image)
        {
            list.Add(image);
        }
    }
}
