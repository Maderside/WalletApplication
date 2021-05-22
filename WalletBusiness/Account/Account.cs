using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletBusiness
{
    internal abstract class Account : IAccount
    {

        protected internal virtual event AccountEventsHandler Withdrawed;
        protected internal virtual event AccountEventsHandler Putted;
        protected internal virtual event AccountEventsHandler AddedIncome;
        protected internal virtual event AccountEventsHandler MonthPassed;
        

        protected int _id;
        protected decimal _balance;
        protected int _months;
        protected int _monthsAfterApply;
        protected int _numberOfCoOwners;
        protected bool _isActive;
        public AccountType _accountType;
        public List<CashFlow> _cashFlowList = new List<CashFlow>();

        public Account(decimal sum)
        {
            _balance = sum;
            _id = GenerateID();
            _months = 0;
            Income put = new Income("Putted", 0);
            _cashFlowList.Add(put);
            Expense withdraw = new Expense("Withdrawed", 0);
            _cashFlowList.Add(withdraw);
            _isActive = true;
            _monthsAfterApply = 0;

            Withdrawed=EventsOutput.ShowAccountMessage;
            AddedIncome = EventsOutput.ShowAccountMessage;
            MonthPassed = EventsOutput.ShowAccountMessage;
            Putted = EventsOutput.ShowAccountMessage;
        }


        //Non-regular balance change
        public void Put(decimal sum)
        {
            if (IsActive == true)
            {
                _balance += sum;
                _cashFlowList[0].BudgetChangeValue += sum;
                Putted(this, new AccountEventArgs("На счет было отправлено: " + sum, sum));
            }
            else
            {
                Putted(this, new AccountEventArgs("Невозможно положить средства, счет заморожен. Введенная сумма: " + sum, sum));
            }
            
        }
        public virtual decimal Withdraw(decimal sum)
        {
            decimal result = 0;
            if (IsActive)
            {
                
                if (sum <= _balance)
                {

                    _balance -= sum;
                    result = sum;
                    _cashFlowList[1].BudgetChangeValue += sum;
                    Withdrawed(this, new AccountEventArgs("Со счета было снято: " + sum, sum));
                    return result;
                }
                else
                {
                    Withdrawed(this, new AccountEventArgs("На счете не хватает средств: " + sum, sum));
                }
                 
            }
            else
            {
                Withdrawed(this, new AccountEventArgs("Невозможно снять средства, счет заморожен. Введенная сумма: " + sum, sum));
            }
            return result;
        }


        //Access to account properties
        public decimal CurrentBalance
        {
            get { return _balance; }
        }

        public int Id
        {
            get { return _id; }
        }

        public int Months
        {
            get { return _months; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

       

        public void IncrementMonths()
        {
            if (IsActive)
            {
                if (_balance >= 0)
                {
                    _months++;
                    _monthsAfterApply++;
                    _cashFlowList[0].BudgetChangeValue = 0;
                    _cashFlowList[1].BudgetChangeValue = 0;
                }
                else
                {
                    MonthPassed(this, new AccountEventArgs("На счете не хватает средств для регулярных расходов, текущий баланс: " + this._balance, this._balance));
                } 
            }

        }

        private int GenerateID()
        {
            Random id = new Random();
            return id.Next(1000, 9999);
        }

        //Add flows
        public virtual void AddIncome(string incomeName, decimal incomeValue)
        {
            if (IsActive)
            {
                if (incomeValue > 0)
                {
                    if (!_cashFlowList.Exists(x => x.ItemName == incomeName))
                    {
                        Income income = new Income(incomeName, incomeValue);
                        _cashFlowList.Add(income);
                    }
                    else
                    {
                        _cashFlowList.Find(x => x.ItemName == incomeName).BudgetChangeValue = incomeValue;
                    }
                }
                else
                {
                    AddedIncome(this, new AccountEventArgs("Ввведенное значение поступления некорректно: " + incomeValue, incomeValue));
                } 
            }
            
        }
        public virtual void AddExpense(string expenseName, decimal expenseValue)
        {
            if (IsActive)
            {
                if (!_cashFlowList.Exists(x => x.ItemName == expenseName))
                {
                    Expense expense = new Expense(expenseName, expenseValue);
                    _cashFlowList.Add(expense);
                }
                else
                {
                    _cashFlowList.Find(x => x.ItemName == expenseName).BudgetChangeValue = expenseValue;
                } 
            }
        }

        //Apply cash
        public virtual void ApplyCashFlow()
        {
            if (IsActive)
            {
                foreach (CashFlow flow in _cashFlowList)
                {
                    _balance += flow.BudgetChangeValue;
                    _monthsAfterApply = 0;
                } 
            }
            
        }

        //getting flows
        public virtual decimal GetMonthlyExpense()
        {
            decimal monthlyExpense=0;
            foreach (CashFlow flow in _cashFlowList)
            {
                if (flow is Expense)
                    monthlyExpense += flow.BudgetChangeValue;
            }
            return monthlyExpense;
        }

        public virtual decimal GetMonthlyIncome()
        {
            decimal monthlyIncome = 0;
            foreach (CashFlow flow in _cashFlowList)
            {
                if (flow is Income)
                    monthlyIncome += flow.BudgetChangeValue;
            }
            return monthlyIncome;
        }

        public virtual decimal GetTotalExpense()
        {
            return this.GetMonthlyExpense() * _months;
        }

        public virtual decimal GetTotalIncome()
        {
            return this.GetMonthlyIncome() * _months;
        }

        public virtual decimal GetMonthlyBalanceChange()
        {
            decimal balanceChange = 0;
            foreach (CashFlow flow in _cashFlowList)
            {
                balanceChange += flow.BudgetChangeValue;
            }
            return balanceChange;
        }

        public virtual decimal GetBalanceChangeByItem(string changeItem)
        {
            decimal balanceChange = 0;
            CashFlow cashFlow = _cashFlowList.Find(x => x.ItemName == changeItem);
            balanceChange = cashFlow.BudgetChangeValue;
            return balanceChange;
        }
        
    }
}
