using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletBusiness
{
    internal class Income: CashFlow
    {
        public Income(string name, decimal incomeValue): base(name, incomeValue) 
        {
            BudgetChangeValue = incomeValue;
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
                    _balanceChangeValue = value;
                }
                else
                {
                    _balanceChangeValue = value*-1;
                }
            }
        } 
    }
}
