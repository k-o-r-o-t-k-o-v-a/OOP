using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem
{
    [Serializable]
    public class OperationWithdraw : IOperations
    {
        public Guid OperationId { get; set; }

        public double Money { get; set; }

        public Account account { get; set; }

        public OperationWithdraw(Account acc, double money)
        {
            account = acc;
            Money = money;
            OperationId = Guid.NewGuid();
            Operate();

        }

        public void Operate()
        {
            account.Money -= Money;
        }

        public void Close()
        {
            account.Money += Money;
        }
    }
}
