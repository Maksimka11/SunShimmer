using SunShimmer.Model;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SunShimmer.Pages
{
    public partial class PrivilegeEditPage : Page
    {
        public PrivilegeEditPage()
        {
            InitializeComponent();
        }
        Privilege privilege = null;

        public PrivilegeEditPage(int id)
        {
            InitializeComponent();

            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                privilege = db.Privileges.FirstOrDefault(x => x.PrivilegeId == id);
            }
            LoadData();
        }

        private void LoadData()
        {
            TbPrivilegeName.Text = privilege.PrivilegeName;
            TbSale.Text = privilege.Sale;
        }

        private void Add()
        {
            try
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    Privilege privilege = db.Privileges.FirstOrDefault(x => x.PrivilegeName == TbPrivilegeName.Text);
                    if (privilege != null)
                    {
                        MessageBox.Show("Этот уровень уже существует");
                        return;
                    }

                    privilege = new Privilege()
                    {
                        PrivilegeName = TbPrivilegeName.Text,
                        Sale = TbSale.Text,
                    };
                    db.Privileges.Add(privilege);
                    db.SaveChanges();
                }
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    Privilege privilege = db.Privileges.FirstOrDefault(x => x.PrivilegeName == TbPrivilegeName.Text);
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
                    Privilege privilege1 = db.Privileges.FirstOrDefault(x => x.PrivilegeId == privilege.PrivilegeId);
                    db.Privileges.Attach(privilege1);
                    privilege1.PrivilegeName = TbPrivilegeName.Text;
                    privilege1.Sale = TbSale.Text;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void TbSale_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private string CheckFields()
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(TbPrivilegeName.Text)) message += "Введите наименование привилегии" + Environment.NewLine;
            else if (string.IsNullOrWhiteSpace(TbSale.Text)) message += "Введите процент скидки" + Environment.NewLine;
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
            if (privilege == null) Add();
            else Update();

            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new PrivilegeAllPage();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new PrivilegeAllPage();
        }
    }
}
