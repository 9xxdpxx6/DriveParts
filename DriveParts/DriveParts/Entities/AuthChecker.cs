using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Security.Cryptography;

namespace DriveParts.Entities
{
    /// <summary>
    /// AuthChecer предназначен работы авторизации
    /// </summary>
    class AuthChecker
    {
        public static bool AuthCheck(string login, string pass)
        {
            try
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
                else if (pass == DrivePartsEntities.GetContext().User.Where(user => user.NickName.Equals(login)).First().Password)
                {
                    MessageBox.Show("Авторизация прошла успешно", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
