using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem
{
    [Serializable]
    public class DepositAccount : Account
    {
    public Guid Id { get; set; }

    public double Money { get; set; }

    public double Percent { get; set; }

    public DateTime Time { get; set; }

        public string Title = "Депозитный счет";

        public DateTime EndTime { get; set; }

    public DateTime StartTime { get; set; }

    public AccountType accountType { get; set; }
    public double VerifiedLimit { get; set; }


    public DepositAccount (double money, double percent, DateTime time, AccountType type, double Limit)
        {
            Id = Guid.NewGuid();
            Percent = percent;
            Money = money;
            Time = time;
            VerifiedLimit = Limit;
            accountType = type;
        }


    public void AddPercent()
        {
            if (Time > EndTime)
            {
                return;
            }
            TimeSpan difference = Time - StartTime;
            if (difference.TotalDays % 30 == 0)
            {
                StartTime = Time;
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
            if (DateTime.Now < Time)
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
            else
            {
                throw new Exception("is not available yet");
            }
        }

    public IOperations Transfer(double money, Account acc)
        {
            if (DateTime.Now < Time)
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
            else
            {
                throw new Exception("is not available yet");
            }
        }

    }
}
