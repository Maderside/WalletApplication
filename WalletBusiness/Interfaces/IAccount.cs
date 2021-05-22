using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletBusiness
{
    //Interface for account necessary operations that implemented by Account and it`s derived classes
    public interface IAccount
    {
        
        void Put(decimal sum);
        
        decimal Withdraw(decimal sum);
    }
}
