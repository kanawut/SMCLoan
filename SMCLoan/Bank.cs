using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMCLoan
{
    public class Bank
    {
        public string Name { get; set; }
        public decimal IntersetRate { get; set; }


        public Bank(string name, decimal interestRate)
        {
            this.Name = name;
            this.IntersetRate = interestRate;
        }
    }
}
