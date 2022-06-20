using SunShimmer.Model;
using System;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace SunShimmer.Pages
{
    public partial class PurchaseSertificateAllPage : Page
    {
        public PurchaseSertificateAllPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                db.PurchaseSertificates.Load();
                ICollectionView view = CollectionViewSource.GetDefaultView(db.PurchaseSertificates.Include(x=>x.SertificateType).ToList());
                DgPurchaseSertificate.ItemsSource = view;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new PurchaseSertificateEditPage();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgPurchaseSertificate.SelectedItem == null) return;
            PurchaseSertificate purchaseSertificate = DgPurchaseSertificate.SelectedItem as PurchaseSertificate;

            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new PurchaseSertificateEditPage(purchaseSertificate.SertificateId);
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DgPurchaseSertificate.SelectedItems.Count < 1) return;
            else if (MessageBox.Show("Вы уверены?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    try
                    {
                        for (int i = 0; i < DgPurchaseSertificate.SelectedItems.Count; i++)
                        {
                            PurchaseSertificate purchaseSertificate = DgPurchaseSertificate.SelectedItems[i] as PurchaseSertificate;
                            PurchaseSertificate purchaseSertificate1 = db.PurchaseSertificates.FirstOrDefault(x => x.SertificateId == purchaseSertificate.SertificateId);
                            db.PurchaseSertificates.Remove(purchaseSertificate1);
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

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new AdminPage();
        }

        private void BtnExcel_Click(object sender, RoutedEventArgs e)
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\" + @"\Отчеты\PurchaseSertificates" + ".xls";
            if (!File.Exists(fileName)) File.Create(fileName);
            Excel.Application xlApp = new Excel.Application();
            Excel.Worksheet xlSheet = new Excel.Worksheet();
            try
            {
                xlApp.Workbooks.Open(fileName);
                xlApp.Interactive = false;
                xlApp.EnableEvents = false;
                xlSheet = (Excel.Worksheet)xlApp.Sheets[1];
                xlSheet.Name = "Продажа сертификатов";
                int row = 2;
                int i = 0;
                xlSheet.Cells[1, 1] = "№";
                xlSheet.Cells[1, 2] = "Код сертификата";
                xlSheet.Cells[1, 3] = "Тип сертификата";
                xlSheet.Cells[1, 4] = "Цена сертификата";
                xlSheet.Cells[1, 6] = "Дата и время активации";
                if (DgPurchaseSertificate.Items.Count > 0)
                {
                    for (i = 0; i < DgPurchaseSertificate.Items.Count; i++)
                    {
                        PurchaseSertificate sell = DgPurchaseSertificate.Items[i] as PurchaseSertificate;
                        xlSheet.Cells[row, 1] = i + 1;
                        xlSheet.Cells[row, 2] = sell.SertificateId;
                        xlSheet.Cells[row, 3] = sell.SertificateType.SertificateTypeName;
                        xlSheet.Cells[row, 4] = sell.SertificateType.Price;
                        xlSheet.Cells[row, 5] = sell.TimeOfActivation;
                        row++;
                    }
                }
                MessageBox.Show("Отчет сохранен");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                xlApp.Visible = true;
                xlApp.Interactive = true;
                xlApp.ScreenUpdating = true;
                xlApp.UserControl = true;
            }
        }
    }
}