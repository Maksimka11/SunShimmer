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
    public partial class ProductAllPage : Page
    {
        public ProductAllPage()
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
            using(SunShimmerEntities db = new SunShimmerEntities())
            {
                db.Products.Load();
                ICollectionView view = CollectionViewSource.GetDefaultView(db.Products.Include(x=>x.Manufacturer).ToList());
                DgProduct.ItemsSource = view;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ProductEditPage();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgProduct.SelectedItem == null) return;
            Product product = DgProduct.SelectedItem as Product;

            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ProductEditPage(product.ProductId);
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DgProduct.SelectedItems.Count < 1) return;
            else if (MessageBox.Show("Вы уверены?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    try
                    {
                        for (int i = 0; i < DgProduct.SelectedItems.Count; i++)
                        {
                            Product product = DgProduct.SelectedItems[i] as Product;
                            Product product1 = db.Products.FirstOrDefault(x => x.ProductId == product.ProductId);
                            db.Products.Remove(product1);
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
