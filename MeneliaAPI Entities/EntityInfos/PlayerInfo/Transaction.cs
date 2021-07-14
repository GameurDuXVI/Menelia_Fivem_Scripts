using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeneliaAPI.Entities;

namespace MeneliaAPI.Entities
{
    public class Transaction
    {
        public DateTime Date;
        public String Receiver;
        public String Description;
        public int Amount;

        public Transaction(DateTime Date, String Receiver, String Description, int Amount)
        {
            this.Date = Date;
            this.Receiver = Receiver;
            this.Description = Description;
            this.Amount = Amount;
        }
    }
}
