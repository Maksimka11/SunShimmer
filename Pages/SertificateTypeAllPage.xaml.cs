using SunShimmer.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SunShimmer.Pages
{
    public partial class SertificateTypeAllPage : Page
    {
        public SertificateTypeAllPage()
        {
            InitializeComponent();
            if (MainWindow.Role != "Админ")
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
                db.SertificateTypes.Load();
                DgSertificateTypes.ItemsSource = db.SertificateTypes.Local;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new SertificateTypeEditPage();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgSertificateTypes.SelectedItem == null) return;
            SertificateType sertificateType = DgSertificateTypes.SelectedItem as SertificateType;

            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new SertificateTypeEditPage(sertificateType.SertificateTypeId);
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DgSertificateTypes.SelectedItems.Count < 1) return;
            else if (MessageBox.Show("Вы уверены?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    try
                    {
                        for (int i = 0; i < DgSertificateTypes.SelectedItems.Count; i++)
                        {
                            SertificateType sertificateType = DgSertificateTypes.SelectedItems[i] as SertificateType;
                            SertificateType sertificateType1 = db.SertificateTypes.FirstOrDefault(x => x.SertificateTypeId == sertificateType.SertificateTypeId);
                            db.SertificateTypes.Remove(sertificateType1);
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
