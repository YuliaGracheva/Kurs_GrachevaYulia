using kursovoi.Helper;
using kursovoi.Model;
using kursovoi.View;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace kursovoi.ViewModel
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Client> ListClient { get; set; }
        public static int ClientId;

        public int MaxId()
        {
            int max = 0;
            if (this.ListClient != null)
            {
                foreach (var cl in this.ListClient)
                {
                    if (max < cl.ClientId) max = cl.ClientId;
                }
            }
            return max;
        }

        public ClientViewModel()
        {
            // Строка подключения к SQLite
            string connectionString = "Data Source=D:\\kursovoi\\kursovoi\\BD\\salonBeauty.db";

            List<Client> clients = MyDbContext.GetEntities<Client>(connectionString, "SELECT * FROM Clients");

            // Преобразование в ObservableCollection
            ListClient = new ObservableCollection<Client>(clients);
        }

        private RelayCommand editClient;
        public RelayCommand EditClient
        {
            get
            {
                return editClient ??
                (editClient = new RelayCommand(obj =>
                {
                    WindowNewClient wnClient = new WindowNewClient
                    { Title = "Редактирование клиента" };

                    Client client = SelectedClient;
                    Client tempClient = new Client();

                    tempClient = client.ShallowCopy();
                    wnClient.DataContext = tempClient;
                    if (wnClient.ShowDialog() == true)
                    {
                        // сохранение данных в оперативной памяти
                        client.ClientName = tempClient.ClientName;
                        client.ClientSurname = tempClient.ClientSurname;
                        client.ClientContactNumber = tempClient.ClientContactNumber;
                        MyDbContext dbContext = new MyDbContext();
                        dbContext.UpdateEntity<Client>(tempClient);
                    }
                }, (obj) => SelectedClient != null && ListClient.Count > 0));
            }
        }


        private Client selectedClient;
        public Client SelectedClient
        {
            get
            {
                return selectedClient;
            }
            set
            {
                selectedClient = value;
                OnPropertyChanged("SelectedClient");
                EditClient.CanExecute(true);
            }
        }

        private RelayCommand addClient;
        public RelayCommand AddClient
        {
            get
            {
                return addClient ??
                 (addClient = new RelayCommand(obj =>
                 {
                     WindowNewClient wnClient = new WindowNewClient
                     {
                         Title = "Новый клиент",
                     };
                     int maxIdClient = MaxId() + 1;
                     Client client = new Client { ClientId = maxIdClient };
                     wnClient.DataContext = client;
                     if (wnClient.ShowDialog() == true)
                     {
                         ListClient.Add(client);
                         MyDbContext dbContext = new MyDbContext();
                         dbContext.SaveEntity<Client>(client);
                     }
                     SelectedClient = client;
                 }));
            }
        }

        private RelayCommand deleteClient;
        public RelayCommand DeleteClient
        {
            get
            {
                return deleteClient ??
                (deleteClient = new RelayCommand(obj =>
                {
                    Client client = SelectedClient;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по фамилии клиента: " + client.ClientSurname, "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        ListClient.Remove(client);
                        MyDbContext dbContext = new MyDbContext();
                        dbContext.DeleteEntityFromDatabase<Client>(client);
                    }
                }, (obj) => SelectedClient != null && ListClient.Count > 0));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
