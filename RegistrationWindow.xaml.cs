using SunShimmer.Model;
using SunShimmer.Pages;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace SunShimmer
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CheckFields()))
            {
                MessageBox.Show(CheckFields());
                return;
            }
            if (!CheckEmail())
            {
                MessageBox.Show("Не корректный Email");
                return;
            }
            if (CheckPassword(PbPassword.Password) != "")
            {
                MessageBox.Show(CheckPassword(PbPassword.Password), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                User user = db.Users.FirstOrDefault(x => x.Email == TbEmail.Text);
                if (user != null)
                {
                    MessageBox.Show("Данный пользователь уже зарегистрирован");
                    return;
                }
                user = new User()
                {
                    Email = TbEmail.Text,
                    Password = PbPassword.Password,
                    Role = "Пользователь"
                };
                db.Users.Add(user);
                db.SaveChanges();
                MessageBox.Show("Пользователь зарегистрирован");
            }

            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                User user = db.Users.FirstOrDefault(x => x.Email == TbEmail.Text);
                MainWindow window = new MainWindow();
                MainWindow.UserId = user.UserId;
                window.Frame.Navigate(new ClientEditPage());
                window.Show();
                this.Hide();
            }
        }

        private string CheckFields()
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(TbEmail.Text)) message += "Введите логин" + Environment.NewLine;
            if (string.IsNullOrEmpty(PbPassword.Password)) message += "Введите пароль" + Environment.NewLine;
            if (string.IsNullOrEmpty(PbPasswordAccept.Password)) message += "Подтвердите пароль" + Environment.NewLine;
            else if (PbPassword.Password != PbPassword.Password) message += "Пароли не совпадают" + Environment.NewLine;
            return message;
        }

        private bool CheckEmail()
        {
            if (string.IsNullOrWhiteSpace(TbEmail.Text)) return true;
            else return Regex.IsMatch(TbEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private string CheckPassword(string password)
        {
            if ((password.Length < 6) || (password.ToLower() == password)
                || (!password.Any(char.IsDigit)))
                return "Длина пароля должна быть более 6 символов\n" +
                       "В пароле должна быть минимум одна прописная буква\n" +
                       "В пароле должна быть минимум одна цифра";
            return "";
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow window = new LoginWindow();
            window.Show();
            this.Hide();
        }
    }
}
