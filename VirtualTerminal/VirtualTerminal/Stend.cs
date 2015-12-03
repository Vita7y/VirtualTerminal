using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using VirtualTerminal.Core;

namespace VirtualTerminal
{
    public partial class Stend : Form
    {
        private readonly Human _human = new Human();
        private readonly Core.VirtualTerminal _vt = new Core.VirtualTerminal();

        public Stend()
        {
            InitializeComponent();
            
            init();

            textEdit1.DataBindings.Add("Text", _vt, "IncomeMoney");
            gridControl1.DataBindings.Add("DataSource", _human, "CoinWallet");
            gridControl2.DataBindings.Add("DataSource", _vt, "ProductList");
            gridControl3.DataBindings.Add("DataSource", _vt, "CoinWallet");
            gridControl1.DataSource = _human.CoinWallet;
            gridControl2.DataSource = _vt.ProductList;
            gridControl3.DataSource = _vt.CoinWallet;
        }

        private void init()
        {
            _human.AddToWallet(new List<SameCoin>
            {
                new SameCoin(1, 10, "1"),
                new SameCoin(2, 30, "2"),
                new SameCoin(5, 20, "5"),
                new SameCoin(10, 15, "10")
            });
            _vt.CoinWallet.Add(new SameCoin(1, 100, "1"));
            _vt.CoinWallet.Add(new SameCoin(2, 100, "2"));
            _vt.CoinWallet.Add(new SameCoin(5, 100, "5"));
            _vt.CoinWallet.Add(new SameCoin(10, 100, "10"));

            _vt.ProductList.Add(new Product(0, 13, 10, "Tea"));
            _vt.ProductList.Add(new Product(1, 18, 20, "Coffee"));
            _vt.ProductList.Add(new Product(2, 21, 20, "Coffee with milk"));
            _vt.ProductList.Add(new Product(3, 35, 15, "Juice"));

            _vt.OnGetMoneyHandler += _human.AddToWallet;
            _vt.OnDataUpdate += OnDataUpdate;
            _vt.OnSendMessage += (sender, message) => MessageBox.Show(this, message);

            _human.OnSendMoney += _vt.AddIncomeMoney;
            _human.DataUpdate += OnDataUpdate;
        }

        private void OnDataUpdate(object sender, EventArgs eventArgs)
        {
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            var coin = gridView1.GetFocusedRow() as SameCoin;
            if (coin == null)
                return;
            _human.DeleteFromWallet(coin.Price);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            var product = gridView2.GetFocusedRow() as Product;
            if (product == null)
                return;
            _vt.Buy(product.Number);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            _vt.SendOddMoney();
        }
    }
}
