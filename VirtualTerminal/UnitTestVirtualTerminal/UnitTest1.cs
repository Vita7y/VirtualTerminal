using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualTerminal.Core;

namespace UnitTestVirtualTerminal
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var human = new Human();
            Assert.AreEqual((uint)0, human.Total());

            human.AddToWallet(new List<SameCoin>
            {
                new SameCoin(1, 1, "1"), 
                new SameCoin(2, 1, "2"), 
                new SameCoin(5, 1, "5"),
                new SameCoin(10, 1, "10")
            });
            Assert.AreEqual((uint)18, human.Total());
        }

        [TestMethod]
        public void TestMethod2()
        {
            var vt = new VirtualTerminal.Core.VirtualTerminal();
            vt.AddIncomeMoney(new SameCoin(1,3,"1"));
            Assert.AreEqual((uint)3, vt.IncomeMoney);
            
            vt.AddIncomeMoney(new SameCoin(10, 2, "1"));
            Assert.AreEqual((uint)23, vt.IncomeMoney);

            vt.CoinWallet.Add(new SameCoin(1, 10, "1"));
            vt.CoinWallet.Add(new SameCoin(2, 10, "2"));
            vt.CoinWallet.Add(new SameCoin(5, 10, "5"));
            vt.CoinWallet.Add(new SameCoin(10, 10, "10"));

            var odd = vt.GetOddMoney();
            Assert.IsNotNull(odd);
            Assert.AreEqual(3, odd.Count);

            uint money = odd.Aggregate<SameCoin, uint>(0, (current, coin) => current + coin.Price*coin.Count);
            Assert.AreEqual((uint)23, money);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var human = new Human();
            human.AddToWallet(new List<SameCoin>
            {
                new SameCoin(1, 10, "1"), 
                new SameCoin(2, 30, "2"), 
                new SameCoin(5, 20, "5"),
                new SameCoin(10, 15, "10")
            });
            var total = human.Total();

            var vt = new VirtualTerminal.Core.VirtualTerminal();
            vt.CoinWallet.Add(new SameCoin(1, 100, "1"));
            vt.CoinWallet.Add(new SameCoin(2, 100, "2"));
            vt.CoinWallet.Add(new SameCoin(5, 100, "5"));
            vt.CoinWallet.Add(new SameCoin(10, 100, "10"));

            vt.ProductList.Add(new Product(0,13,10,"Tea") );
            vt.ProductList.Add(new Product(1, 18, 20, "Coffee"));
            vt.ProductList.Add(new Product(2, 21, 20, "Coffee with milk"));
            vt.ProductList.Add(new Product(3, 35, 15, "Juice"));

            vt.OnGetMoneyHandler += human.AddToWallet;
            human.OnSendMoney += vt.AddIncomeMoney;

            human.DeleteFromWallet(1);
            human.DeleteFromWallet(10);
            human.DeleteFromWallet(10);
            human.DeleteFromWallet(5);
            Assert.AreEqual((uint)26, vt.IncomeMoney);
            var total1 = human.Total();
            Assert.AreEqual(total-26, total1);

            var product = vt.Buy(1);
            Assert.IsNotNull(product);
            Assert.AreEqual((uint)8, vt.IncomeMoney);
            Assert.AreEqual((uint)19, vt.ProductList[1].Count);

            vt.SendOddMoney();
            Assert.AreEqual((uint)0, vt.IncomeMoney);
            var total2 = human.Total();
            Assert.AreEqual(total1+8, total2);
        }

    }
}
