using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletBusiness
{
    internal class EventsOutput
    {
        //Output for Account events
        public static void ShowAccountMessage(object sender, AccountEventArgs arg)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (arg.Message.Contains("доход"))
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            else if ((arg.Message.Contains("расход")))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }
            Console.WriteLine(arg.Message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        //Output for Wallet events
        public static void ShowWalletMessage(object sender, WalletEventArgs arg)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (arg.Message.Contains("доход"))
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            else if ((arg.Message.Contains("расход")))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }
            Console.WriteLine(arg.Message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

    }
}
