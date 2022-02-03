using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem
{
    public interface IOperations
    {
        Guid OperationId { get; set; }

        double Money { get; set; }

        void Operate();

        void Close();
    }
}
