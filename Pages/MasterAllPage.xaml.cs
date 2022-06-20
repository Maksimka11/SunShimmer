using SunShimmer.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SunShimmer.Pages
{
    public partial class MasterAllPage : Page
    {
        public MasterAllPage()
        {
            InitializeComponent();
            LoadData();
            if (MainWindow.Role == "Админ")
            {
                BtnAdd.Visibility = Visibility.Visible;
                BtnEdit.Visibility = Visibility.Visible;
                BtnDel.Visibility = Visibility.Visible;
                StackFunc.Visibility = Visibility.Visible;
                DrtPhoneNumber.Visibility = Visibility.Visible;
            }
        }

        private void LoadData()
        {
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                db.Masters.Load();
                DgMasters.ItemsSource = db.Masters.Local;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new MasterEditPage();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgMasters.SelectedItem == null) return;
            Master master = DgMasters.SelectedItem as Master;

            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new MasterEditPage(master.MasterId);
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DgMasters.SelectedItems.Count < 1) return;
            else if (MessageBox.Show("Вы уверены?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    try
                    {
                        for (int i = 0; i < DgMasters.SelectedItems.Count; i++)
                        {

                            Master master = DgMasters.SelectedItems[i] as Master;
                            Master master1 = db.Masters.FirstOrDefault(x => x.MasterId == master.MasterId);
                            db.Masters.Remove(master1);
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