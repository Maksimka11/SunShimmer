using SunShimmer.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SunShimmer.Pages
{
    public partial class UserAllPage : Page
    {
        public UserAllPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                db.Users.Load();
                DgUsers.ItemsSource = db.Users.Local;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new UserEditPage();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgUsers.SelectedItem == null) return;
            User user = DgUsers.SelectedItem as User;

            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new UserEditPage(user.UserId);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new AdminPage();
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DgUsers.SelectedItems.Count < 1) return;
            else if (MessageBox.Show("Вы уверены?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    try
                    {
                        for (int i = 0; i < DgUsers.SelectedItems.Count; i++)
                        {

                            User user = DgUsers.SelectedItems[i] as User;
                            User user1 = db.Users.FirstOrDefault(x => x.UserId == user.UserId);
                            db.Users.Remove(user1);
                            db.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        db.Dispose();
                    }
                    finally
                    {
                        LoadData();
                    }
                }
            }
        }
    }
}
