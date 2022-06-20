using SunShimmer.Model;
using System;
using System.Linq;
using System.Windows;

namespace SunShimmer
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CheckFields()))
            {
                MessageBox.Show(CheckFields());
                return;
            }

            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                User user = db.Users.FirstOrDefault(x => x.Email == TbEmail.Text);
                if (user == null || user.Password != PbPassword.Password)
                {
                    MessageBox.Show("Логин или пароль неверны");
                    return;
                }
                MainWindow window = new MainWindow(user.Role,user.UserId);
                window.Owner = this;
                window.Show();
                this.Hide();
            }
        }

        private string CheckFields()
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(TbEmail.Text)) message += "Введите логин" + Environment.NewLine;
            if (string.IsNullOrEmpty(PbPassword.Password)) message += "Введите пароль" + Environment.NewLine;
            return message;
        }

        private void BtnRegistration_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow window = new RegistrationWindow();
            window.Show();
            this.Hide();
        }
    }
}
