using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem
{
    [Serializable]
    public class OperationTransfer : IOperations
    {
        public Guid OperationId { get; set; }

        public double Money { get; set; }

        public Account account { get; set; }

        public Account account2 { get; set; }

        public OperationTransfer(Account acc, Account acc2, double money)
        {
            account = acc;
            account2 = acc2;
            Money = money;
            OperationId = Guid.NewGuid();
            Operate();

        }

        public void Operate()
        {
            account.Money -= Money;
            account2.Money += Money;
        }

        public void Close()
        {
            account.Money += Money;
            account2.Money -= Money;
        }
    }
}
