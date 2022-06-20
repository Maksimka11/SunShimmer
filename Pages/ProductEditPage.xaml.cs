using Microsoft.Win32;
using SunShimmer.Model;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SunShimmer.Pages
{
    public partial class ProductEditPage : Page
    {
        public ProductEditPage()
        {
            InitializeComponent();
            LoadCb();
        }
        Product product = null;

        public ProductEditPage(int id)
        {
            InitializeComponent();
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                product = db.Products.FirstOrDefault(x => x.ProductId == id);
            }            
            LoadCb();
            LoadData();
        }

        private void LoadCb()
        {
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                db.Manufacturers.Load();
                CbManufacturer.ItemsSource = db.Manufacturers.Local;
            }
        }

        private void LoadData()
        {

            if (!string.IsNullOrWhiteSpace(product.Photo))
            {
                try
                {
                    ImgPhoto.Source = new BitmapImage(new Uri(product.GetPhoto));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            TbProductName.Text = product.ProductName;
            TbPrice.Text = product.Price.ToString();
            IupCount.Text = product.Count.ToString();
            TbVolume.Text = product.Volume.ToString();
            CbManufacturer.SelectedValue = product.ManufacturerId;
            CbProductType.Text = product.ProductType;
            TbDescription.Text = product.Description;
        }
        string imgName = "";
        string imgPath = "";

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Filter = "All Files (*.*)|*.*|JPEG, JPG (*.jpeg),(*.jpg)|*.jpeg;*.jpg|PNG (*.png)|*.png",
                Title = "Фотография не выбрана"
            };
            if (fileDialog.ShowDialog() == true)
            {
                imgName = fileDialog.SafeFileName;
                imgPath = fileDialog.FileName;
                ImgPhoto.Source = new BitmapImage(new Uri(fileDialog.FileName));
                CheckPhotoName();
            }
        }

        private void CheckPhotoName()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Images\Products\" + imgName))
            {
                for (int i = 1; i < Int32.MaxValue; i++)
                {
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Images\Products\" + imgName))
                    { imgName = $"({i})" + imgName; }
                    else
                     return;
                }
            }
        }

        private void Add()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(imgName))
                {
                    string dest = AppDomain.CurrentDomain.BaseDirectory + @"\Images\Products\" + imgName;
                    File.Copy(imgPath, dest);
                }
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    product = new Product()
                    {
                        Photo = imgName,
                        ProductName = TbProductName.Text,
                        Price = Convert.ToInt32(TbPrice.Text),
                        Count = Convert.ToInt32(IupCount.Value),
                        Volume = Convert.ToInt32(TbVolume.Text),
                        ManufacturerId = (int)CbManufacturer.SelectedValue,
                        ProductType = CbProductType.Text,
                        Description = TbDescription.Text,
                    };
                    db.Products.Add(product);
                    db.SaveChanges();
                    MessageBox.Show("Запись добавлена");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void Update()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(imgName))
                {
                    string dest = AppDomain.CurrentDomain.BaseDirectory + @"\Images\Products\" + imgName;
                    File.Copy(imgPath, dest);
                }

                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    Product product1 = db.Products.FirstOrDefault(x => x.ProductId == product.ProductId);
                    db.Products.Attach(product1);
                    if (!string.IsNullOrWhiteSpace(imgName)) product1.Photo = imgName;

                    product1.ProductName = TbProductName.Text;
                    product1.ManufacturerId = (int)CbManufacturer.SelectedValue;
                    product1.Price = Convert.ToInt32(TbPrice.Text);
                    product1.Count = Convert.ToInt32(IupCount.Value);
                    product1.Volume = Convert.ToInt32(TbVolume.Text);
                    product1.ProductType = CbProductType.Text;
                    product1.Description = TbDescription.Text;
                    db.SaveChanges();
                    MessageBox.Show("Запись обновлена");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private string CheckFields()
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(TbProductName.Text)) message += "Введите название товара" + Environment.NewLine;
            if (string.IsNullOrWhiteSpace(TbPrice.Text)) message += "Введите цену товара" + Environment.NewLine;
            if (IupCount.Value == null) message += "Введите количество товаров" + Environment.NewLine;
            if (string.IsNullOrWhiteSpace(TbVolume.Text)) message += "Введите объем товара" + Environment.NewLine;
            if (CbManufacturer.SelectedIndex == -1) message += "Выберите фирму" + Environment.NewLine;
            if (CbProductType.SelectedIndex == -1) message += "Выберите тип товара" + Environment.NewLine;
            else if (string.IsNullOrWhiteSpace(TbDescription.Text)) message += "Введите описание товара" + Environment.NewLine;
            return message;
        }
        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CheckFields()))
            {
                MessageBox.Show(CheckFields());
                return;
            }
            if (product == null) Add();
            else Update();

            LoadData();
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ProductAllPage();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ProductAllPage();
        }
    }
}
