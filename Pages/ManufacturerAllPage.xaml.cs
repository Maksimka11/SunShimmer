using SunShimmer.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SunShimmer.Pages
{
    public partial class ManufacturerAllPage : Page
    {
        public ManufacturerAllPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                db.Manufacturers.Load();
                DgManufacturers.ItemsSource = db.Manufacturers.Local;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ManufacturerEditPage();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgManufacturers.SelectedItem == null) return;
            Manufacturer manufacturer = DgManufacturers.SelectedItem as Manufacturer;

            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ManufacturerEditPage(manufacturer.ManufacturerId);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new AdminPage();
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DgManufacturers.SelectedItems.Count < 1) return;
            else if (MessageBox.Show("Вы уверены?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    try
                    {
                        for (int i = 0; i < DgManufacturers.SelectedItems.Count; i++)
                        {

                            Manufacturer manufacturer = DgManufacturers.SelectedItems[i] as Manufacturer;
                            Manufacturer manufacturer1 = db.Manufacturers.FirstOrDefault(x => x.ManufacturerId == manufacturer.ManufacturerId);
                            db.Manufacturers.Remove(manufacturer1);
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