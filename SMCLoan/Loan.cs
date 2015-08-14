using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMCLoan
{
    public class Loan
    {
        public decimal Pricipal {get;set; }
        public decimal  Outstanding { get; set; }
        public DateTime       LastPaydate { get; set; }
        public Bank PreviousOwner { get; set; }
        public decimal Accrued { get; set; }

        public Loan(decimal pricipal, decimal outstanding, Bank bank, DateTime lastpaydate)
        {
            this.Pricipal = pricipal;
            this.Outstanding = outstanding;
            this.PreviousOwner = bank;
            this.LastPaydate = lastpaydate;
        }

        public Payment Pay(DateTime date , decimal amount)
        {
            var payment = new Payment();
            payment.Days = ( date.Date - this.LastPaydate.Date).Days;
            payment.PaidAmount = amount;
            payment.PaidDate = date;


            payment.InterestAmount = Math.Round( (Outstanding * PreviousOwner.IntersetRate * payment.Days) / 36500 , 2 ,MidpointRounding.AwayFromZero );

            decimal tempInterestAmount = payment.InterestAmount;

            if (payment.InterestAmount > amount)
            {
                payment.InterestAmount = amount;
            }

            if(amount - payment.InterestAmount < 0   ) 
            {
                payment.PaidPrincipalAmount = 0;
            }
                else
                {
                 payment.PaidPrincipalAmount = (amount - payment.InterestAmount);
                }

            payment.Outstanding = this.Outstanding - payment.PaidPrincipalAmount;

            this.LastPaydate = date;
            this.Outstanding = payment.Outstanding;

            if (payment.PaidPrincipalAmount == 0.0m)
            {
                this.Accrued = this.Accrued + ( tempInterestAmount - payment.PaidAmount);
            }

            return payment;
        }
    }
    
}
