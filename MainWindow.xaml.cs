using SunShimmer.Model;
using SunShimmer.Models;
using SunShimmer.Pages;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SunShimmer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static public string Role;
        static public int UserId;

        public MainWindow(string role, int userId)
        {
            InitializeComponent();
            Role = role;
            UserId = userId;
            Frame.Content = new MainPage();
            Frame.Navigate(new MainPage());
            Manager.Frame = Frame;
            if (MainWindow.Role == "Админ") BtnAdmin.Visibility = Visibility.Visible;
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult x = MessageBox.Show("Вы действительно хотите выйти?",
                "Выйти", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (x == MessageBoxResult.Cancel)
                e.Cancel = true;
        }

        private void BtnMainPage_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new MainPage();
        }

        private void BtnApplication_Click(object sender, RoutedEventArgs e)
        {
            Client client = null;
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                client = db.Clients.FirstOrDefault(x => x.UserId == UserId);
            }

            if(client != null)
            {
                MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
                window.Frame.Content = new ApplicationEditPage(client.ClientId);
            }
            else
            {
                MessageBox.Show("Заполните информацию о клиенте", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void BtnService_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ServiceAllPage();
        }

        private void BtnProduct_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ProductAllPage();
        }
        private void BtnMaster_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new MasterAllPage();
        }

        private void BtnSertificate_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new SertificateTypeAllPage();
        }

        private void BtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new AdminPage();
        }

        private void BtnUnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            LoginWindow window = new LoginWindow();
            window.Show();
            Application.Current.MainWindow = window;
        }

        private void BtnClientEditPage_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ClientEditPage();
        }
    }
}
