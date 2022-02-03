using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem
{
    [Serializable]
    public class BuilderClient : Builder

    {

        Client client;

        public BuilderClient()
        {
            client = new Client();
        }

        public void AddName(string name)
        {
            client.Name = name;
        }
        public void AddSurname(string surname)
        {
            client.Surname = surname;
        }

        public void AddAddress(string address)
        {
            client.Address = address;
        }

        public void AddPassport(Passport passport)
        {
            client.passport = passport;
        }

        public bool IsVerified()
        {
            if ((client.Name == default) || (client.Address == default) || (client.passport.Series == default) || (client.passport.Number == default))
            {
                client.clientType = ClientType.IsNotVerified;
                return false;
            }
            else
            {
                client.clientType = ClientType.IsVerified;
                return true;
            }
        }

        public Client GetClient()
        {
            IsVerified();
            return client;
        }
    }
}
