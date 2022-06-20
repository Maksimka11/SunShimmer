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
    public partial class ProvisionServiceEditPage : Page
    {
        public ProvisionServiceEditPage()
        {
            InitializeComponent();
            LoadCb();
        }
        ProvisionService provisionService = null;

        public ProvisionServiceEditPage(int id)
        {
            InitializeComponent();
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                provisionService = db.ProvisionServices.FirstOrDefault(x => x.ProvisionServiceId == id);
            }
            LoadCb();
            LoadData();
        }

        private void LoadCb()
        {
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                db.Services.Load();
                CbService.ItemsSource = db.Services.Local;
                db.Products.Load();
                CbUsedProduct.ItemsSource = db.Products.Local;
                db.PurchaseSertificates.Load();
                CbSertificate.ItemsSource = db.PurchaseSertificates.Local;
                db.Records.Load();
                ICollectionView view = new CollectionView(db.Records.Include(x => x.Client).ToList());
                CbRecord.ItemsSource = view;
            }
        }

        private void LoadData()
        {
            CbService.SelectedValue = provisionService.ServiceId;
            TbPrice.Text = provisionService.Price.ToString();
            CbRecord.SelectedValue = provisionService.RecordId;
            CbSertificate.SelectedValue = provisionService.SertificateId;
            DtpTimeOfProvision.Value = provisionService.TimeOfProvision;
            CbUsedProduct.SelectedValue = provisionService.ProductId;
        }

        private void Add()
        {
            try
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {


                    provisionService = new ProvisionService()
                    {
                        ServiceId = (int)CbService.SelectedValue,
                        RecordId = (int)CbRecord.SelectedValue,
                        TimeOfProvision = (DateTime)DtpTimeOfProvision.Value,
                        ProductId = (int)CbUsedProduct.SelectedValue
                    };
                    if (CbSertificate.SelectedIndex != -1)
                    {
                        provisionService.SertificateId = (int)CbSertificate.SelectedValue;
                        PurchaseSertificate sertificate = db.PurchaseSertificates.FirstOrDefault(x => x.SertificateId == (int)CbSertificate.SelectedValue);
                        if (Convert.ToInt32(TbPrice.Text) < sertificate.RestSum)
                        {
                            provisionService.Price = 0;
                            db.PurchaseSertificates.Attach(sertificate);
                            sertificate.RestSum = sertificate.RestSum - Convert.ToInt32(TbPrice);
                        }
                        else
                        {
                            provisionService.Price = Convert.ToInt32(TbPrice.Text) - sertificate.RestSum;
                            db.PurchaseSertificates.Attach(sertificate);
                            sertificate.RestSum = 0;
                        }
                    }

                    db.ProvisionServices.Add(provisionService);
                    db.SaveChanges();
                    UpdateSertificate();
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
            
            using (SunShimmerEntities db = new SunShimmerEntities())
            {

                ProvisionService provisionService1 = db.ProvisionServices.FirstOrDefault(x => x.ProvisionServiceId == provisionService.ProvisionServiceId);
                db.ProvisionServices.Attach(provisionService1);
                provisionService1.ServiceId = (int)CbService.SelectedValue;

                if (CbSertificate.SelectedIndex != -1)
                {
                    PurchaseSertificate sertificate = db.PurchaseSertificates.FirstOrDefault(x => x.SertificateId == (int)CbSertificate.SelectedValue);

                    if (Convert.ToInt32(TbPrice.Text) < sertificate.RestSum)
                    {
                        provisionService1.Price = 0;
                        db.PurchaseSertificates.Attach(sertificate);
                        sertificate.RestSum = sertificate.RestSum - Convert.ToInt32(TbPrice);
                    }
                    else
                    {
                        provisionService1.Price = Convert.ToInt32(TbPrice.Text) - sertificate.RestSum;
                        db.PurchaseSertificates.Attach(sertificate);
                        sertificate.RestSum = 0;
                    }
                }
                else provisionService1.Price = Convert.ToInt32(TbPrice.Text);

                provisionService1.RecordId = (int)CbRecord.SelectedValue;
                if (CbSertificate.SelectedIndex != -1) provisionService1.SertificateId = (int)CbSertificate.SelectedValue;
                else provisionService1.SertificateId = null;
                provisionService1.TimeOfProvision = (DateTime)DtpTimeOfProvision.Value;
                provisionService1.ProductId = (int)CbUsedProduct.SelectedValue;
                db.SaveChanges();
                UpdateSertificate();
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
            if (CbService.SelectedIndex == -1) message += "Выберите услугу" + Environment.NewLine;
            if (CbRecord.SelectedIndex == -1) message += "Выберите запись" + Environment.NewLine;
            if (CbUsedProduct.SelectedIndex == -1) message += "Выберите используемый товар" + Environment.NewLine;
            if (string.IsNullOrWhiteSpace(TbPrice.Text)) message += "Введите цену товара" + Environment.NewLine;
            if (CbSertificate.SelectedIndex != -1)
            {
                if (!CheckSertificate()) message += "Выбранный сертификат не активен" + Environment.NewLine;
            }
            try
            {
                int Price = (int)CbUsedProduct.SelectedValue;
            }
            catch
            {
                message += "Цена товара указана не корректно" + Environment.NewLine;
            }
            if (DtpTimeOfProvision.Value == null) message += "Выберите дату и время продажи" + Environment.NewLine;
            return message;
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

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CheckFields()))
            {
                MessageBox.Show(CheckFields());
                return;
            }
            if (provisionService == null) Add();
            else Update();
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ProvisionServiceAllPage();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ProvisionServiceAllPage();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            CbSertificate.SelectedIndex = -1;
        }
    }
}
