using SMCLoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Should;


namespace SMCLoanFacts
{
   public class LoanFacts
    {
        public class PayMethod
        {

            

            private ITestOutputHelper output;

            public PayMethod(ITestOutputHelper output)
            {
                this.output = output;
            }

            [Fact]
            public void DifferentYearPayment_HaveCorrectDays()
            {
                var loan = new Loan(pricipal: 1000000, outstanding: 1000000,
                bank: new Bank("TEST BANK", 7.0M),
                lastpaydate: new DateTime(2014, 12, 20));
                var p = loan.Pay(new DateTime(2015, 1, 20), 10000);

                outputPayment(p);

                Assert.Equal(31, p.Days);
            }


            private void outputPayment(Payment p)
            {
                  output.WriteLine("{0:s}\t{1} days\r\nPaid: {2:n2}\r\nPrincipal: {3:n2}\r\nInterest: {4:n2}\r\nOutstanding: {5:n2}",
                  p.PaidDate, p.Days,
                  p.PaidAmount, p.PaidPrincipalAmount, p.InterestAmount,
                  p.Outstanding);
            }


            [Fact]
            public void FirstPay()
            { 
                //arrange

                Bank bank = new Bank("SMC", 7.0m);                
     

                Loan loan = new Loan(pricipal: 1000000, outstanding: 1000000 , bank: bank , lastpaydate:new DateTime(2015, 7, 1) );


                //act
                var d = new DateTime(2015, 8, 1);
                var payment = loan.Pay(d, 10000);


                

                // assert
                Assert.NotNull(payment);
                Assert.Equal(31, payment.Days);

                Assert.Equal(10000, payment.PaidAmount);
                Assert.Equal(d, payment.PaidDate);

                Assert.Equal(5945.21m, payment.InterestAmount);
                Assert.Equal(4054.79m, payment.PaidPrincipalAmount);



               
            }

            [Fact]
            public void LoanShouldStorePaymentAfterPaid()
            {
                Loan loan = new Loan(
                         pricipal: 1000000,
                         outstanding: 1000000,
                         bank: new Bank("TEST BANK", 7.0M),
                         lastpaydate: new DateTime(2015, 7, 1)); // 1 Jul, 2015.  

                var p = loan.Pay(new DateTime(2015, 8, 1), 10000);

                loan.Payments.ShouldNotBeNull();
                loan.Payments.Count().ShouldEqual(1);
                loan.Payments.ShouldContain(p);
            }


            [Fact]
            public void PayTwiceInTheSameDay()
            {
                //arrange

                Bank bank = new Bank("SMC", 7.0m);


                Loan loan = new Loan(pricipal: 1000000, outstanding: 1000000, bank: bank, lastpaydate: new DateTime(2015, 7, 1));


                //act
                var d = new DateTime(2015, 8, 1);
                var payment = loan.Pay(d, 10000);
                var payment2 = loan.Pay(d, 10000);


                // assert
                Assert.NotNull(payment);
                Assert.Equal(31, payment.Days);

                Assert.Equal(10000, payment.PaidAmount);
                Assert.Equal(d, payment.PaidDate);

                Assert.Equal(5945.21m, payment.InterestAmount);
                Assert.Equal(4054.79m, payment.PaidPrincipalAmount);


                Assert.NotNull(payment2);
                Assert.Equal(0, payment2.Days);

                Assert.Equal(10000, payment2.PaidAmount);
                Assert.Equal(d, payment2.PaidDate);

                Assert.Equal(0m, payment2.InterestAmount);
                Assert.Equal(10000m, payment2.PaidPrincipalAmount);



            }


            [Fact]
            public void PaidAmountIsNotEnoughForInterest()
            {
                // arrange       
                Loan loan = new Loan(
                  pricipal: 1000000,
                  outstanding: 1000000,
                  bank: new Bank("TEST BANK", 7.0M),
                  lastpaydate: new DateTime(2015, 7, 1)); // 1 Jul, 2015.          

                // act
                var d1 = new DateTime(2015, 8, 1);
                var payment1 = loan.Pay(d1, 3000);

                var d2 = new DateTime(2015, 9, 1);
                var payment2 = loan.Pay(d2, 10000);

                // assert
                Assert.NotNull(d1);
                Assert.Equal(31, payment1.Days);
                Assert.Equal(3000m, payment1.PaidAmount);
                Assert.Equal(d1, payment1.PaidDate);

                Assert.Equal(3000m, payment1.InterestAmount);
                Assert.Equal(0m, payment1.PaidPrincipalAmount);
                Assert.Equal(1000000m, payment1.Outstanding);

                Assert.Equal(2945.21m, loan.Accrued);

                Assert.NotNull(payment2);
                Assert.Equal(31, payment2.Days);
                Assert.Equal(10000, payment2.PaidAmount);
                Assert.Equal(d2, payment2.PaidDate);

                Assert.Equal(5945.21m, payment2.InterestAmount);
                Assert.Equal(4054.79m, payment2.PaidPrincipalAmount);
                Assert.Equal(995945.21m, payment2.Outstanding);
                Assert.Equal(2945.21m, loan.Accrued);

                Assert.Equal(d2, loan.LastPaydate);
                Assert.Equal(995945.21m, loan.Outstanding);
            }
        }
    }
}
