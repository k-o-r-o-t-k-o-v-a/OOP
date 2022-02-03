using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem
{
    [Serializable]
    public class CreditAccount : Account
    {
        public Guid Id { get; set; }

        public double Money { get; set; }

        public double CreditLimit { get; set; }

        public double VerifiedLimit { get; set; }

        public string Title = "Кредитный счет";
        
        public AccountType accountType { get; set; }

        public double Commission { get; set; }

        public CreditAccount(double money, double creditlimit, double commission, AccountType type, double VerifiedLim)
        {
            Id = Guid.NewGuid();
            Money = money;
            CreditLimit = creditlimit;
            VerifiedLimit = VerifiedLim;
            Commission = commission;
            accountType = type;

        }
        public void AddPercent() {
            return;
        }

        public IOperations Add(double money)

        {
            return new OperationAdd(this, money);
        }

        public IOperations Withdraw(double money)

        {
            if (money < CreditLimit)
            {
                throw new Exception("not enough credit");
            }

            //добавила
            if /*((accountType == AccountType.IsNotVerified) &*/ (money > VerifiedLimit)
            {
                throw new Exception("you are not verified to deal with so much money");
            }

            //добавила
            /*if (money > VerifiedLimit)
            {
                throw new Exception("too much money for not verified account");
            }
            */
            else
            {
                if (money > 0)//Money
                {
                    if (money > Money)
                    {
                        throw new Exception("not enough money");
                    }
                    return new OperationWithdraw(this, money);
                }
                else
                {
                    money = money * (1 + Commission);
                    return new OperationWithdraw(this, money);

                }
            }
        }

        public IOperations Transfer(double money, Account acc)
        {
            if (money < CreditLimit)
            {
                throw new Exception("not enough credit");
            }
            if (money > VerifiedLimit)
            {
                throw new Exception("you are not verified to deal with so much money");
            }
            else
            {
                if (money > 0)
                {
                    if (money > Money)
                    {
                        throw new Exception("not enough money");
                    }
                    return new OperationTransfer(this, acc, money);
                }
                else
                {
                    money = money * (1 + Commission);
                    return new OperationTransfer(this, acc, money);

                }
            };
        }
    }
}
