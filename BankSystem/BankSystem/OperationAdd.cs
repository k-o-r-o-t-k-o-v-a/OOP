using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem
{
    [Serializable]
    public class OperationAdd : IOperations
    {
        public Guid OperationId { get; set; }

        public double Money { get; set; }

        public Account account { get; set; }
        public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public OperationAdd(Account acc, double money)
        {
            account = acc;
            Money = money;
            OperationId = Guid.NewGuid();
            Operate();

        }

        public void Operate()
        {
            account.Money += Money;            
        }

        public void Close()
        {
            account.Money -= Money;
        }
    }
}
