using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletBusiness
{
    internal class Expense: CashFlow
    {
        public Expense(string name, decimal expenseValue) : base(name, expenseValue) 
        {
            BudgetChangeValue = expenseValue;
        }

        public override decimal BudgetChangeValue
        {
            get
            {
                return _balanceChangeValue;
            }
            set
            {
                if (value > 0)
                {
                    _balanceChangeValue = value*-1;
                }
                else
                {
                    _balanceChangeValue = value;
                }
            }
        }
    }
}
