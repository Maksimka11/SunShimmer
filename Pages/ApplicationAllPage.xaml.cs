using SunShimmer.Model;
using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SunShimmer.Pages
{
    public partial class ApplicationAllPage : Page
    {
        public ApplicationAllPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new MainPage();
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DgRecord.SelectedItems.Count < 1) return;
            else if (MessageBox.Show("Вы уверены?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    try
                    {
                        for (int i = 0; i < DgRecord.SelectedItems.Count; i++)
                        {
                            Record record = DgRecord.SelectedItems[i] as Record;
                            Record record1 = db.Records.FirstOrDefault(x => x.RecordId == record.RecordId);
                            db.Records.Remove(record1);
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

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (DgRecord.SelectedItem == null) return;
            Record record = DgRecord.SelectedItem as Record;

            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new RecordEditPage(record.RecordId);
        }
    }
}