using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletBusiness
{
    internal class PersonalAccount: Account
    {
        public PersonalAccount(decimal sum) : base(sum) 
        { 
            _accountType = AccountType.Personal;
            _numberOfCoOwners = 0;
        }

    }
}
