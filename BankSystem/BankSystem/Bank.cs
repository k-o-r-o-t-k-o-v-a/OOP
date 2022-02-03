using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BankSystem
{
    [Serializable]
    public class Bank
    {
        static public List<Bank> Banks;

        public string Title;

        public double DebitFixPercent;

        public int DepositPercent;

        public double CreditLimit;

        public double CreditCommission;

        public double VerifiedLimit;

        public Dictionary<Client, List<Account>>info;

        public Dictionary<Client, List<IOperations>> history;

        public List<double> DepositPersanteges;

        public List<double> Money;

        public Bank(string title, double debitpercent, double limit, double commission, List<double> money, int perc, double verifiedlim)
        {
            Title = title;
            info = new Dictionary<Client, List<Account>>();
            history = new Dictionary<Client, List<IOperations>>();
            DebitFixPercent = debitpercent;
            CreditLimit = limit;
            CreditCommission = commission;
            VerifiedLimit = verifiedlim;
            Money = money;
            // DepositPersanteges = perc;
            DepositPercent = perc;
            Banks.Add(this);
        }

        static Bank()
        {
            Banks = new List<Bank>();
        }

        public Bank()
        { 
        }

        public Dictionary<Client, List<IOperations>> GetHistory()
        {
            return this.history;
        }

        public double GetDepositPersent(double beginmoney)
        {
            int count = Money.Count;
            var mon = Money.Where(x => x > beginmoney).FirstOrDefault();
            if (mon != default)
                count = Money.Count(x => x == mon);
            return DepositPersanteges[count];
        }

        public void AddClient(Client cl)
        {
            info.Add(cl, new List<Account>());
            history.Add(cl, new List<IOperations>());
        }

        //добавила ретурн
        public Account AddCreditAccount(Client cl, double money)
        {
            ChangeLimit(cl);
            Account account = new CreditAccount(money, CreditLimit, CreditCommission, AccountType.IsNotVerified, VerifiedLimit);
            AddToInfo(cl, account);
            return account;
        }

        public Account AddDebitAccount(Client cl, double money, DateTime start)
        {
            ChangeLimit(cl);
            Account account = new DebitAccount(money, DebitFixPercent, start, AccountType.IsNotVerified, VerifiedLimit);
            AddToInfo(cl, account);
            return account;
        }

        public void DeleteHistory(IOperations operations)
        {
            foreach (var h in history)
            {
                List <IOperations> op = h.Value;
                foreach (IOperations i in op)
                {
                    if (i == operations)
                    {
                        op.Remove(operations);
                        return;
                    }
                }
            }
        }

        public Account AddDepositAccount(Client cl, double money, DateTime start)
        {
            ChangeLimit(cl);
           // double perc = GetDepositPersent(money);
            Account account = new DepositAccount(money, DepositPercent, start, AccountType.IsNotVerified, VerifiedLimit);
            AddToInfo(cl, account);
            return account;
        }

        public void ChangeLimit(Client cl)
        {
            if (cl.clientType == ClientType.IsVerified)
            {
                VerifiedLimit = double.MaxValue;
            }
        }

        public void AddToInfo(Client cl, Account account)
        {
            foreach (var client in info.Keys)
            {
                if (cl == client)
                {
                    info[cl].Add(account);
                }
            }
        }

        public Account GetAccount(Guid id)
        {
            Account account = default;
            foreach (var acc in info.Values)
            {
                account = acc.Where(x => x.Id == id).FirstOrDefault();
                if (account != default)
                {
                    return account;
                }
            }
            throw new Exception("No such account");
        }

        public void Add(Guid id, double money)
        {
            Account acc = GetAccount(id);
            acc.Add(money);

        }

        public void Transfer(Guid id, double money, Guid id2)
        {
            Account acc = GetAccount(id);
            Account acc2 = GetAccount(id2);
            acc.Transfer(money, acc2);
        }

        public void Withdraw(Guid id, double money)
        {
            Account acc = GetAccount(id);
            acc.Withdraw(money);
        }

        public Client GetClient(Account acc)
        {
            foreach (var client in info.Keys)
            {
                if (info[client] == acc)
                {
                    return client;
                }

            }
            throw new Exception("no such  account");
        }

        public List<Account> GetAccounts(Client client)
        {
            List<Account> acc = new List<Account>();

            foreach (var i in info)
                if (i.Key.Login == client.Login)
                    return i.Value;

            return acc;
        }
        
        public void SaveToHistory(Client cl, IOperations operation)
        {      
           
            foreach (var client in info.Keys)
            {
                if (cl.Login == client.Login)
                {
                    this.history[client].Add(operation);
                }
            }
        }

        public void AddMonthlyPercent(Guid accid)
        {
            Account acc = GetAccount(accid);
            acc.AddPercent();
        }

        public IOperations GetOperation(Guid id)
        {
            IOperations operation = default;
            foreach (var op in history.Values)
            {
                operation = op.Where(x => x.OperationId == id).FirstOrDefault();
                if (operation != default)
                {
                    return operation;
                }
            }
            throw new Exception("No such operation");
        }

        public IOperations Cancel(Guid oppId, Account acc)
        {
            IOperations operation = GetOperation(oppId);
            return new CancelOperation(operation, acc);
        }

        public void ChangeType(Client client)
        {
            if (client.clientType == ClientType.IsNotVerified)  
            {
                foreach (var acc in info[client])
                {
                    acc.accountType = AccountType.IsNotVerified;
                }
            }
        }

        public Bank GetBank(string Title)
        {
            foreach (Bank b in Banks)
                if (b.Title == Title)
                    return b;

            return null;
        }

        static public void Save()
        {
            BinaryFormatter serializer = new BinaryFormatter();
            string xml;

            using (FileStream fs = new FileStream("Data/Bank.dat", FileMode.OpenOrCreate))
                serializer.Serialize(fs, Banks);
        }

        static public void Load()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Banks = new List<Bank>();
            using (FileStream fs = new FileStream("Data/Bank.dat", FileMode.OpenOrCreate))
            {
                List<Bank> banks = (List<Bank>)formatter.Deserialize(fs);
                Banks = banks;
            }
        }

       
    }
}
