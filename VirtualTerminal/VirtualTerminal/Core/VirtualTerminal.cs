using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using VirtualTerminal.Annotations;

namespace VirtualTerminal.Core
{
    public delegate void SendMessage(object sender, string message);

    public class VirtualTerminal : INotifyPropertyChanged
    {
        public VirtualTerminal()
        {
            ProductList = new BindingList<Product>();
            CoinWallet = new BindingList<SameCoin>();
            IncomeMoney = 0;
        }

        public event EventHandler OnDataUpdate;

        public event SendMoneyHandler OnGetMoneyHandler;

        public event SendMessage OnSendMessage;

        private BindingList<Product> _productList;

        public BindingList<Product> ProductList
        {
            get { return _productList; }
            private set
            {
                _productList = value;
                OnPropertyChanged("ProductList");
            }
        }

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

        private uint _incomeMoney;
        public uint IncomeMoney
        {
            get { return _incomeMoney; }
            private set
            {
                _incomeMoney = value;
                OnPropertyChanged("IncomeMoney");
            }
        }

        public void AddIncomeMoney(object sender, List<SameCoin> money)
        {
            foreach (var sameCoin in money)
            {
                AddIncomeMoney(sameCoin);
            }
        }

        public void AddIncomeMoney(SameCoin sameCoin)
        {
            //var find = CoinWallet.FirstOrDefault(am => am.Price == sameCoin.Price);
            //if(find==null)
            //    CoinWallet.Add(new SameCoin(sameCoin.Price, sameCoin.Count, sameCoin.Description));
            //else
            //    find.Add(sameCoin.Count);
            IncomeMoney += sameCoin.Price*sameCoin.Count;

            if (OnDataUpdate != null)
                OnDataUpdate(this, EventArgs.Empty);
        }

        public List<SameCoin> GetOddMoney()
        {
            if (IncomeMoney == 0)
                return null;

            var odd = new List<SameCoin>();
            CoinWallet.Sort(SameCoin.CompareByPrice);

            foreach (var coin in CoinWallet)
            {
                var count = IncomeMoney/coin.Price;
                if (count > 0
                    && coin.Count >= count)
                {
                    odd.Add(new SameCoin(coin.Price, count, coin.Description));
                    coin.Delete(count);
                    IncomeMoney -= count*coin.Price;
                }
            }

            if (OnDataUpdate != null)
                OnDataUpdate(this, EventArgs.Empty);

            return odd;
        }

        public void SendOddMoney()
        {
            if (OnGetMoneyHandler != null)
                OnGetMoneyHandler(this, GetOddMoney());
        }

        public Product Buy(uint number)
        {
            var product = ProductList.FirstOrDefault(am => am.Number == number);
            if (product == null
                || product.Price > IncomeMoney
                || product.Count == 0)
            {
                if (OnSendMessage != null)
                    OnSendMessage(this, "Недостаточно средств");
                return null;
            }

            product.Delete(1);
            IncomeMoney -= product.Price;

            if (OnDataUpdate != null)
                OnDataUpdate(this, EventArgs.Empty);
            if (OnSendMessage != null)
                OnSendMessage(this, "Спасибо!");

            return new Product(product.Number, product.Price, 1, product.Description);
        }

        public bool CheckMoneyToBuy(uint number)
        {
            var product = ProductList.FirstOrDefault(am => am.Number == number);
            return product != null && product.Price <= IncomeMoney;
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