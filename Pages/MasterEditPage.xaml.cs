using Microsoft.Win32;
using SunShimmer.Model;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SunShimmer.Pages
{
    public partial class MasterEditPage : Page
    {
        public MasterEditPage()
        {
            InitializeComponent();
        }
        Master master = null;

        public MasterEditPage(int id)
        {
            InitializeComponent();
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                master = db.Masters.FirstOrDefault(x => x.MasterId == id);
            }
            LoadData();
        }

        private void LoadData()
        {
            if (!string.IsNullOrWhiteSpace(master.Photo))
            {
                try
                {
                    imgMaster.Source = new BitmapImage(new Uri(master.GetPhoto));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            TbSecondName.Text = master.SecondName;
            TbFirstName.Text = master.FirstName;
            TbPatronymic.Text = master.Patronymic;
            TbPhoneNumber.Text = master.PhoneNumber;
            TbQualification.Text = master.Qualification;
            TbWorkShedule.Text = master.WorkSchedule;
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
                imgMaster.Source = new BitmapImage(new Uri(fileDialog.FileName));
                CheckPhotoName();
            }
        }

        private void CheckPhotoName()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Images\Masters\" + imgName))
            {
                for (int i = 1; i < Int32.MaxValue; i++)
                {
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Images\Masters\" + imgName))
                    { imgName = $"({i})" + imgName; }
                    else
                    { 
                        return; 
                    }
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
            if (master == null) Add();
            else Update();

            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new MasterAllPage();
        }

        private void Add()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(imgName))
                {
                    string dest = AppDomain.CurrentDomain.BaseDirectory + @"\Images\Masters\" + imgName;
                    File.Copy(imgPath, dest);
                }
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    master = new Master()
                    {
                        Photo = imgName,
                        SecondName = TbSecondName.Text,
                        FirstName = TbFirstName.Text,
                        Patronymic = TbPatronymic.Text,
                        PhoneNumber = TbPhoneNumber.Text,
                        Qualification = TbQualification.Text,
                        WorkSchedule = TbWorkShedule.Text,
                    };
                    db.Masters.Add(master);
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
                    string dest = AppDomain.CurrentDomain.BaseDirectory + @"\Images\Masters\" + imgName;
                    File.Copy(imgPath, dest);
                }

                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    Master master1 = db.Masters.FirstOrDefault(x => x.MasterId == master.MasterId);
                    db.Masters.Attach(master1);
                    if (!string.IsNullOrWhiteSpace(imgName)) master1.Photo = imgName;
                    master1.SecondName = TbSecondName.Text;
                    master1.FirstName = TbFirstName.Text;
                    master1.Patronymic = TbPatronymic.Text;
                    master1.PhoneNumber = TbPhoneNumber.Text;
                    master1.Qualification = TbQualification.Text;
                    master1.WorkSchedule = TbWorkShedule.Text;
                    db.SaveChanges(); 
                    MessageBox.Show("Запись изменена");
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
            if (string.IsNullOrWhiteSpace(TbSecondName.Text)) message += "Введите фамилию" + Environment.NewLine;
            if (string.IsNullOrWhiteSpace(TbFirstName.Text)) message += "Введите имя" + Environment.NewLine;
            if (string.IsNullOrWhiteSpace(TbPhoneNumber.Text)) message += "Введите номер телефона" + Environment.NewLine;
            if (TbPhoneNumber.Text.Length != 11) message += "Некорректный номер телефона" + Environment.NewLine;
            return message;
        }

        private void TbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Frame.Content = new MainPage();
        }
    }
}
