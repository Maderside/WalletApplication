using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletBusiness
{
    public delegate void AccountEventsHandler(object sender, AccountEventArgs e);

    public class AccountEventArgs
    {
        
        public string Message { get; private set; }
        public string FlowName { get; private set; }
        
        public decimal Sum { get; private set; }

        public AccountEventArgs(string _mes, decimal _sum)
        {
            Message = _mes;
            Sum = _sum;
        }
        public AccountEventArgs(string _mes)
        {
            Message = _mes;
        }
        
    }
}
