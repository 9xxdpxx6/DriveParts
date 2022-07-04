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
    /// Логика взаимодействия для UCCategory.xaml
    /// </summary>
    public partial class UCCategory : UserControl
    {
        public UCCategory(Category category)
        {
            InitializeComponent();

            DataContext = category;
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e) => Manager.MainFrame.Navigate(new PageAddEditCategory((sender as Button).DataContext as Category));

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            DrivePartsEntities data = DrivePartsEntities.GetContext();
            Category categoryForRemoving = (sender as Button).DataContext as Category;
            if (MessageBox.Show($"Вы точно хотите удалить категорию {categoryForRemoving.CategoryName}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (Category category in data.Category.ToList())
                    {
                        if (category.ParentId == categoryForRemoving.Id)
                        {
                            MessageBox.Show("Невозможно удалить объект из-за наличия ссылок на него в базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    if (categoryForRemoving.Product.Count > 0)
                    {
                        MessageBox.Show("Невозможно удалить объект из-за наличия ссылок на него в базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    data.Category.Remove(categoryForRemoving);
                    data.SaveChanges();
                    new PageProviders().ListProviders.Items.Remove(new UCCategory(categoryForRemoving));
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
