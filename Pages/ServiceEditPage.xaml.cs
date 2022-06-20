using SunShimmer.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SunShimmer.Pages
{
    public partial class ServiceEditPage : Page
    {
        public ServiceEditPage()
        {
            InitializeComponent();
        }
        Service service = null;

        public ServiceEditPage(int id)
        {
            InitializeComponent();
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                service = db.Services.FirstOrDefault(x => x.ServiceId == id);
            }
            LoadData();            
        }

        private void LoadData()
        {
            int p = service.Price;
            string strPrice = p.ToString();

            TbServiceName.Text = service.ServiceName;
            TbPrice.Text = strPrice;
            TbContraindications.Text = service.Contraindications;
            TbDescription.Text = service.Description;
        }


        private void Add()
        {
            try
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    int intPrice = Convert.ToInt32(TbPrice.Text);
                    service = new Service()
                    {
                        ServiceName = TbServiceName.Text,
                        Price = intPrice,
                        Contraindications = TbContraindications.Text,
                        Description = TbDescription.Text,
                    };
                    db.Services.Add(service);
                    db.SaveChanges();
                    MessageBox.Show("Запись добавлена");
                }
                MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
                window.Frame.Content = new ServiceAllPage();
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
                    Service service1 = db.Services.FirstOrDefault(x => x.ServiceId == service.ServiceId);
                    db.Services.Attach(service1);

                    service1.ServiceName = TbServiceName.Text;
                    service1.Price = intPrice;
                    service1.Contraindications = TbContraindications.Text;
                    service1.Description = TbDescription.Text;
                    db.SaveChanges();
                    MessageBox.Show("Запись изменена");
                }
                MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
                window.Frame.Content = new ServiceAllPage();
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
            if (string.IsNullOrWhiteSpace(TbServiceName.Text)) message += "Введите название услуги" + Environment.NewLine;
            if (string.IsNullOrWhiteSpace(TbPrice.Text)) message += "Введите цену услуги" + Environment.NewLine;
            if (string.IsNullOrWhiteSpace(TbContraindications.Text)) message += "Введите противопоказания" + Environment.NewLine;
            else if (string.IsNullOrWhiteSpace(TbDescription.Text)) message += "Введите описание услуги" + Environment.NewLine;
            return message;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ServiceAllPage();
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CheckFields()))
            {
                MessageBox.Show(CheckFields());
                return;
            }
            if (service == null) Add();
            else Update();
        }
    }
}
