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
        DateTime date;
        String description;
        int amount;

        public Transaction(DateTime date, String description, int amount)
        {
            this.date = date;
            this.description = description;
            this.amount = amount;
        }
    }
}
