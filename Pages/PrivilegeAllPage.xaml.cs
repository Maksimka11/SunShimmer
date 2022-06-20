using SunShimmer.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SunShimmer.Pages
{
    public partial class PrivilegeAllPage : Page
    {
        public PrivilegeAllPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                db.Privileges.Load();
                DgPrivileges.ItemsSource = db.Privileges.Local;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new PrivilegeEditPage();

            if (window.DialogResult == true)
            {
                MessageBox.Show("Запись добавлена");
                LoadData();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgPrivileges.SelectedItem == null) return;
            Privilege privilege = DgPrivileges.SelectedItem as Privilege;

            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new PrivilegeEditPage(privilege.PrivilegeId);

            if (window.DialogResult == true)
            {
                MessageBox.Show("Запись обновлена");
                LoadData();
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new AdminPage();
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DgPrivileges.SelectedItems.Count < 1) return;
            else if (MessageBox.Show("Вы уверены?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    try
                    {
                        for (int i = 0; i < DgPrivileges.SelectedItems.Count; i++)
                        {

                            Privilege privilege = DgPrivileges.SelectedItems[i] as Privilege;
                            Privilege privilege1 = db.Privileges.FirstOrDefault(x => x.PrivilegeId == privilege.PrivilegeId);
                            db.Privileges.Remove(privilege1);
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
