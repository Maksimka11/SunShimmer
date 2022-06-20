using SunShimmer.Model;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SunShimmer.Pages
{
    public partial class SertificateTypeEditPage : Page
    {
        public SertificateTypeEditPage()
        {
            InitializeComponent();
        }
        SertificateType sertificateType = null;

        public SertificateTypeEditPage(int id)
        {
            InitializeComponent();

            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                sertificateType = db.SertificateTypes.FirstOrDefault(x => x.SertificateTypeId == id);
            }
            LoadData();
        }

        private void LoadData()
        {
            int p = sertificateType.Price;
            string strPrice = p.ToString();
            
            
            TbSertificateTypeName.Text = sertificateType.SertificateTypeName;
            TbPrice.Text = strPrice;
            TbDescription.Text = sertificateType.Description;
        }

        private void Add()
        {
            try
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    SertificateType sertificateType = db.SertificateTypes.FirstOrDefault(x => x.SertificateTypeName == TbSertificateTypeName.Text);
                    if (sertificateType != null)
                    {
                        MessageBox.Show("Этот уровень уже существует");
                        return;
                    }
                    int intPrice = Convert.ToInt32(TbPrice.Text);
                    sertificateType = new SertificateType()
                    {
                        SertificateTypeName = TbSertificateTypeName.Text,
                        Price = intPrice,
                        Description = TbDescription.Text,
                    };
                    db.SertificateTypes.Add(sertificateType);
                    db.SaveChanges();
                    MessageBox.Show("Запись добавлена");
                }
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    SertificateType sertificateType = db.SertificateTypes.FirstOrDefault(x => x.SertificateTypeName == TbSertificateTypeName.Text);
                    
                    MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
                    window.Frame.Content = new SertificateTypeAllPage();
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
                int intPrice = Convert.ToInt32(TbPrice.Text);
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    SertificateType sertificateType1 = db.SertificateTypes.FirstOrDefault(x => x.SertificateTypeId == sertificateType.SertificateTypeId);
                    db.SertificateTypes.Attach(sertificateType1);
                    sertificateType1.SertificateTypeName = TbSertificateTypeName.Text;
                    sertificateType1.Price = intPrice;
                    sertificateType1.Description = TbDescription.Text;
                    db.SaveChanges();
                    MessageBox.Show("Запись обновлена");
                }
                MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
                window.Frame.Content = new SertificateTypeAllPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void TbPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private string CheckFields()
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(TbSertificateTypeName.Text)) message += "Введите название сертификата" + Environment.NewLine;
            if (string.IsNullOrWhiteSpace(TbDescription.Text)) message += "Введите описание" + Environment.NewLine;
            else if (string.IsNullOrWhiteSpace(TbPrice.Text)) message += "Введите цену сертификата" + Environment.NewLine;
            return message;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CheckFields()))
            {
                MessageBox.Show(CheckFields());
                return;
            }
            if (!string.IsNullOrWhiteSpace(CheckFields()))
            {
                MessageBox.Show(CheckFields());
                return;
            }
            if (sertificateType == null) Add();
            else Update();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new SertificateTypeAllPage();
        }
    }
}