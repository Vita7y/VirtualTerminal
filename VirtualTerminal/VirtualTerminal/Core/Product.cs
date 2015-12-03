using System.ComponentModel;
using VirtualTerminal.Annotations;

namespace VirtualTerminal.Core
{
    public class Product : INotifyPropertyChanged
    {
        public Product(uint number, uint price, uint count, string description)
        {
            Number = number;
            Price = price;
            Count = count;
            Description = description;
        }

        private uint _number;
        public uint Number { get{return _number;} private set { _number = value; OnPropertyChanged("Number"); } }

        private uint _price;
        public uint Price { get { return _price; } private set { _price = value; OnPropertyChanged("Price"); } }

        private uint _count;
        public uint Count { get { return _count; } private set { _count = value; OnPropertyChanged("Count"); } }

        private string _description;
        public string Description { get { return _description; } private set { _description = value; OnPropertyChanged("Description"); } }

        public void Add(uint count)
        {
            Count += count;
        }

        public bool Delete(uint count)
        {
            if (count > Count)
                return false;
            Count -= count;
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}