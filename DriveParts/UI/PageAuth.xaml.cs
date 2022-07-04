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

namespace DriveParts.UI
{
    /// <summary>
    /// Логика взаимодействия для PageAuth.xaml
    /// </summary>
    public partial class PageAuth : Page
    {
        public PageAuth()
        {
            InitializeComponent();
        }
        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            if (PbPass.Password.Length == 0)
                PbPass.Password = TbPassVisible.Text;
            if (AuthChecker.AuthCheck(TbLogin.Text, PbPass.Password))
            {
                Manager.MainFrame.Navigate(new PageProducts());
                TbLogin.Text = "";
                PbPass.Password = "";
                TbPassVisible.Text = "";
            }
        }

        private void TbLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TbLogin.Text.Length > 0)
            {
                PbPass.IsEnabled = true;
                TbPassVisible.IsEnabled = true;
                CbShowPass.IsEnabled = true;
            }
            else
            {
                PbPass.IsEnabled = false;
                TbPassVisible.IsEnabled = false;
                CbShowPass.IsEnabled = false;
            }
        }

        private void CbShowPass_Click(object sender, RoutedEventArgs e)
        {
            if (CbShowPass.IsChecked.Value)
                ShowPass();
            else
                HidePass();
        }

        /// <summary>
        /// Показывает пароль
        /// </summary>
        private void ShowPass()
        {
            TbPassVisible.Text = PbPass.Password;
            PbPass.Visibility = Visibility.Hidden;
            TbPassVisible.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Скрывает пароль
        /// </summary>
        private void HidePass()
        {
            PbPass.Password = TbPassVisible.Text;
            PbPass.Visibility = Visibility.Visible;
            TbPassVisible.Visibility = Visibility.Hidden;
        }

        private void PbPass_LostFocus(object sender, RoutedEventArgs e)
        {
            TbPassVisible.Text = PbPass.Password;
        }

        private void TbPassVisible_LostFocus(object sender, RoutedEventArgs e)
        {
            PbPass.Password = TbPassVisible.Text;
        }
    }
}
