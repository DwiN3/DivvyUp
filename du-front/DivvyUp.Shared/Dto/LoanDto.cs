using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivvyUp_Shared.Dto
{
    public class LoanDto
    {
        public int id;
        public int personId;
        public DateTime date;
        public double amount;
        public bool lent;
        public bool settled;
        public PersonDto person { get; set; }

        public LoanDto()
        {
            id = 0;
            personId = 0;
            date = DateTime.Now;
            amount = 0;
            lent = false;
            settled = false;
        }

        public LoanDto(int id, int personId, DateTime date, double amount, bool lent, bool settled)
        {
            this.id = id;
            this.personId = personId;
            this.date = date;
            this.amount = amount;
            this.lent = lent;
            this.settled = settled;
        }
    }
}
