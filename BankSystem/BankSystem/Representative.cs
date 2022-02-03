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
    class Representative
    {
        public static List<Representative> Representatives;

        public string Login;
        public string Password;
        public Bank Bank;

        static Representative()
        {
            Representatives = new List<Representative>();
        }

        public Representative()
        {
            Representatives.Add(this);
        }

        public Representative(string login, string pass, Bank bank)
        {
            Login = login;
            Password = pass;
            Bank = bank;
            Representatives.Add(this);
        }

        static public void Save()
        {
            BinaryFormatter serializer = new BinaryFormatter();

            using (FileStream fs = new FileStream("Data/Representative.dat", FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs, Representatives);
            }
        }

        static public void Load()
        {
            try
            {

                BinaryFormatter formatter = new BinaryFormatter();
                Representatives = new List<Representative>();
                using (FileStream fs = new FileStream("Data/Representative.dat", FileMode.OpenOrCreate))
                {
                    List<Representative> representative = (List<Representative>)formatter.Deserialize(fs);
                    Representatives = representative;
                }
            }
            catch
            {

            }
        }

        public void AddAdministrator()
        {
            Administrator administrator = new Administrator();
            administrator.Bank = this.Bank;

            Console.WriteLine("Введите фамилию");
            administrator.Surname = Console.ReadLine();

            Console.WriteLine("Введите имя");
            administrator.Name = Console.ReadLine();

            bool isLogin = false;
            while (!isLogin)
            {
                Console.WriteLine("Введите Логин:");

                string login = Console.ReadLine();

                if (administrator.Contain(login) == null)
                {
                    administrator.Login = login;
                    isLogin = true;
                }
                else
                {
                    Console.WriteLine("Такой логин уже зарегестрирован");
                }
            }


            bool isPassword = false;

            while (!isPassword)
            {
                Console.WriteLine("Введите пароль длннной больше 5 символов");
                string pass = Console.ReadLine();

                if (pass != "" && pass.Length < 5)
                {
                    Console.WriteLine("Некорректный пароль");
                }
                else
                {
                    administrator.Password = pass;
                    isPassword = true;
                }

            }

        }

        public void DeleteAdministrator()
        {
            Administrator administrator = new Administrator();
            //List<Administrator> admin = Administrator.GetAdministrators().Where(x => x.Bank.Title == bank.Title).ToList();
            List<Administrator> admin = administrator.GetAdministrator(Bank);
            if (admin.Count() > 0)
            {
                Console.WriteLine("Выберите администратора для удаления:");

                for (int i = 0; i < admin.Count(); i++)
                    Console.WriteLine($"{i + 1}. {admin[i].Surname} {admin[i].Name}");

                int selected = int.Parse(Console.ReadLine());

                administrator.Delete(admin[selected - 1]);
            }

            else
            {
                Console.WriteLine("Админитраторов нет");
            }
        }

        public void PrintInfo()
        {
            Console.WriteLine($"==={Bank.Title}===" + "\n" +
                    $"1. Дебетовый процет: {Bank.DebitFixPercent}" + "\n" +
                    $"2. Депозитный процент: {Bank.DepositPercent}" + "\n" +
                    $"3. Кредитный процент: {Bank.CreditCommission}");
        }

        public void Update()
        {
            Console.WriteLine("Выберите изменяемы параметр:");
            PrintInfo();

            int select = int.Parse(Console.ReadLine());

            switch (select)
            {
                case 1:
                    {
                        Console.WriteLine("Введите новый процент по дебиту:");
                        Bank.DebitFixPercent = double.Parse(Console.ReadLine());
                    }; break;
                case 2:
                    {
                        Console.WriteLine("Введите новый процент по депозиту:");
                        Bank.DepositPercent = int.Parse(Console.ReadLine());
                    }; break;
                case 3:
                    {
                        Console.WriteLine("Введите новый процент по кредиту:");
                        Bank.CreditCommission = double.Parse(Console.ReadLine());
                    }; break;
            }
        }
    }
}
