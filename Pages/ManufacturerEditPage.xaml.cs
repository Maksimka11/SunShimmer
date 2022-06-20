using SunShimmer.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SunShimmer.Pages
{
    public partial class ManufacturerEditPage : Page
    {
        public ManufacturerEditPage()
        {
            InitializeComponent();
        }
        Manufacturer manufacturer = null;

        public ManufacturerEditPage(int id)
        {
            InitializeComponent();

            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                manufacturer = db.Manufacturers.FirstOrDefault(x => x.ManufacturerId == id);
            }
            LoadData();
        }

        private void LoadData()
        {
            TbManufacturerName.Text = manufacturer.ManufacturerName;
            TbDescription.Text = manufacturer.Description;
        }

        private void Add()
        {
            try
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    Manufacturer manufacturer = db.Manufacturers.FirstOrDefault(x => x.ManufacturerName == TbManufacturerName.Text);
                    if (manufacturer != null)
                    {
                        MessageBox.Show("Эта фирма уже существует");
                        return;
                    }

                    manufacturer = new Manufacturer()
                    {
                        ManufacturerName = TbManufacturerName.Text,
                        Description = TbDescription.Text,
                    };
                    db.Manufacturers.Add(manufacturer);
                    db.SaveChanges();
                    MessageBox.Show("Запись добавлена");
                }
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    Manufacturer manufacturer = db.Manufacturers.FirstOrDefault(x => x.ManufacturerName == TbManufacturerName.Text);
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
                    Manufacturer manufacturer1 = db.Manufacturers.FirstOrDefault(x => x.ManufacturerId == manufacturer.ManufacturerId);
                    db.Manufacturers.Attach(manufacturer1);
                    manufacturer1.ManufacturerName = TbManufacturerName.Text;
                    manufacturer1.Description = TbDescription.Text;
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
            if (string.IsNullOrWhiteSpace(TbManufacturerName.Text)) message += "Введите фирму изготовителя" + Environment.NewLine;
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
            if (manufacturer == null) Add();
            else Update();      
            
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ManufacturerAllPage();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ManufacturerAllPage();
        }
    }
}
