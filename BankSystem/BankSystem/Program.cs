using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class Program
    {
        static Client client;
        static Bank bank;
        static Administrator administrator;
        static Representative representative;
        static void Main(string[] args)
        {
            Representative.Load();
            Client.Load();
            Bank.Load();
            Administrator.Load();
            Menu();


            Console.Read();
        }



        static void Menu()
        {
            Console.WriteLine("Выберите действие: ");
            Console.WriteLine("1. Вход;");
            Console.WriteLine("2. Регистрация;");
            Console.WriteLine("3. Админ. доступ");
            Console.WriteLine("4. Представитель");

            int select = int.Parse(Console.ReadLine());

            switch (select)
            {
                case 1: Entrance(); break;
                case 2:
                    {
                        CreateClient();

                        Client.Save();
                        Bank.Save();
                        Menu();
                    }
                    break;
                case 3: LoginAdmin(); break;
                case 4: Login(); break;
            }
        }

        #region Регистрация
        static public void CreateClient()
        {
            client = new Client();
            Console.WriteLine("===РЕГИСТРАЦИЯ НОВОГО ПОЛЬЗОВАТЕЛЯ===");
            Console.Write("Введите фамилию: ");
            client.Surname = Console.ReadLine();

            Console.Write("Введите имя: ");
            client.Name = Console.ReadLine();

            Console.Write("Введите адресс: ");
            client.Address = Console.ReadLine();

            Console.Write("Введите серию паспота: ");

            string series = Console.ReadLine();
            if (series == "" || series.Length!=4)
            {
                Console.WriteLine("Некорректная серия паспорта. Повторите попытку регистрации");
                Client.Delete(client);
                return;
            }
            else client.passport.Series = int.Parse(series);

            Console.Write("Введите номер паспорта: ");
            string number = Console.ReadLine();
            if (number == "" || number.Length != 6)
            {
                Console.WriteLine("Некорректный номер паспорта. Повторите попытку регистрации");
                Client.Delete(client);
                return;
            }
            else client.passport.Number = int.Parse(number);

            CreateLoginPass(client);

            client.clientType = ClientType.IsNotVerified;

            SelectTypeAccaunt(client);

            Console.WriteLine("Клиент добавлен");
        }

        static public int SelectBank()
        {
            Console.WriteLine("Выберите банк: ");

            for (int i = 0; i < Bank.Banks.Count(); i++)
                Console.WriteLine("");

            return 0;
        }

        static public void SelectTypeAccaunt(Client c)
        {
            Console.WriteLine("Выберите тип счета:");
            Console.WriteLine("1. Дебетовый счет;");
            Console.WriteLine("2. Депозитный счет;");
            Console.WriteLine("3. Кредитный счет;");

            int select = int.Parse(Console.ReadLine());

            Console.WriteLine("Выберите оптимальные для Вас условия:");
            switch (select)
            {
                case 1:
                    {
                        for (int i = 0; i < Bank.Banks.Count(); i++)
                            Console.WriteLine($"{i + 1}.Банк: {Bank.Banks[i].Title} Условия: {Bank.Banks[i].DebitFixPercent}% годовых");
                        AddScore("debit", c);
                    }; break;
                case 2:
                    {
                        for (int i = 0; i < Bank.Banks.Count(); i++)
                            Console.WriteLine($"Банк: {Bank.Banks[i].Title} Условия: {Bank.Banks[i].DepositPercent}% годовых");
                        AddScore("deposit", c);
                    }; break;
                case 3:
                    {
                        for (int i = 0; i < Bank.Banks.Count(); i++)
                            Console.WriteLine($"Банк: {Bank.Banks[i].Title} Лимит: {Bank.Banks[i].CreditLimit} Условия: {Bank.Banks[i].CreditCommission}% годовых");
                        AddScore("credit", c);
                    }; break;
            }
        }

        static void AddScore(string type, Client c)
        {
            int select = int.Parse(Console.ReadLine());
            switch (type)
            {
                case "debit":
                    {
                        Bank.Banks[select - 1].AddClient(c);
                        Console.WriteLine("Введите внесенную сумму: ");
                        double count = double.Parse(Console.ReadLine());
                        Bank.Banks[select - 1].AddDebitAccount(c, count, DateTime.Now);
                    }; break;
                case "deposit":
                    {
                        Bank.Banks[select - 1].AddClient(c);
                        Console.WriteLine("Введите внесенную сумму: ");
                        double count = double.Parse(Console.ReadLine());
                        Bank.Banks[select - 1].AddDepositAccount(c, count, DateTime.Now);
                    }; break;
                case "credit":
                    {
                        Bank.Banks[select - 1].AddClient(c);
                        Bank.Banks[select - 1].AddCreditAccount(c, 100000);
                    }; break;
            }

        }

        static void CreateLoginPass(Client client)
        {
            bool isLogin = false;
            while (!isLogin)
            {
                Console.WriteLine("Введите Логин:");

                string login = Console.ReadLine();

                if (client.Contain(login))
                {
                    client.Login = login;
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
                    client.Password = pass;
                    isPassword = true;
                }

            }
        }
        #endregion

        #region Вход
        static public void Entrance()
        {
            bool isEntrance = false;
            while (!isEntrance)
            {
                client = new Client();
                Console.WriteLine("Введите ЛОГИН");
                string login = Console.ReadLine();
                Console.WriteLine("Введите ПАРОЛЬ");
                string pass = Console.ReadLine();

                client = client.GetClient(login, pass);

                if (client != null)
                {
                    isEntrance = true;
                    PrintClient(client);
                    MenuUser();
                }
                else
                    Console.WriteLine("Некорректный ЛОГИН и/или ПАРОЛЬ");

            }
        }


        static public void PrintClient(Client client)
        {
            Console.WriteLine($"Добро пожаловаь: {client.Surname} {client.Name}");
        }
        #endregion

        #region Работа со счетом
        static public void MenuUser()
        {
            Console.WriteLine("Выберите действие: ");
            Console.WriteLine("1. Снять;");
            Console.WriteLine("2. Перевести;");
            Console.WriteLine("3. Пополнлить");
            Console.WriteLine("4. Информация о счетах");
            Console.WriteLine("5. Открыть новый счет");
            Console.WriteLine("6. Выход");

            int select = int.Parse(Console.ReadLine());

            switch (select)
            {
                case 1:
                    {
                        TakeOff();
                    }; break;
                case 2:
                    {
                        Transfer();
                    }; break;
                case 3:
                    {
                        AddMoney();
                    }; break;
                case 4:
                    {
                        PrintScore(client);
                        MenuUser();
                    }; break;
                case 5:
                    {
                        SelectTypeAccaunt(client);
                        MenuUser();
                        Bank.Save();
                        Client.Save();
                    }; break;
                case 6:
                    {
                        client = null;
                        Menu();
                    }; break;
            }
        }

        static public void TakeOff()
        {
            Account ac = default;
            Bank b = default;

            Dictionary<Bank, List<Account>> temp = PrintScore(client);

            Console.WriteLine("Выберите счет: ");
            int select = int.Parse(Console.ReadLine());

            foreach (var t in temp)
                if (select > t.Value.Count)
                    select -= t.Value.Count;
                else
                {
                    ac = t.Value[select - 1];
                    b = t.Key;
                }

            Console.WriteLine($"Выбран аккаунт: {ac.Id} {ac.Money}");

            Console.WriteLine("Введите сумму вывода: ");
            double count = double.Parse(Console.ReadLine());

            OperationWithdraw withdraw = new OperationWithdraw(ac, count);
            b.SaveToHistory(client, withdraw);
            Client.Save();
            Bank.Save();
            MenuUser();
        }

        static public void AddMoney()
        {
            Account ac = default;
            Bank b = default;

            Dictionary<Bank, List<Account>> temp = PrintScore(client);

            Console.WriteLine("Выберите счет: ");
            int select = int.Parse(Console.ReadLine());

            foreach (var t in temp)
                if (select > t.Value.Count)
                    select -= t.Value.Count;
                else
                {
                    ac = t.Value[select - 1];
                    b = t.Key;
                }

            Console.WriteLine($"Выбран аккаунт: {ac.Id} {ac.Money}");

            Console.WriteLine("Введите сумму ввода: ");
            double count = double.Parse(Console.ReadLine());

            OperationAdd add = new OperationAdd(ac, count);
            b.SaveToHistory(client, add);
            Client.Save();
            Bank.Save();
            MenuUser();
        }

        static public void Transfer()
        {
            Account ac = default;
            Account ac2 = default;
            Bank b = default;
            Bank b2 = default;

            Dictionary<Bank, List<Account>> temp = PrintScore(client);

            Console.WriteLine("Выберите счет: ");
            int select = int.Parse(Console.ReadLine());

            foreach (var t in temp)
                if (select > t.Value.Count)
                    select -= t.Value.Count;
                else
                {
                    ac = t.Value[select - 1];
                    b = t.Key;
                }

            Console.WriteLine($"Выбран аккаунт: {ac.Id} {ac.Money}");

            bool isChech = false;

            Client client2 = default;

            while (!isChech)
            {
                Console.WriteLine("Введите логин кому переводим средства:");
                string c = Console.ReadLine();
                client2 = client.GetClient(c);

                if (client2 != null)
                    isChech = true;
                else Console.WriteLine("Некорректный логин");
            }


            temp = PrintScore(client2);

            Console.WriteLine("Выберите счет: ");
            select = int.Parse(Console.ReadLine());

            foreach (var t in temp)
                if (select > t.Value.Count)
                    select -= t.Value.Count;
                else
                {
                    ac2 = t.Value[select - 1];
                    b2 = t.Key;
                }

            Console.WriteLine($"Выбран аккаунт: {ac2.Id} {ac2.Money}");

            Console.WriteLine("Введите сумму ввода: ");
            double count = double.Parse(Console.ReadLine());

            OperationTransfer transfer = new OperationTransfer(ac, ac2, count);
            b.SaveToHistory(client, transfer);
            b2.SaveToHistory(client2, transfer);

            Client.Save();
            Bank.Save();
            MenuUser();
        }

        static public Dictionary<Bank, List<Account>> PrintScore(Client client)
        {
            Dictionary<Bank, List<Account>> temp = new Dictionary<Bank, List<Account>>();

            for (int i = 0; i < Bank.Banks.Count; i++)
            {
                List<Account> acc = new List<Account>();
                acc = Bank.Banks[i].GetAccounts(client);
                temp.Add(Bank.Banks[i], acc);
            }

            int countScore = 0;
            foreach (var t in temp)
            {
                List<Account> acc = t.Value;
                Console.BackgroundColor = ConsoleColor.Green;
                for (int j = 0; j < acc.Count; j++)
                {
                    Console.WriteLine($"{countScore + 1}. Банк: {t.Key.Title} Сумма: {acc[j].Money}");
                    countScore++;
                }
                Console.BackgroundColor = ConsoleColor.Black;
            }

            return temp;
        }
        #endregion

        #region Администратор

        static public void LoginAdmin()
        {
            bool isEntrance = false;
            while (!isEntrance)
            {
                administrator = new Administrator();
                Console.WriteLine("Введите ЛОГИН");
                string login = Console.ReadLine();
                Console.WriteLine("Введите ПАРОЛЬ");
                string pass = Console.ReadLine();

                administrator = administrator.GetAdministrator(login, pass);

                if (administrator != null)
                {
                    isEntrance = true;
                    Console.WriteLine($"Добро пожаловать {administrator.Surname} {administrator.Name} администратор банка {administrator.Bank.Title}");
                    MenuAdmin();
                }
                else
                    Console.WriteLine("Некорректный ЛОГИН и/или ПАРОЛЬ");
            }
        }

        static public void MenuAdmin()
        {
            Console.WriteLine("Выберите действие");
            Console.WriteLine("1. Верефикация пользователей");
            Console.WriteLine("2. Отмена транзакций");
            Console.WriteLine("3. Выход");

            int select = int.Parse(Console.ReadLine());

            switch (select)
            {
                case 1:
                    {
                        Verification();
                        Bank.Save();
                    };break;
                case 2:
                    {
                        CancelingTransaction();
                        Bank.Save();
                        MenuAdmin();
                    };break;
                case 3:
                    {
                        administrator = null;
                        Menu();
                    };break;
            }

        }

        static public void Verification()
        {
            Account ac = default;
            Bank bank = new Bank();

            bank = bank.GetBank(administrator.Bank.Title);
            Dictionary<Client, List<Account>> temp;

            temp = bank.info;
            int countScore = 0;
            foreach (var t in temp)
            {
                List<Account> acc = t.Value;
                for (int j = 0; j < acc.Count; j++)
                {
                    Console.WriteLine($"{countScore + 1}. {t.Key.Surname} {t.Key.Name} Верификация: {acc[j].accountType.ToString()}");
                    countScore++;
                }
            }

            Console.WriteLine("Выберите счет для верефикации: ");
            int select = int.Parse(Console.ReadLine());

            foreach (var t in temp)
                if (select > t.Value.Count)
                    select -= t.Value.Count;
                else
                {
                    ac = t.Value[select - 1];
                }

            ac.accountType = AccountType.IsVerified;
            Console.WriteLine($"Счет верефицирован");

            MenuAdmin();
        }

        static public void CancelingTransaction()
        {
            //Account ac = default;

            Bank bank = new Bank();

            bank = bank.GetBank(administrator.Bank.Title);
            Dictionary<Client, List<IOperations>> temp;

            //temp = administrator.Bank.history;
            temp = bank.history;

            int countScore = 0;
            foreach (var t in temp)
            {
                Console.WriteLine(t.Key.Surname);
                List<IOperations> operation = t.Value;
                for (int j = 0; j < operation.Count; j++)
                {
                    Console.WriteLine($"{countScore + 1}. {t.Key.Surname} {t.Key.Name} Операция ID: {operation[j].OperationId} Сумма: {operation[j].Money}");
                    countScore++;
                }
            }

            IOperations operations = default;


            if (temp.Count() == 0)
                Console.WriteLine("Транзакции отсутствуют");
            else
            {
                Console.WriteLine("Выберите транзакцию для отмены: ");
                int select = int.Parse(Console.ReadLine());

                foreach (var t in temp)
                    if (select > t.Value.Count)
                        select -= t.Value.Count;
                    else
                    {
                        operations = t.Value[select - 1];
                    }

                operations.Close();

                bank.DeleteHistory(operations);
                Console.WriteLine("Транзакция отменена");
            }


        }

        #endregion

        #region Представитель
        static public void Login()
        {
            bool isEntrance = false;
            while (!isEntrance)
            {
                bank = new Bank();
                Console.WriteLine("Введите ЛОГИН");
                string login = Console.ReadLine();
                Console.WriteLine("Введите ПАРОЛЬ");
                string pass = Console.ReadLine();

                for (int i = 0; i < Bank.Banks.Count; i++)
                {
                    if (Representative.Representatives[i].Login == login && Representative.Representatives[i].Password == pass)
                    {
                        bank = bank.GetBank(Representative.Representatives[i].Bank.Title);
                        representative = Representative.Representatives[i];
                    }
                }

                if (bank.Title != null)
                {
                    isEntrance = true;
                    Console.WriteLine($"Добро пожаловать представитель банка {bank.Title}");
                    MenuRepresentative();
                }
                else
                    Console.WriteLine("Некорректный ЛОГИН и/или ПАРОЛЬ");

            }
        }

        static public void AddAdministrator()
        {
            representative.AddAdministrator();
            Administrator.Save();
        }

        static public void MenuRepresentative()
        {
            Console.WriteLine("Выберите действие: ");
            Console.WriteLine("1. Просмотреть параметры;");
            Console.WriteLine("2. Изменить параметры");
            Console.WriteLine("3. Добавить администратора");
            Console.WriteLine("4. Удалить администратора");
            Console.WriteLine("5. Выход");

            int select = int.Parse(Console.ReadLine());

            switch (select)
            {
                case 1:
                    {
                        PrintInfoBank();
                        MenuRepresentative();
                    }; break;
                case 2:
                    {
                        Update();
                        Bank.Save();
                        MenuRepresentative();
                    };break;
                case 3:
                    {
                        AddAdministrator();
                        Administrator.Save();
                        MenuRepresentative();
                    }; break;
                case 4:
                    {
                        DeleteAdministrator();
                        Administrator.Save();
                        MenuRepresentative();
                    }; break;
                case 5:
                    {
                        bank = null;
                        Menu();
                    };break;
            }
        }

        static public void DeleteAdministrator()
        {
            representative.DeleteAdministrator();
        }

        static public void PrintInfoBank()
        {
            representative.PrintInfo();
        }

        static public void Update()
        {
            representative.Update();
        }
        #endregion
    }
}
