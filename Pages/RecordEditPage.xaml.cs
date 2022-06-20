using SunShimmer.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace SunShimmer.Pages
{
    public partial class RecordEditPage : Page
    {   
        public RecordEditPage()
        {
            InitializeComponent();
            LoadCb();
        }
        Record record = null;

        public RecordEditPage(int id)
        {
            InitializeComponent();
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                record = db.Records.FirstOrDefault(x => x.RecordId == id);
            }
            LoadCb();
            LoadData();
        }

        private void LoadCb()
        {
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                db.Clients.Load();
                CbClient.ItemsSource = db.Clients.Local;
                db.Masters.Load();
                CbMaster.ItemsSource = db.Masters.Local;
            }
        }

        private void LoadData()
        {
            CbClient.SelectedValue = record.ClientId;
            CbMaster.SelectedValue = record.MasterId;
            DtpTimeOfRecord.Value = record.TimeOfRecord;
            CbStatus.Text = record.RecordStatus;
        }

        private void Add()
        {
            try
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    record = new Record()
                    {
                        ClientId = (int)CbClient.SelectedValue,                        
                        MasterId = (int)CbMaster.SelectedValue,
                        RecordStatus = CbStatus.Text,
                        TimeOfRecord = (DateTime)DtpTimeOfRecord.Value,
                    };
                    db.Records.Add(record);
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
                    Record record1 = db.Records.FirstOrDefault(x => x.RecordId == record.RecordId);
                    db.Records.Attach(record1);

                    record1.ClientId = (int)CbClient.SelectedValue;
                    record1.MasterId = (int)CbMaster.SelectedValue;
                    record1.TimeOfRecord = (DateTime)DtpTimeOfRecord.Value;
                    record1.RecordStatus = CbStatus.Text;
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
            if (CbClient.SelectedIndex == -1) message += "Выберите клиента" + Environment.NewLine;
            if (CbMaster.SelectedIndex == -1) message += "Выберите мастера" + Environment.NewLine;
            if (CbStatus.SelectedIndex == -1) message += "Выберите статус записи" + Environment.NewLine;
            else if (DtpTimeOfRecord.Value == null) message += "Выберите дату и время записи" + Environment.NewLine;
            return message;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CheckFields()))
            {
                MessageBox.Show(CheckFields());
                return;
            }
            if (record != null)  Update();

            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new RecordAllPage();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new RecordAllPage();
        }
    }
}
