using DriveParts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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

namespace DriveParts.UI
{
    /// <summary>
    /// Логика взаимодействия для PageAddEditCustomer.xaml
    /// </summary>
    public partial class PageAddEditCustomer : Page
    {
        private Customer _customer = new Customer();

        public PageAddEditCustomer(Customer customer)
        {
            InitializeComponent();

            if (customer != null)
            {
                Title = "Редактирование клиента";
                _customer = customer;
            }

            DataContext = _customer;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DrivePartsEntities data = DrivePartsEntities.GetContext();
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_customer.FullName))
                errors.AppendLine("Укажите полное имя клиента");
            if (!Regex.IsMatch(TbPhone.Text, @"^[\d]{7,11}$"))
                errors.AppendLine("Телефон должен быть задан арабскими цифрами без тире и пробелов и составлять от 7 до 11 символов");
            if (!IsEmailValid(TbEmail.Text))
                errors.AppendLine("E-mail должен быть задан в формате: example@server.domen");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_customer.Id == 0)
                data.Customer.Add(_customer);

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

        private bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return true;
            try
            {
                MailAddress address = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
