using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletBusiness
{
    internal class FamilyAccount: Account
    {
        public List<CoOwner> _coOwners = new List<CoOwner>();
        
        public FamilyAccount(decimal sum) : base(sum) 
        { 
            _accountType = AccountType.Family; 
        }

        public void AddExpense(string expenseItem, decimal expenseValue, string coOwnerName)
        {
            base.AddExpense(expenseItem, expenseValue);
            CoOwner coOwner = _coOwners.Find(x => x.Name == coOwnerName);
            coOwner.AddExpense(expenseItem, expenseValue);
        }
        public void AddIncome(string incomeItem, decimal incomeValue, string coOwnerName)
        {
            base.AddIncome(incomeItem, incomeValue);
            CoOwner coOwner = _coOwners.Find(x => x.Name == coOwnerName);
            coOwner.AddIncome(incomeItem, incomeValue);
        }

        public decimal GetMonthlyExpenseOfCoOwner(string coOwnerName)
        {
            CoOwner coOwner = _coOwners.Find(x => x.Name == coOwnerName);
            return coOwner.GetMonthlyExpense();
        }

        public decimal GetMonthlyIncomeOfCoOwner(string coOwnerName)
        {
            CoOwner coOwner = _coOwners.Find(x => x.Name == coOwnerName);
            return coOwner.GetMonthlyIncome();
            
        }

        public void AddCoOwner(string name)
        {
            _coOwners.Add(new CoOwner(name));
            _numberOfCoOwners++;
        }
    }

}
