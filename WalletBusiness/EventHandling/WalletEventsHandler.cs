using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletBusiness
{
    public delegate void WalletEventsHandler(object sender, WalletEventArgs e);

    public class WalletEventArgs
    {
        
        public string Message { get; private set; }
        

        public WalletEventArgs(string _mes, decimal _sum)
        {
            Message = _mes;
        }
        public WalletEventArgs(string _mes)
        {
            Message = _mes;
        }
    }
}
