using SunShimmer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SunShimmer.Pages
{
    public partial class PurchaseSertificateEditPage : Page
    {
        public PurchaseSertificateEditPage()
        {
            InitializeComponent();
            LoadCb();

        }
        PurchaseSertificate purchaseSertificate = null;

        public PurchaseSertificateEditPage(int id)
        {
            InitializeComponent();
            LoadCb();
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                purchaseSertificate = db.PurchaseSertificates.FirstOrDefault(x => x.SertificateId == id);
            }
            LoadData();
        }

        private void LoadData()
        {
            CbSertificateType.SelectedValue = purchaseSertificate.SertificateTypeId;
            CbStatus.SelectedIndex = Convert.ToInt16(purchaseSertificate.SertificateStatus);
            DtpTimeOfActivation.Value = purchaseSertificate.TimeOfActivation;
        }

        private void LoadCb()
        {
            using(SunShimmerEntities db = new SunShimmerEntities())
            {
                db.SertificateTypes.Load();
                CbSertificateType.ItemsSource = db.SertificateTypes.Local;
            }
        }

        private void Add()
        {
            try
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    purchaseSertificate = new PurchaseSertificate()
                    {
                        TimeOfActivation = (DateTime)DtpTimeOfActivation.Value,
                        SertificateStatus = Convert.ToBoolean(CbStatus.SelectedIndex),
                        SertificateTypeId = (int)CbSertificateType.SelectedValue,
                        
                    };
                    SertificateType type = db.SertificateTypes.FirstOrDefault(x => x.SertificateTypeId == (int)CbSertificateType.SelectedValue);
                    purchaseSertificate.RestSum = type.Price;
                    db.PurchaseSertificates.Add(purchaseSertificate);
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
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    PurchaseSertificate sertificateType1 = db.PurchaseSertificates.FirstOrDefault(x => x.SertificateId == purchaseSertificate.SertificateId);
                   db.PurchaseSertificates.Attach(sertificateType1);
                    sertificateType1.TimeOfActivation = (DateTime)DtpTimeOfActivation.Value;
                    sertificateType1.SertificateStatus = Convert.ToBoolean(CbStatus.SelectedIndex);
                    sertificateType1.SertificateTypeId = (int)CbSertificateType.SelectedValue;
                    SertificateType type = db.SertificateTypes.FirstOrDefault(x => x.SertificateTypeId == (int)CbSertificateType.SelectedValue);
                    sertificateType1.RestSum = type.Price;
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
            if (CbSertificateType.SelectedItem == null) message += "Выберите тип сертификата" + Environment.NewLine;
            if (CbStatus.SelectedItem == null) message += "Выберите статус сертификата" + Environment.NewLine;
            if (DtpTimeOfActivation.Value == null) message += "Выберите время активации" + Environment.NewLine;
            return message;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CheckFields()))
            {
                MessageBox.Show(CheckFields());
                return;
            }

            if (purchaseSertificate == null) Add();
            else Update();

            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new PurchaseSertificateAllPage();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new PurchaseSertificateAllPage();
        }
    }
}