using DriveParts.Entities;
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

namespace DriveParts.UI.UC
{
    /// <summary>
    /// Логика взаимодействия для UCBrand.xaml
    /// </summary>
    public partial class UCBrand : UserControl
    {
        public UCBrand(Brand brand)
        {
            InitializeComponent();

            DataContext = brand;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e) => Manager.MainFrame.Navigate(new PageAddEditBrand((sender as Button).DataContext as Brand));

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            DrivePartsEntities data = DrivePartsEntities.GetContext();
            Brand brandForRemoving = (sender as Button).DataContext as Brand;
            if (MessageBox.Show($"Вы точно хотите удалить брэнд {brandForRemoving.BrandName}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    if (brandForRemoving.Product.Count > 0)
                    {
                        MessageBox.Show("Невозможно удалить объект из-за наличия ссылок на него в базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    data.Brand.Remove(brandForRemoving);
                    data.SaveChanges();
                    new PageProviders().ListProviders.Items.Remove(new UCBrand(brandForRemoving));
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
