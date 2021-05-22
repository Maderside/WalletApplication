using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletBusiness;

namespace WalletApplication
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Wallet wallet=null;
            bool alive = true;
            

            while (alive)
            {
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.White; // выводим список команд зеленым цветом
                Console.WriteLine("1. Создать кошелек \t 2. Создать счет");
                Console.WriteLine("3. Добавить на счет 4. Взять со счета");
                Console.WriteLine("5. Вывести информацию о ежемесячных изменениях бюджета \t 6.Вывести информацию о изменениях бюджета за все время");
                Console.WriteLine("7. Вывести изменения бюджета совладельцев семейного счета");
                Console.WriteLine("8. Добавить статью расхода \t 9. Добавить статью дохода");
                Console.WriteLine("10. Добавить статью расхода совладельца \t 11. Добавить статью дохода совладельца");
                Console.WriteLine("12. Добавить совладельца");
                Console.WriteLine("13. Вывести общие данные");
                Console.WriteLine("14. Подождать месяц");
                Console.WriteLine("15. Удалить счет");
                Console.WriteLine("16. Заморозить счет (функция администратора)");
                Console.WriteLine("17. Выйти из программы");
                Console.WriteLine("Введите номер пункта:");
                Console.ForegroundColor = color;

                try
                {
                    int command = Convert.ToInt32(Console.ReadLine());

                    switch (command)
                    {
                        case 1:
                            wallet = CreateWallet();
                            break;
                        case 2:
                            CreateAccount(wallet);
                            break;
                        case 3:
                            Put(wallet);
                            break;
                        case 4:
                            Withdraw(wallet);
                            break;
                        case 5:
                            ShowMonthlyChange(wallet);
                            break;
                        case 6:
                            ShowTotalChange(wallet);
                            break;
                        case 7:
                            ShowCoOwnersChange(wallet);
                            break;
                        case 8:
                            AddExpense(wallet);
                            break;
                        case 9:
                            AddIncome(wallet);
                            break;
                        case 10:
                            AddCoOwnerExpense(wallet);
                            break;
                        case 11:
                            AddCoOwnerIncome(wallet);
                            break;
                        case 12:
                            AddCoOwner(wallet);
                            break;
                        case 13:
                            ShowGeneralInfo(wallet);
                            break;
                        case 14:
                            WaitMonth(wallet);
                            break;
                        case 15:
                            DeleteAccount(wallet);
                            break;
                        case 16:
                            FreezeAccount(wallet);
                            break;
                        case 17:
                            alive = false;
                            continue;
                    }
                    
                }
                catch (FormatException ex)
                {
                    //Output the exepcion for invalid menu value
                    color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = color;
                }
                catch (NullReferenceException)
                {
                    //Handling exception connected with null value of wallet or account
                    color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Выбранный кошелек или счет не существует");
                    Console.ForegroundColor = color;
                }

            }
        }
        
        public static Wallet CreateWallet()
        {
            Console.WriteLine("Введите ваше имя:");
            string name = Console.ReadLine();
            while (!name.Contains(" ") || name.Any(char.IsDigit))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Имя пользователя введено неверно, попробуйте еще раз:");
                Console.ForegroundColor = ConsoleColor.Gray;
                name = Console.ReadLine();
            }
            
            Console.WriteLine("Введите ваш пароль:");
            string password = Console.ReadLine();
            return new Wallet(name, password);
        }

        public static void DeleteAccount(Wallet wallet)
        {
            Console.WriteLine("Введите ваше имя:");
            string name = Console.ReadLine();
            Console.WriteLine("Введите ваш пароль:");
            string password = Console.ReadLine();
            AccountType type = ChooseAccount();
            wallet.DeleteAccount(name, password, type);
        }

        public static void FreezeAccount(Wallet wallet)
        {
            Console.WriteLine("Введите специальный код администратора:");
            string code= Console.ReadLine();
            Console.WriteLine("Введите ID счета для удаления:");
            int ID = Convert.ToInt32(Console.ReadLine());
            wallet.FreezeAccount(code, ID);
        }

        public static void CreateAccount(Wallet wallet)
        {
            if (wallet != null)
            {
                AccountType type = ChooseAccount();
                wallet.CreateAccount(type);
            }
            else
            {
                Console.WriteLine("Кошелек еще не создан");
            }
        }
         
        public static void Put(Wallet wallet)
        {
            AccountType type = ChooseAccount();
            Console.WriteLine("Введите сумму");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            wallet.Put(sum, type);
        }
        
        public static void Withdraw(Wallet wallet) 
        {
            
            AccountType type = ChooseAccount();
            Console.WriteLine("Введите сумму");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            wallet.Withdraw(sum, type);
            
        }

        public static AccountType ChooseAccount()
        {
            AccountType type = AccountType.Personal;
            bool alive = true;
            while (alive)
            {
                Console.WriteLine("Выберите аккаунт");
                Console.WriteLine("1. Personal \t 2.Family");

                try
                {
                    int command = Convert.ToInt32(Console.ReadLine());
                    switch (command)
                    {
                        case 1:
                            type = AccountType.Personal;
                            alive = false;
                            break;
                        case 2:
                            type = AccountType.Family;
                            alive = false;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
            return type;
        }
            
        public static void ShowMonthlyChange(Wallet wallet)
        {
            AccountType type = ChooseAccount();
            wallet.ShowAccountMonthlyFlowInfo(type);
        }

        public static void ShowTotalChange(Wallet wallet)
        {
            AccountType type = ChooseAccount();
            wallet.ShowTotalFlowInfo(type);
        }

        public static void ShowCoOwnersChange(Wallet wallet)
        {
            wallet.ShowCoOwnersFlowInfo();
        }

        public static void AddExpense(Wallet wallet)
        {
            AccountType type = ChooseAccount();
            Console.WriteLine("Введите сумму расхода:");
            decimal sum = Convert.ToDecimal(Console.ReadLine());

            Console.WriteLine("Введите название статьи расхода:");
            string name = Console.ReadLine();
            wallet.AddExpense(name, sum, type);
        }

        public static void AddIncome(Wallet wallet)
        {
            AccountType type = ChooseAccount();
            Console.WriteLine("Введите сумму дохода:");
            decimal sum = Convert.ToDecimal(Console.ReadLine());

            Console.WriteLine("Введите название статьи дохода:");
            string name = Console.ReadLine();
            wallet.AddIncome(name, sum, type);
        }

        public static void AddCoOwnerExpense(Wallet wallet)
        {

            try
            {
                Console.WriteLine("Введите сумму расхода:");
                decimal sum = Convert.ToDecimal(Console.ReadLine());

                Console.WriteLine("Введите название статьи расхода:");
                string name = Console.ReadLine();

                Console.WriteLine("Введите имя совладельца:");
                string coOwnerName = Console.ReadLine();

                wallet.AddCoownerExpense(name, sum, coOwnerName);
            }
            catch (NullReferenceException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Владельца с таким именем не найдено:");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public static void AddCoOwnerIncome(Wallet wallet)
        {

            try
            {
                Console.WriteLine("Введите сумму дохода:");
                decimal sum = Convert.ToDecimal(Console.ReadLine());

                Console.WriteLine("Введите название статьи дохода:");
                string name = Console.ReadLine();

                Console.WriteLine("Введите имя совладельца:");
                string coOwnerName = Console.ReadLine();

                wallet.AddCoownerIncome(name, sum, coOwnerName);
            }
            catch (NullReferenceException)
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Владельца с таким именем не найдено:");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public static void AddCoOwner(Wallet wallet)
        { 
            Console.WriteLine("Введите имя совладельца:");
            string coOwnerName = Console.ReadLine();

            wallet.AddCoowner(coOwnerName);
        }

        public static void WaitMonth(Wallet wallet)
        {
            wallet.WaitMonth();
        }

        public static void ShowGeneralInfo(Wallet wallet)
        {
            AccountType type = ChooseAccount();
            int ID = wallet.GetID(type);
            Console.WriteLine($"Имя владельца: {wallet.OwnerName}, ID счета: {ID}");
            Console.WriteLine($"На всех счетах находится средств: {wallet.GetOverallBalance()}");
        }

        
    }
    
}
