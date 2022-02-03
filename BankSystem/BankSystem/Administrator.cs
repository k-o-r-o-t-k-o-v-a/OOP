using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    [Serializable]
    class Administrator
    {
        public static List<Administrator> Administrators;

        public Bank Bank;

        public string Surname;
        public string Name;
        public string Login;
        public string Password;

        static Administrator()
        {
            Administrators = new List<Administrator>();
        }

        public Administrator(Bank bank, string surname, string name, string login, string pass)
        {
            Bank = bank;
            Surname = surname;
            Name = name;
            Login = login;
            Password = pass;
            Administrators.Add(this);
        }

        public Administrator()
        {
            Administrators.Add(this);
        }

        public Administrator Contain(string login)
        {

            foreach (Administrator adm in Administrators)
                if (adm.Login == login)
                    return adm;

            return null;
        }

        static public void Save()
        {
            BinaryFormatter serializer = new BinaryFormatter();

            using (FileStream fs = new FileStream("Data/Administrator.dat", FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs, Administrators);
            }
        }

        static public void Load()
        {
            try
            {

                BinaryFormatter formatter = new BinaryFormatter();
                Administrators = new List<Administrator>();
                using (FileStream fs = new FileStream("Data/Administrator.dat", FileMode.OpenOrCreate))
                {
                    List<Administrator> administrator = (List<Administrator>)formatter.Deserialize(fs);
                    Administrators = administrator;
                }
            }
            catch
            { 
            
            }
        }

        static public List<Administrator> GetAdministrators()
        {
            return Administrators;
        }

        public List<Administrator> GetAdministrator(Bank b)
        {

            return Administrators.Where(x => x.Bank != null).Where(x => x.Bank.Title == b.Title).ToList();
        }

        internal Administrator GetAdministrator(string login, string pass)
        {
            foreach (Administrator adm in Administrators)
                if (adm.Login == login && adm.Password == pass)
                    return adm;

            return null;
        }

        internal void Delete(Administrator administrator)
        {
            Administrators.Remove(administrator);
        }
    }
}
