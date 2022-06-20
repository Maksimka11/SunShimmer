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
    public partial class ProvisionServiceAllPage : Page
    {
        public ProvisionServiceAllPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                db.ProvisionServices.Load();
                ICollectionView view = CollectionViewSource.GetDefaultView(db.ProvisionServices.Include(x => x.Product).Include(x=>x.PurchaseSertificate).Include(x=>x.Service).ToList());
                DgProvisionService.ItemsSource = view;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ProvisionServiceEditPage();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgProvisionService.SelectedItem == null) return;
            ProvisionService provisionService = DgProvisionService.SelectedItem as ProvisionService;

            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ProvisionServiceEditPage(provisionService.ProvisionServiceId);
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DgProvisionService.SelectedItems.Count < 1) return;
            else if (MessageBox.Show("Вы уверены?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    try
                    {
                        for (int i = 0; i < DgProvisionService.SelectedItems.Count; i++)
                        {
                            ProvisionService provisionService = DgProvisionService.SelectedItems[i] as ProvisionService;
                            ProvisionService provisionService1 = db.ProvisionServices.FirstOrDefault(x => x.ProvisionServiceId == provisionService.ProvisionServiceId);
                            db.ProvisionServices.Remove(provisionService1);
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
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\" + @"\Отчеты\ProvisionServices" + ".xls";
            if (!File.Exists(fileName)) File.Create(fileName);
            Excel.Application xlApp = new Excel.Application();
            Excel.Worksheet xlSheet = new Excel.Worksheet();
            try
            {
                xlApp.Workbooks.Open(fileName);
                xlApp.Interactive = false;
                xlApp.EnableEvents = false;
                xlSheet = (Excel.Worksheet)xlApp.Sheets[1];
                xlSheet.Name = "Продажа товаров";
                int row = 2;
                int i = 0;
                xlSheet.Cells[1, 1] = "№";
                xlSheet.Cells[1, 2] = "Услуга";
                xlSheet.Cells[1, 3] = "Используемые товары";
                xlSheet.Cells[1, 4] = "Итоговая цена";
                xlSheet.Cells[1, 5] = "Дата и время продажи";
                if (DgProvisionService.Items.Count > 0)
                {
                    for (i = 0; i < DgProvisionService.Items.Count; i++)
                    {
                        ProvisionService sell = DgProvisionService.Items[i] as ProvisionService;
                        xlSheet.Cells[row, 1] = i + 1;
                        xlSheet.Cells[row, 2] = sell.Service.ServiceName;
                        xlSheet.Cells[row, 3] = sell.Product.ProductName;
                        xlSheet.Cells[row, 4] = sell.Price;
                        xlSheet.Cells[row, 5] = sell.TimeOfProvision;
                        row++;
                    }
                }
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
            MessageBox.Show("Отчет добавлен");
        }
    }
}