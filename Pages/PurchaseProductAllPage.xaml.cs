using SunShimmer.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Data.Entity;
using System.ComponentModel;
using System.Windows.Data;

namespace SunShimmer.Pages
{
    public partial class PurchaseProductAllPage : Page
    {
        public PurchaseProductAllPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                db.PurchaseProducts.Load();
                ICollectionView view = CollectionViewSource.GetDefaultView(db.PurchaseProducts.Include(x=>x.Product).Include(x=>x.PurchaseSertificate).ToList());
                DgPurchaseProduct.ItemsSource = view;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new PurchaseProductEditPage();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgPurchaseProduct.SelectedItem == null) return;
            PurchaseProduct purchaseProduct = DgPurchaseProduct.SelectedItem as PurchaseProduct;

            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new PurchaseProductEditPage(purchaseProduct.PurchaseProductId);
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DgPurchaseProduct.SelectedItems.Count < 1) return;
            else if (MessageBox.Show("Вы уверены?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    try
                    {
                        for (int i = 0; i < DgPurchaseProduct.SelectedItems.Count; i++)
                        {
                            PurchaseProduct purchaseProduct = DgPurchaseProduct.SelectedItems[i] as PurchaseProduct;
                            PurchaseProduct purchaseProduct1 = db.PurchaseProducts.FirstOrDefault(x => x.PurchaseProductId == purchaseProduct.PurchaseProductId);
                            db.PurchaseProducts.Remove(purchaseProduct1);
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
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\" + @"\Отчеты\PurchaseProducts" + ".xls";
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
                xlSheet.Cells[1, 2] = "Товар";
                xlSheet.Cells[1, 3] = "Количество";
                xlSheet.Cells[1, 4] = "Итоговая цена";
                xlSheet.Cells[1, 5] = "Дата и время продажи";
                if (DgPurchaseProduct.Items.Count > 0)
                {
                    for (i = 0; i < DgPurchaseProduct.Items.Count; i++)
                    {
                        PurchaseProduct sell = DgPurchaseProduct.Items[i] as PurchaseProduct;
                        xlSheet.Cells[row, 1] = i + 1;
                        xlSheet.Cells[row, 2] = sell.Product.ProductName;
                        xlSheet.Cells[row, 3] = sell.Count;
                        xlSheet.Cells[row, 4] = sell.TotalPrice;
                        xlSheet.Cells[row, 5] = sell.TimeOfPurchase;
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