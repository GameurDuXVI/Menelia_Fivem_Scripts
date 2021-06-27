using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeneliaAPI.Entities;

namespace MeneliaAPI.Entities
{
    public class Banking
    {
        public int Money;
        public List<Transaction> Transactions;

        public Banking()
        {
            this.Money = 0;
            this.Transactions = new List<Transaction>();
        }

        public Banking(int Money, List<Transaction> Transactions)
        {
            this.Money = Money;
            this.Transactions = Transactions;
        }
    }
}
