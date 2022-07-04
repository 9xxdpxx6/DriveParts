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
using DriveParts.Entities;
using DriveParts.UI;

namespace DriveParts
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class WindowBase : Window
    {
        public WindowBase()
        {
            InitializeComponent();
            FrameBase.Navigate(new PageAuth());
            Manager.MainFrame = FrameBase;
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (FrameBase.CanGoBack)
                Manager.MainFrame.GoBack();
        }

        private void FrameBase_ContentRendered(object sender, EventArgs e)
        {
            if (FrameBase.Content is PageAuth)
            {
                GridSidePanel.Visibility = Visibility.Hidden;
                FrameBase.Padding = new Thickness(0, 0, 0, 0);
            }
            else
            {
                GridSidePanel.Visibility = Visibility.Visible;
                FrameBase.Padding = new Thickness(80, 0, 0, 0);
            }

            if (FrameBase.CanGoBack)
                BtnBack.Visibility = Visibility.Visible;
            else
                BtnBack.Visibility = Visibility.Hidden;
        }
        private void SpMenuItemProducts_MouseUp(object sender, MouseButtonEventArgs e) => Manager.MainFrame.Navigate(new PageProducts());

        private void SpMenuItemOrders_MouseUp(object sender, MouseButtonEventArgs e) => Manager.MainFrame.Navigate(new PageOrders());

        private void SpMenuItemCustomers_MouseUp(object sender, MouseButtonEventArgs e) => Manager.MainFrame.Navigate(new PageCustomers());

        private void SpMenuItemBrands_MouseUp(object sender, MouseButtonEventArgs e) => Manager.MainFrame.Navigate(new PageBrands());

        private void SpMenuItemProviders_MouseUp(object sender, MouseButtonEventArgs e) => Manager.MainFrame.Navigate(new PageProviders());

        private void SpMenuItemCategories_MouseUp(object sender, MouseButtonEventArgs e) => Manager.MainFrame.Navigate(new PageCategories());

        private void SpMenuItemSettings_MouseUp(object sender, MouseButtonEventArgs e) => Manager.MainFrame.Navigate(new PageSettings());

        private void SpMenuItemSignOut_MouseUp(object sender, MouseButtonEventArgs e) 
        {
            while (Manager.MainFrame.CanGoBack)
                Manager.MainFrame.GoBack();
        }
    }
}
