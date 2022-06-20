using SunShimmer.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SunShimmer.Pages
{

    public partial class PurchaseProductEditPage : Page
    {
        public PurchaseProductEditPage()
        {
            InitializeComponent();
            LoadCb();
        }
        PurchaseProduct purchaseProduct = null;

        public PurchaseProductEditPage(int id)
        {
            InitializeComponent();
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                purchaseProduct = db.PurchaseProducts.FirstOrDefault(x => x.PurchaseProductId == id);
            }
            LoadCb();
            LoadData();
        }

        private void LoadCb()
        {
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                db.Products.Load();
                CbProduct.ItemsSource = db.Products.Local;
                db.PurchaseSertificates.Load();
                CbSertificate.ItemsSource = db.PurchaseSertificates.Local;
            }
        }

        private void LoadData()
        {
            CbProduct.SelectedValue = purchaseProduct.ProductId;
            TbTotalPrice.Text = purchaseProduct.TotalPrice.ToString();
            IupCount.Value = purchaseProduct.Count;
            CbSertificate.SelectedValue = purchaseProduct.SertificateId;
            DtpTimeOfPurchase.Value = purchaseProduct.TimeOfPurchase;
        }

        private void Add()
        {
            try
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                { 

                    purchaseProduct = new PurchaseProduct()
                    {                       
                        ProductId = (int)CbProduct.SelectedValue,
                        Count = Convert.ToInt32(IupCount.Value),
                        TimeOfPurchase = (DateTime)DtpTimeOfPurchase.Value
                    };

                    if (CbSertificate.SelectedIndex != -1)
                    {
                        PurchaseSertificate sertificate = db.PurchaseSertificates.FirstOrDefault(x => x.SertificateId == (int)CbSertificate.SelectedValue);

                        if (Convert.ToInt32(TbTotalPrice.Text) < sertificate.RestSum)
                        {
                            purchaseProduct.TotalPrice = 0;
                            db.PurchaseSertificates.Attach(sertificate);
                            sertificate.RestSum = sertificate.RestSum - Convert.ToInt32(TbTotalPrice.Text);
                        }
                        else
                        {
                            purchaseProduct.TotalPrice = Convert.ToInt32(TbTotalPrice.Text) - sertificate.RestSum;
                            db.PurchaseSertificates.Attach(sertificate);
                            sertificate.RestSum = 0;
                        }
                    }
                    else purchaseProduct.TotalPrice = Convert.ToInt32(TbTotalPrice.Text);

                    if (CbSertificate.SelectedIndex == -1)
                    {
                        purchaseProduct.SertificateId = null;
                    }
                    else purchaseProduct.SertificateId = (int)CbSertificate.SelectedValue; 
                    db.PurchaseProducts.Add(purchaseProduct);
                    db.SaveChanges();
                    MessageBox.Show("Запись добавлена");
                    UpdateSertificate();
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
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    PurchaseProduct purchaseProduct1 = db.PurchaseProducts.FirstOrDefault(x => x.PurchaseProductId == purchaseProduct.PurchaseProductId);
                    db.PurchaseProducts.Attach(purchaseProduct1);

                    purchaseProduct1.ProductId = (int)CbProduct.SelectedValue;
                    if(CbSertificate.SelectedIndex != -1)
                    {
                        PurchaseSertificate sertificate = db.PurchaseSertificates.FirstOrDefault(x => x.SertificateId == (int)CbSertificate.SelectedValue);

                        if (Convert.ToInt32(TbTotalPrice.Text) < sertificate.RestSum)
                        {
                            purchaseProduct1.TotalPrice = 0;
                            db.PurchaseSertificates.Attach(sertificate);
                            sertificate.RestSum = sertificate.RestSum - Convert.ToInt32(TbTotalPrice.Text);
                        }
                        else
                        {
                            purchaseProduct1.TotalPrice = Convert.ToInt32(TbTotalPrice.Text) - sertificate.RestSum;
                            db.PurchaseSertificates.Attach(sertificate);
                            sertificate.RestSum = 0;
                        }
                    }
                    else purchaseProduct1.TotalPrice = Convert.ToInt32(TbTotalPrice.Text);
                    purchaseProduct1.Count = Convert.ToInt32(IupCount.Value);
                    purchaseProduct1.TimeOfPurchase = (DateTime)DtpTimeOfPurchase.Value;

                    if (CbSertificate.SelectedIndex == -1)
                    {
                        purchaseProduct1.SertificateId = null;
                    }
                    else purchaseProduct1.SertificateId = (int)CbSertificate.SelectedValue; 

                    db.SaveChanges();
                    MessageBox.Show("Запись обновлена");
                    UpdateSertificate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private bool CheckSertificate()
        {
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                PurchaseSertificate sertificate = db.PurchaseSertificates.FirstOrDefault(x => x.SertificateId == (int)CbSertificate.SelectedValue);
                if (sertificate.SertificateStatus == false) return false;
            }
            return true;
        }

        private void UpdateSertificate()
        {
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                if (CbSertificate.SelectedIndex == -1) return;
                PurchaseSertificate sertificate = db.PurchaseSertificates.FirstOrDefault(x => x.SertificateId == (int)CbSertificate.SelectedValue);

                if (sertificate.RestSum == 0)
                {
                    db.PurchaseSertificates.Attach(sertificate);
                    sertificate.SertificateStatus = false;
                    db.SaveChanges();
                }
            }
        }

        private string CheckFields()
        {
            string message = "";            
            if (CbProduct.SelectedIndex == -1) message += "Выберите товар" + Environment.NewLine;
            if (CbSertificate.SelectedIndex != -1)
            {
                if (!CheckSertificate()) message += "Выбранный сертификат не активен" + Environment.NewLine;
            }
            if (IupCount.Value == null) message += "Введите количество товаров" + Environment.NewLine;
            else if (DtpTimeOfPurchase.Value == null) message += "Выберите дату и время продажи" + Environment.NewLine;
            return message;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CheckFields()))
            {
                MessageBox.Show(CheckFields());
                return;
            }
            if (purchaseProduct == null) Add();
            else Update();

            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new PurchaseProductAllPage();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new PurchaseProductAllPage();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (CbProduct.SelectedIndex == -1)
            {
                MessageBox.Show("Товар не выбран");
            }
            if (IupCount.Value == null)
            {
                MessageBox.Show("Количество товара не указано");
            }
            else
            {
                int price = 0;
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    db.Products.Load();                    
                    Product product = db.Products.FirstOrDefault(x => x.ProductId == (int)CbProduct.SelectedValue);
                    price = product.Price;
                }
                int totalPrice = totalPrice = (int)(IupCount.Value * price);
                TbTotalPrice.Text = totalPrice.ToString();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            CbSertificate.SelectedIndex = -1;
        }
    }
}
