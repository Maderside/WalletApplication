using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletBusiness
{
    internal class CoOwner
    {
        protected string _name;

        public string Name
        {
            get { return _name; }
            private set { }
        }

        internal List<CashFlow> _pesonCashFlowList = new List<CashFlow>();
        public CoOwner(string name)
        {
            _name = name;
        }

        public void AddExpense(string name, decimal expenseValue)
        {
            Expense expense = new Expense(name, expenseValue);
            _pesonCashFlowList.Add(expense);
        }
        public void AddIncome(string name, decimal incomeValue)
        {
            Income income = new Income(name, incomeValue);
            _pesonCashFlowList.Add(income);
        }

        public decimal GetMonthlyExpense()
        {
            decimal monthlyExpense = 0;
            foreach (CashFlow flow in _pesonCashFlowList)
            {
                if (flow is Expense)
                    monthlyExpense += flow.BudgetChangeValue;
            }
            return monthlyExpense;
        }
        public decimal GetMonthlyIncome()
        {
            decimal monthlyIncome = 0;
            foreach (CashFlow flow in _pesonCashFlowList)
            {
                if (flow is Income)
                    monthlyIncome += flow.BudgetChangeValue;
            }
            return monthlyIncome;
        }
    }
}
