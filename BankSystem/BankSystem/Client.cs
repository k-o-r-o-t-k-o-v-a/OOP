using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace BankSystem
{
    [Serializable]
    public struct Passport
    {
        public int Number;

        public int Series;
    }

    [Serializable]
    public enum ClientType
    {
        IsVerified,
        IsNotVerified
    }

    [Serializable]
    public class Client
    {
       public  static List<Client> Clients;

        public string Login;

        public string Password;

        public string Name;

        public string Address;

        public Passport passport;

        public ClientType clientType;

        public string Surname;

        static Client()
        {
            Clients = new List<Client>();
        }

        public Client()
        {
            Name = default;

            Address = default;

            passport = default;

            clientType = ClientType.IsNotVerified;
            Clients.Add(this);
        }

        public static void Save()
        {
            BinaryFormatter serializer = new BinaryFormatter();
            string xml;

            using (FileStream fs = new FileStream("Data/Client.dat", FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs, Clients);

            }
        }

        public static void Load()
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Clients = new List<Client>();
                using (FileStream fs = new FileStream("Data/Client.dat", FileMode.OpenOrCreate))
                {
                    List<Client> clients = (List<Client>)formatter.Deserialize(fs);
                    Clients = clients;
                }
            }
            catch
            { 
            
            }

        }

        

        public bool Contain(string login)
        {
            var temp = Clients.Where(x => x.Login == login);
            if (temp != null) return true;
            else return false;
        }

        public Client GetClient(string login, string pass)
        {
           foreach(Client c in Clients)          
                if (c.Login == login && c.Password == pass)
                    return c;
            return null;
        }

        public Client GetClient(string login)
        {
            foreach (Client c in Clients)
                if (c.Login == login)
                    return c;
            return null;
        }

        internal static void Delete(Client client)
        {
            Clients.Remove(client);
        }
    }
    

}
