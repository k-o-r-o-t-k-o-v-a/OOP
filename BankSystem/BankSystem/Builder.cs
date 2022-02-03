using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem
{
    public interface Builder
    {
        void AddName(string name);

        void AddSurname(string surname);

        void AddAddress(string address);

        void AddPassport(Passport passport);
    }
}
