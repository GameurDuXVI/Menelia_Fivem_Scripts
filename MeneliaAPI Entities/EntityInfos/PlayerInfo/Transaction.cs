using System;

namespace Menelia.Entities
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
