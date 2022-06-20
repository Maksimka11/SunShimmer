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
    public partial class ClientAllPage : Page
    {
        public ClientAllPage()
        {
            InitializeComponent();
            LoadData();

        }

        private void LoadData()
        {
            using (SunShimmerEntities db = new SunShimmerEntities())
            {
                db.Clients.Load();
                ICollectionView view = new CollectionView(db.Clients.Include(x => x.Privilege).ToList());
                DgClients.ItemsSource = view;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new AdminPage();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgClients.SelectedItem == null) return;
            Client client = DgClients.SelectedItem as Client;

            MainWindow window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.Frame.Content = new ClientEditPage(client.UserId);
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DgClients.SelectedItems.Count < 1) return;
            else if (MessageBox.Show("Вы уверены?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (SunShimmerEntities db = new SunShimmerEntities())
                {
                    try
                    {
                        for (int i = 0; i < DgClients.SelectedItems.Count; i++)
                        {
                            Client client = DgClients.SelectedItems[i] as Client;
                            Client client1 = db.Clients.FirstOrDefault(x => x.ClientId == client.ClientId);
                            db.Clients.Remove(client1);
                            db.SaveChanges();
                            MessageBox.Show("Запись удалена");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        db.Dispose();
                    }
                    finally
                    { 
                        LoadData(); 
                    }
                }
            }
        }
    }
}
