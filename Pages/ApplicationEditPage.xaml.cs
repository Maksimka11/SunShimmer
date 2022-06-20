using SunShimmer.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SunShimmer.Pages
{
    public partial class ApplicationEditPage : Page
    {

        Client client = null;
        Record record = null;

        public ApplicationEditPage(int id)
        {
            InitializeComponent();
            LoadMasters();

            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                client = db.Clients.FirstOrDefault(x => x.ClientId == id);
            }
        }

        public void LoadMasters()
        {
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                db.Masters.Load();
                CbMaster.ItemsSource = db.Masters.Local;
            }
        }

        private void Add()
        {
            try
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    record = new Record()
                    {
                        Comment = TbComment.Text,
                        DatefApplication = DateTime.Today,
                        ApplicationView = true,
                        TimeOfRecord = (DateTime)dtpRecord.Value,
                        RecordStatus = "Активен",
                        MasterId = (int)CbMaster.SelectedValue,
                        ClientId = client.ClientId
                    };
                    if (string.IsNullOrWhiteSpace(TbPhone.Text)) record.PhoneNumber = client.PhoneNumber;
                    else record.PhoneNumber = TbPhone.Text;
                    db.Records.Add(record);
                    db.SaveChanges();
                    MessageBox.Show("Заявка сохранена");
                }
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
                    window.Frame.Content = new MainPage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void TbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private string CheckFields()
        {
            string message = "";
            if (CbMaster.SelectedItem == null) message += "Выберите мастера" + Environment.NewLine;
            if (dtpRecord.Value == null) message += "Выберите дату и время записи" + Environment.NewLine;
            if(!string.IsNullOrWhiteSpace(TbPhone.Text))
                if(TbPhone.Text.Length != 11) message += "Не корректный номер телефона" + Environment.NewLine;
            return message;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CheckFields()))
            {
                MessageBox.Show(CheckFields());
                return;
            }
            Add();
        }
    }
}