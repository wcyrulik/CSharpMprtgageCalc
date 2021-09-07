using CSharpMprtgageCalc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpMprtgageCalc.wwwroot.Helpers
{
    public class LoanHelper
    {
        public Loan GetPayments(Loan loan)
        {
            // Calculate my monthly payment
            loan.Payment = CalcPayment(loan.Amount, loan.Rate, loan.Term);

            // Create a loop 1 to term
            var balance = loan.Amount;
            var totalInterest = 0.0m;
            var monthlyInterest = 0.0m;
            var monthlyPrincipal = 0.0m;
            var monthlyRate = CalcMonthlyRate(loan.Rate);

            for (int month = 1; month <= loan.Term; month++)
            {
                monthlyInterest = CalcMonthlyInterest(balance, monthlyRate);
                totalInterest += monthlyInterest;
                monthlyPrincipal = loan.Payment - monthlyInterest;
                balance -= monthlyPrincipal;

                LoanPayment loanPayment = new();

                loanPayment.Month = month;
                loanPayment.Payment = loan.Payment;
                loanPayment.MonthlyPrincipal = monthlyPrincipal;
                loanPayment.MonthlyInterest = monthlyInterest;
                loanPayment.TotalInterest = totalInterest;
                loanPayment.Balance = balance;

                loan.Payments.Add(loanPayment);
            }
            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.Amount + totalInterest;

            return (loan);
        }

        private decimal CalcPayment(decimal amount, decimal rate, int term)
        {
            decimal payment = 0.0m;
            decimal monthlyrate = CalcMonthlyRate(rate);

            var rateD = Convert.ToDouble(monthlyrate);
            var amountD = Convert.ToDouble(amount);

            var paymentD = (amountD * rateD) / (1 - Math.Pow(1 + rateD, -term));

            return Convert.ToDecimal(paymentD);
        }

        private decimal CalcMonthlyRate(decimal rate)
        {
            return rate / 1200;
        }

        private decimal CalcMonthlyInterest(decimal balance, decimal monthlyRate)
        {
            return balance * monthlyRate;
        }
    }
}
