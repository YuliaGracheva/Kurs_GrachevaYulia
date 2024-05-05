using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace kursovoi.Model
{
    public class Client : INotifyPropertyChanged
    {
        private string _name;
        private string _surname;
        private string _contactNumber;

        [Key]
        public int ClientId { get; set; }
        public string ClientName 
        {
            get { return _name; }
            set 
            {  
                _name = value;
                OnPropertyChanged("ClientName");
            } 
        }
        public string ClientSurname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                OnPropertyChanged("ClientSurname");
            }
        }
        public string ClientContactNumber
        {
            get { return _contactNumber; }
            set
            {
                _contactNumber = value;
                OnPropertyChanged("ClientContactNumber");
            }
        }

        public Client() { }
        public Client(int clientId, string clientName, string clientSurname, string clientContactNumber)
        {
            ClientId = clientId;
            ClientName = clientName;
            ClientSurname = clientSurname;
            ClientContactNumber = clientContactNumber;
        }
        public Client ShallowCopy()
        {
            return (Client)this.MemberwiseClone();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
