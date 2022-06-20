using Microsoft.Win32;
using SunShimmer.Model;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SunShimmer.Pages
{
    public partial class ClientEditPage : Page
    {
        public ClientEditPage()
        {
            InitializeComponent();
            LoadPrivelegies();

            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                client = db.Clients.FirstOrDefault(x => x.UserId == MainWindow.UserId);
                if (MainWindow.Role == "Админ") LPrivilege.Visibility = Visibility.Visible;
                if (MainWindow.Role == "Админ") CbPrivilege.Visibility = Visibility.Visible;
                if(client != null) LoadData();
            }
        }
        Client client = null;

        public ClientEditPage(int id)
        {
            InitializeComponent();
            LoadPrivelegies();

            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                client = db.Clients.FirstOrDefault(x => x.UserId == id);
                if (MainWindow.Role == "Админ") LPrivilege.Visibility = Visibility.Visible;
                if (MainWindow.Role == "Админ") CbPrivilege.Visibility = Visibility.Visible;
                LoadData();

            }
        }

        private void LoadPrivelegies()
        {
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                db.Privileges.Load();
                CbPrivilege.ItemsSource = db.Privileges.Local;
            }
        }

        private void LoadData()
        {
            if (!string.IsNullOrWhiteSpace(client.Photo))
            {
                try
                {
                    imgClient.Source = new BitmapImage(new Uri(client.GetPhoto));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            TbSecondName.Text = client.SecondName;
            TbFirstName.Text = client.FirstName;
            TbPatronymic.Text = client.Patronymic;
            DpDateOfBirthday.SelectedDate = client.DateOfBirthday;
            CbPrivilege.SelectedValue = client.PrivilegeId;
            TbPhoneNumber.Text = client.PhoneNumber;
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
                imgClient.Source = new BitmapImage(new Uri(fileDialog.FileName));
                CheckPhotoName();
            }
        }

        private void CheckPhotoName()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Images\Clients\" + imgName))
            {
                for (int i = 1; i < Int32.MaxValue; i++)
                {
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Images\Clients\" + imgName))
                    { imgName = $"({i})" + imgName; }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private bool CheckDate(DateTime now, DateTime birthDate)
        {
            int age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                age--;
            if (age < 18)
                return false;
            return true;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CheckFields()))
            {
                MessageBox.Show(CheckFields());
                return;
            }
            if (!CheckDate(DateTime.Now, (DateTime)DpDateOfBirthday.SelectedDate))
            {
                MessageBox.Show("Возраст клиента должен быть не менее 18 лет", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (client == null) Add();
            else Update();

            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new MainPage();
        }

        private void Add()
        {

            try
            {

                if (!string.IsNullOrWhiteSpace(imgName))
                {
                    string dest = AppDomain.CurrentDomain.BaseDirectory + @"\Images\Clients\" + imgName;
                    File.Copy(imgPath, dest);
                }
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    client = new Client()
                    {
                        Photo = imgName,
                        SecondName = TbSecondName.Text,
                        FirstName = TbFirstName.Text,
                        Patronymic = TbPatronymic.Text,
                        DateOfBirthday = DpDateOfBirthday.DisplayDate,
                        PhoneNumber = TbPhoneNumber.Text,
                        PrivilegeId = 1,
                        UserId = MainWindow.UserId
                    };
                    db.Clients.Add(client);
                    db.SaveChanges();
                    MessageBox.Show("Анкета добавлена");
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
                    string dest = AppDomain.CurrentDomain.BaseDirectory + @"\Images\Clients\" + imgName;
                    File.Copy(imgPath, dest);
                }

                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    Client client1 = db.Clients.FirstOrDefault(x => x.ClientId == client.ClientId);
                    db.Clients.Attach(client1);

                    if (!string.IsNullOrWhiteSpace(imgName)) client1.Photo = imgName;
                    client1.SecondName = TbSecondName.Text;
                    client1.FirstName = TbFirstName.Text;
                    client1.Patronymic = TbPatronymic.Text;
                    client1.DateOfBirthday = DpDateOfBirthday.DisplayDate;
                    client1.PhoneNumber = TbPhoneNumber.Text;
                    client1.PrivilegeId = (int)CbPrivilege.SelectedValue;
                    db.SaveChanges();
                }
                MessageBox.Show("Информация обновлена");
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
            if (DpDateOfBirthday.DisplayDate == null) message += "Введите дату рождения" + Environment.NewLine;
            else if (string.IsNullOrWhiteSpace(TbPhoneNumber.Text)) message += "Введите номер телефона" + Environment.NewLine;
            return message;
        }
        private void TbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new MainPage();
        }
    }
}
