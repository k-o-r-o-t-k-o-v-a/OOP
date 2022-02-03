using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem
{
    [Serializable]
    public class DebitAccount : Account
    {
        public Guid Id { get; set; }

        public double Money { get; set; }

        public double Percent { get; set; }
        public AccountType accountType { get; set; }

        public string Title = "Дебитовый счет";

        public double VerifiedLimit { get; set; }

        public DateTime StartTime { get; set; }

        public DebitAccount(double money, double percent, DateTime startTime, AccountType type, double limit )
        {
            Id = Guid.NewGuid();
            Money = money;
            Percent = percent;
            StartTime = startTime;
            VerifiedLimit = limit;
        }

        public void AddPercent()
        {
            
            TimeSpan difference = DateTime.Now - StartTime;
            if (difference.TotalDays % 30 == 0)
            {
                StartTime = DateTime.Now;
                Money += Money * Percent * difference.TotalDays;
            }
        }


        public IOperations Add(double money)

        {
            if (money > VerifiedLimit)
            {
                throw new Exception("you are not verified to deal with so much money");
            }
            return new OperationAdd(this, money);
        }

        public IOperations Withdraw(double money)

        {
            if (money > Money)
            {
                throw new Exception("not enough money");
            }
            if (money > VerifiedLimit)
            {
                throw new Exception("you are not verified to deal with so much money");
            }
            return new OperationWithdraw(this, money);
        }

        public IOperations Transfer(double money, Account acc)
        {
            if (money > Money)
            {
                throw new Exception("not enough money");
            }
            if (money > VerifiedLimit)
            {
                throw new Exception("you are not verified to deal with so much money");
            }
            return new OperationTransfer(this, acc, money);
        }
    }
    
}
