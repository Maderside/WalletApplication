using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletBusiness
{
    abstract internal class CashFlow
    {
        public CashFlow(string name, decimal incomeValue)
        {
            _itemName = name;
            _balanceChangeValue = incomeValue;
        }

        protected decimal _balanceChangeValue;

        protected string _itemName;
        public abstract decimal BudgetChangeValue { get; set; }
        public string ItemName { get { return _itemName; } }
    }
}
