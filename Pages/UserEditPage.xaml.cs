using SunShimmer.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SunShimmer.Pages
{
    public partial class UserEditPage : Page
    {
        public UserEditPage()
        {
            InitializeComponent();
        }
        User user = null;

        public UserEditPage(int id)
        {
            InitializeComponent();
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                user = db.Users.FirstOrDefault(x => x.UserId == id);
            }
            LoadData();
        }

        private void LoadData()
        {
            TbEmail.Text = user.Email;
            TbPass.Text = user.Password;
            CbRole.Text = user.Role;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CheckFields()))
            {
                MessageBox.Show(CheckFields());
                return;
            }
            if (user == null) Add();
            else Update();
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new UserAllPage();
        }

        private void Add()
        {
            try
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    User user = db.Users.FirstOrDefault(x => x.Email == TbEmail.Text);
                    if (user != null)
                    {
                        MessageBox.Show("Этот пользователь уже существует");
                        return;
                    }

                    user = new User()
                    {
                        Email = TbEmail.Text,
                        Password = TbPass.Text,
                        Role = CbRole.Text,
                    };
                    db.Users.Add(user);
                    db.SaveChanges();
                    MessageBox.Show("Запись добавлена");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void Update()
        {
            try
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    User user1 = db.Users.FirstOrDefault(x => x.UserId == user.UserId);
                    db.Users.Attach(user1);
                    user1.Email = TbEmail.Text;
                    user1.Password = TbPass.Text;
                    user1.Role = CbRole.Text;
                    db.SaveChanges();
                    MessageBox.Show("Запись обновлена");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private string CheckFields()
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(TbEmail.Text)) message += "Введите логин" + Environment.NewLine;
            if (string.IsNullOrWhiteSpace(TbPass.Text)) message += "Введите пароль" + Environment.NewLine;
            if (CbRole.SelectedIndex == -1) message += "Выберите роль" + Environment.NewLine;
            return message;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new UserAllPage();
        }
    }
}