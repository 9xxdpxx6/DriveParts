using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DBFramework
{
    public class AuthChecker
    {
        public static string Role { get; set; }

        public static bool AuthCheck(string login, string pass)
        {
            if (login == null || login == "")
            {
                MessageBox.Show("Введите логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (pass == null || pass == "")
            {
                MessageBox.Show("Введите пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (login == "admin" && pass == "admin")
            {
                MessageBox.Show("Авторизация прошла успешно", "", MessageBoxButton.OK, MessageBoxImage.Information);
                Role = "Admin";
                return true;
            }
            else if (login == "manager" && pass == "manager")
            {
                MessageBox.Show("Авторизация прошла успешно", "", MessageBoxButton.OK, MessageBoxImage.Information);
                Role = "Manager";
                return true;
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
