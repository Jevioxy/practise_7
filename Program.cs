using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Menu mainMenu = new Menu();
            mainMenu.Run();
        }        
    }
}
