using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletBusiness
{
    public enum AccountType
    {
        Personal,
        Family,
    }
    public class Wallet
    {
        protected internal event AccountEventsHandler AccountCreated;
        protected internal event AccountEventsHandler AccountCashTransited;
        protected internal event AccountEventsHandler AccountInfoDisplayed;

        protected internal event WalletEventsHandler AccountExists;
        protected internal event WalletEventsHandler AccountDeleted;
        protected internal event WalletEventsHandler AccountFreezed;
        protected internal event WalletEventsHandler MonthWaited;
        protected internal event WalletEventsHandler CoOwnerAdded;


        protected string _ownerName;
        protected string _password;
        protected string _adminAccessCode = "0101";
        protected int _numberOfAccounts;

        private List<Account> accounts;

        

        public Wallet(string name, string password)
        {
            _ownerName = name;
            _password = password;
            _numberOfAccounts = 0;
            accounts = new List<Account>();

            //Handling new Wallet events
            AccountCreated = EventsOutput.ShowAccountMessage;
            AccountCashTransited = EventsOutput.ShowAccountMessage;
            AccountInfoDisplayed = EventsOutput.ShowAccountMessage;

            AccountExists = EventsOutput.ShowWalletMessage;
            AccountDeleted = EventsOutput.ShowWalletMessage;
            AccountFreezed = EventsOutput.ShowWalletMessage;
            MonthWaited = EventsOutput.ShowWalletMessage;
            CoOwnerAdded = EventsOutput.ShowWalletMessage;

        }

        public string OwnerName
        {
            get { return this._ownerName; }
        }

        //Gloabal/Basic interaction
        public void CreateAccount(AccountType accountType)
        {
            Account newAccount;

            //Handling new account events
            AccountCreated=EventsOutput.ShowAccountMessage;
            AccountCashTransited = EventsOutput.ShowAccountMessage;
            AccountInfoDisplayed = EventsOutput.ShowAccountMessage;


            //Creating new account and adding it to the list of accounts
            if (_numberOfAccounts < 2 && accounts.Exists(x => x._accountType == accountType)==false) 
            {
                
                switch (accountType)
                {
                    case AccountType.Personal:
                        
                        newAccount = new PersonalAccount(0);
                        accounts.Add(newAccount);
                        AccountCreated(newAccount, new AccountEventArgs("Создан персональный счет, текущий баланс: " + newAccount.CurrentBalance, newAccount.CurrentBalance));
                        break;
                        
                    case AccountType.Family:
                        newAccount = new FamilyAccount(0);
                        accounts.Add(newAccount);
                        AccountCreated(newAccount, new AccountEventArgs("Создан семейный счет, текущий баланс: " + newAccount.CurrentBalance, newAccount.CurrentBalance));
                        break;
                }
                _numberOfAccounts++;
                
            }
            else
            {
                AccountExists(this, new WalletEventArgs("У вас уже присутсвуют счета таких типов"));
            }
        }

        public void DeleteAccount(string name, string password, AccountType accountType)
        {
            if(_ownerName==name && _password == password)
            {
                Account accountFirst = accounts.Find(x => x._accountType == accountType);
                Account accountSecond = accounts.Find(x => x._accountType != accountType);

                if (accountFirst != null && accountSecond!=null && accountFirst.CurrentBalance >= 0)
                {
                    CashTransit(accountFirst, accountSecond);
                    accounts.Remove(accountFirst);
                    AccountDeleted(this, new WalletEventArgs("Выбранный счет удален."));
                }
                else if(accountFirst != null && accountFirst.CurrentBalance < 0)
                {
                    AccountCashTransited(accountFirst, new AccountEventArgs("Невозможен перевод средств на другой счет, процедура удаления счета отменена. Необходимо погасить задолженность: " + accountFirst.CurrentBalance, accountFirst.CurrentBalance));
                }
                else if(accountFirst == null)
                {
                    AccountDeleted(this, new WalletEventArgs("Счет для удаления не найден."));
                }
                else if(accountFirst != null && accountSecond == null)
                {
                    accounts.Remove(accountFirst);
                    AccountDeleted(this, new WalletEventArgs("Выбранный счет удален, без перевода средств."));
                }
                _numberOfAccounts--;
            }
            else
            {
                AccountDeleted(this, new WalletEventArgs("Неверный пароль или имя пользователя."));
            }
        }


        private void CashTransit(Account accountFirst, Account accountSecond)
        {   

            if (accountFirst != null && accountSecond != null && accountFirst.CurrentBalance > 0)
            {
                decimal transitCash = accountFirst.Withdraw(accountFirst.CurrentBalance);
                accountSecond.Put(transitCash);
                AccountCashTransited(this, new AccountEventArgs("Средства переведены на другой счет: ", transitCash));
            }
            else if (accountFirst != null && accountFirst.CurrentBalance <= 0)
            {
                AccountCashTransited(accountFirst, new AccountEventArgs("Невозможен перевод средств на другой счет. Текущий баланс: " + accountFirst.CurrentBalance, accountFirst.CurrentBalance));
            }
            else if (accountFirst == null)
            {
                AccountCashTransited(accountFirst, new AccountEventArgs("Счет для перевода не найден.", accountFirst.CurrentBalance));
            }
            else if (accountFirst != null && accountSecond == null)
            {
                AccountCashTransited(accountFirst, new AccountEventArgs("Второй счет для перевода.", accountFirst.CurrentBalance));
            }
        }


        //Admin interaction
        public void FreezeAccount(string enteredCode, int enteredID)
        {
            if (_adminAccessCode == enteredCode)
            {
                Account account = accounts.Find(x => x.Id == enteredID);
                account.IsActive = false;
                AccountFreezed(this, new WalletEventArgs("Выбранный счет заморожен"));
            }
            else
            {
                AccountFreezed(this, new WalletEventArgs("Неверный код администратора или ID"));
            }
        }


        //User interaction
        public void Put(decimal sum, AccountType accountType)
        {
            Account account = accounts.Find(x => x._accountType == accountType);
            account.Put(sum);
        }

        public void Withdraw(decimal sum, AccountType accountType)
        {
            Account account = accounts.Find(x => x._accountType == accountType);
            account.Withdraw(sum);
        }

        public void WaitMonth()
        {
            if (accounts.Count == 2)
            {
                accounts[0].IncrementMonths();
                accounts[0].ApplyCashFlow();

                accounts[1].IncrementMonths();
                accounts[1].ApplyCashFlow();
            }
            else if(accounts.Count == 1)
            {
                accounts[0].IncrementMonths();
                accounts[0].ApplyCashFlow();
            }
            else
            {
                MonthWaited(this, new WalletEventArgs("Счет еще не создан"));
            }
            

        }

        public void AddExpense(string expenseName, decimal value,  AccountType accountType)
        {
            Account account = accounts.Find(x => x._accountType == accountType);
            account.AddExpense(expenseName, value);
        }

        public void AddIncome(string incomeName, decimal value, AccountType accountType)
        {
            Account account = accounts.Find(x => x._accountType == accountType);
            account.AddIncome(incomeName, value);
        }

        public void AddCoownerExpense(string expenseName, decimal value, string coOwnerName)
        {
            FamilyAccount account = accounts.Find(x => x._accountType == AccountType.Family) as FamilyAccount;
            account.AddExpense(expenseName, value, coOwnerName);
        }

        public void AddCoownerIncome(string incomeName, decimal value, string coOwnerName)
        {
            FamilyAccount account = accounts.Find(x => x._accountType == AccountType.Family) as FamilyAccount;
            account.AddIncome(incomeName, value, coOwnerName);
        }

        public void AddCoowner(string coOwnerName)
        {
            FamilyAccount account = accounts.Find(x => x._accountType == AccountType.Family) as FamilyAccount;
            if (account != null)
            {
                account.AddCoOwner(coOwnerName);
            }
            else
            {
                CoOwnerAdded(this, new WalletEventArgs("Не найдено семейного аккаунта."));
            }
        }
        public void ShowAccountMonthlyFlowInfo(AccountType accountType)
        {
            Account account = accounts.Find(x => x._accountType == accountType);
            AccountInfoDisplayed(account, new AccountEventArgs("Текущий баланс: "+ account.CurrentBalance, account.CurrentBalance));

            //Handling displaying events for expenses
            AccountInfoDisplayed(account, new AccountEventArgs("Расходы для текущего счета владельца "+this._ownerName+": ", account.CurrentBalance));
            foreach (CashFlow expense in account._cashFlowList)
            {
                if(expense is Expense)
                {
                    AccountInfoDisplayed(expense, new AccountEventArgs("Расходы по статье " + expense.ItemName + " составляют: " + expense.BudgetChangeValue, expense.BudgetChangeValue));
                }
                
            }
            AccountInfoDisplayed(account, new AccountEventArgs("Суммарные месячные расходы: " + account.GetMonthlyExpense(), account.GetMonthlyExpense()));


            //Handling displaying events for income
            AccountInfoDisplayed(account, new AccountEventArgs("Доходы для текущего счета владельца " + this._ownerName + ": ", account.CurrentBalance));
            foreach (CashFlow income in account._cashFlowList)
            {
                if (income is Income)
                {
                    AccountInfoDisplayed(income, new AccountEventArgs("Доходы по статье " + income.ItemName + " составляют: " + income.BudgetChangeValue, income.BudgetChangeValue));
                }
                    
            }
            AccountInfoDisplayed(account, new AccountEventArgs("Суммарные месячные доходы: " + account.GetMonthlyIncome(), account.GetMonthlyIncome()));


            //Handling displaying monthly cash flow
            AccountInfoDisplayed(account, new AccountEventArgs("Месячное изменение баланса: " + account.GetMonthlyBalanceChange(), account.GetMonthlyBalanceChange()));
        }

        public void ShowTotalFlowInfo(AccountType accountType)
        {
            Account account = accounts.Find(x => x._accountType == accountType);
            AccountInfoDisplayed(account, new AccountEventArgs("Суммарные расходы с момента создания счета: "+account.GetTotalExpense(), account.GetTotalExpense()));
            AccountInfoDisplayed(account, new AccountEventArgs("Суммарные доходы с момента создания счета: " + account.GetTotalIncome(), account.GetTotalIncome()));
        }

        public void ShowCoOwnersFlowInfo()
        {
            try
            {
                FamilyAccount account = accounts.Find(x => x._accountType == AccountType.Family) as FamilyAccount;
                foreach (CoOwner owner in account._coOwners)
                {
                    //Handling displaying events for expenses
                    AccountInfoDisplayed(owner, new AccountEventArgs("Расходы для текущего счета совладельца " + owner.Name + ": ", account.CurrentBalance));
                    foreach (CashFlow expense in owner._pesonCashFlowList)
                    {
                        if (expense is Expense)
                        {
                            AccountInfoDisplayed(expense, new AccountEventArgs("Расходы по статье " + expense.ItemName + " составляют: " + expense.BudgetChangeValue, expense.BudgetChangeValue));
                        }
                    }
                    AccountInfoDisplayed(owner, new AccountEventArgs("Суммарные месячные расходы: " + owner.GetMonthlyExpense(), owner.GetMonthlyExpense()));


                    //Handling displaying events for income
                    AccountInfoDisplayed(account, new AccountEventArgs("Доходы для текущего счета совладельца " + owner.Name + ": ", account.CurrentBalance));
                    foreach (CashFlow income in owner._pesonCashFlowList)
                    {
                        if (income is Income)
                        {
                            AccountInfoDisplayed(income, new AccountEventArgs("Доходы по статье " + income.ItemName + " составляют: " + income.BudgetChangeValue, income.BudgetChangeValue));
                        }
                    }
                    AccountInfoDisplayed(owner, new AccountEventArgs("Суммарные месячные доходы: " + owner.GetMonthlyIncome(), owner.GetMonthlyIncome()));
                }
            }
            catch (Exception)
            {
                AccountInfoDisplayed(this, new AccountEventArgs("В аккаунт не было добавлено совладельцев: "));
            }
            
        }

        public decimal GetOverallBalance()
        {
            try
            {
                Account accountF = accounts.Find(x => x._accountType == AccountType.Family);
                Account accountP = accounts.Find(x => x._accountType == AccountType.Personal);
                if (accountF == null)
                {
                    return accountP.CurrentBalance;
                }
                else if(accountP==null)
                {
                    return accountF.CurrentBalance;
                }
                else
                {
                    return accountF.CurrentBalance + accountP.CurrentBalance;
                }
                
            }
            catch (NullReferenceException)
            {
                AccountInfoDisplayed(this, new AccountEventArgs("Не создано ни одного счета"));
                return 0;
            }
        }

        public int GetID(AccountType type)
        {
            Account account = accounts.Find(x => x._accountType == type);
            return account.Id;
        }
    }
}


