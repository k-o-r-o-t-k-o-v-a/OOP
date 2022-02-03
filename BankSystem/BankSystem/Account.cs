using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem
{
    public enum AccountType
    {
        IsVerified,
        IsNotVerified
    }

    public interface Account
    {
        Guid Id { get; set; }

        double Money { get; set; }
       
        double VerifiedLimit { get; set; }


        AccountType accountType { get; set; }

        void AddPercent();

        IOperations Add(double money);
        IOperations Withdraw(double money);

        IOperations Transfer(double money, Account acc);
    }
}
