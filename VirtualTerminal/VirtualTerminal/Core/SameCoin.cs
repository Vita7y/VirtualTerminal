using System.Collections.Generic;
using System.ComponentModel;
using VirtualTerminal.Annotations;

namespace VirtualTerminal.Core
{
    public delegate void SendMoneyHandler(object sender, List<SameCoin> money);

    public class SameCoin : INotifyPropertyChanged
    {
        public SameCoin(uint price, uint count, string description)
        {
            Price = price;
            Count = count;
            Description = description;
        }

        private uint _price;
        public uint Price { get { return _price; } private set { _price = value;OnPropertyChanged("Price"); } }

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

        public static int CompareByPrice(SameCoin x, SameCoin y)
        {
            if (x == null)
            {
                if (y == null)
                    return 0;
                return -1;
            }
            if (y == null)
                return 1;

            return y.Price.CompareTo(x.Price);
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