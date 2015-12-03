using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using VirtualTerminal.Annotations;

namespace VirtualTerminal.Core
{

    public class Human : INotifyPropertyChanged
    {
        public Human()
        {
            CoinWallet = new BindingList<SameCoin>();
        }

        public event EventHandler DataUpdate;

        public event SendMoneyHandler OnSendMoney;

        private BindingList<SameCoin> _coinWallet;

        public BindingList<SameCoin> CoinWallet
        {
            get
            {
                return _coinWallet;
            }
            private set
            {
                _coinWallet = value;
                OnPropertyChanged("CoinWallet");
            }
        }

        public void AddToWallet(object sender, List<SameCoin> odd)
        {
            AddToWallet(odd);
        }

        public void AddToWallet(List<SameCoin> income)
        {
            foreach (var coin in income)
            {
                var find = CoinWallet.FirstOrDefault(am => am.Price == coin.Price);
                if (find == null)
                    CoinWallet.Add(new SameCoin(coin.Price, coin.Count, coin.Description));
                else
                    find.Add(coin.Count);
            }

            if (DataUpdate != null)
                DataUpdate(this, EventArgs.Empty);
        }

        public bool DeleteFromWallet(uint price)
        {
            var find = CoinWallet.FirstOrDefault(am => am.Price == price);
            if (find == null
                || find.Count == 0)
                return false;

            var res = find.Delete(1);
            if (!res)
                return false;

            if (OnSendMoney != null)
                OnSendMoney(this, new List<SameCoin> {new SameCoin(find.Price, 1, find.Description)});
            if (DataUpdate != null)
                DataUpdate(this, EventArgs.Empty);

            return true;
        }

        public uint Total()
        {
            return CoinWallet.Aggregate<SameCoin, uint>(0, (current, coin) => current + coin.Price*coin.Count);
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