using SunShimmer.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SunShimmer.Pages
{
    public partial class ServiceAllPage : Page
    {
        public ServiceAllPage()
        {
            InitializeComponent();
            if(MainWindow.Role != "Админ")
            {
                BtnAdd.Visibility = Visibility.Collapsed;
                BtnDel.Visibility = Visibility.Collapsed;
                BtnEdit.Visibility = Visibility.Collapsed;
            }
            LoadData();
        }

        private void LoadData()
        {
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                db.Services.Load();
                DgServices.ItemsSource = db.Services.Local;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ServiceEditPage();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgServices.SelectedItem == null) return;
            Service service = DgServices.SelectedItem as Service;

            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ServiceEditPage(service.ServiceId);
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DgServices.SelectedItems.Count < 1) return;
            else if (MessageBox.Show("Вы уверены?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    try
                    {
                        for (int i = 0; i < DgServices.SelectedItems.Count; i++)
                        {

                            Service service = DgServices.SelectedItems[i] as Service;
                            Service service1 = db.Services.FirstOrDefault(x => x.ServiceId == service.ServiceId);
                            db.Services.Remove(service1);
                            db.SaveChanges();
                            MessageBox.Show("Запись удалена");
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