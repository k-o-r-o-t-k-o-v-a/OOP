using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem
{
    [Serializable]
    public class CancelOperation : IOperations
    {
        public IOperations Operation;

        public Guid OperationId { get; set; }

        public double Money { get; set; }

        public Account account { get; set; }
        public Account account2 { get; set; }


        public CancelOperation(IOperations Operation, Account acc)
        {
            account = acc;
            Money = Operation.Money;
            Operate();
        }

        public void Operate()
        {
            if (Operation is OperationWithdraw)
            {
                account.Money -= Money;
            }
            if (Operation is OperationTransfer)
            {
                account.Money += Money;
                account2.Money -= Money;
            }
            else
            {
                account.Money -= Money;
            }
            
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
