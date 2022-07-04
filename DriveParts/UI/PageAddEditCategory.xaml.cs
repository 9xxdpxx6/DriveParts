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

namespace DriveParts.UI
{
    /// <summary>
    /// Логика взаимодействия для PageAddEditCategory.xaml
    /// </summary>
    public partial class PageAddEditCategory : Page
    {
        private Category _category = new Category();

        public Category Parent { get; set; }

        public PageAddEditCategory(Category category)
        {
            InitializeComponent();

            DrivePartsEntities data = DrivePartsEntities.GetContext();
            List<Category> allCategories = data.Category.ToList();
            allCategories.Insert(0, new Category{ CategoryName = "Родительская категория", ParentId = 0 });
            ComboParentCategory.ItemsSource = allCategories;

            if (category != null)
            {
                Title = "Редактирование категории";
                if (category.ParentId > 0)
                {
                    Parent = data.Category.Where(c => c.Id == category.ParentId).FirstOrDefault();
                    ComboParentCategory.SelectedItem= Parent;
                }
                _category = category;
            }

            DataContext = _category;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DrivePartsEntities data = DrivePartsEntities.GetContext();
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_category.CategoryName))
                errors.AppendLine("Укажите наименование категории");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_category.Id == 0)
                data.Category.Add(_category);

            if (Parent != null)
            {
                _category.ParentId = Parent.Id;
                var entity = data.Category.Find(_category.Id);
                if (entity == null)
                    return;
                data.Entry(entity).CurrentValues.SetValues(_category);
            }

            try
            {
                data.SaveChanges();
                MessageBox.Show("Информация сохранена", "", MessageBoxButton.OK, MessageBoxImage.Information);
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ComboParentCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Parent = ComboParentCategory.SelectedItem as Category;
        }
    }
}
