using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIPNetworkSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            string key;
            bool on = true;
            Network net = new Network();
            string n;
            string n2;
            string n3;
            net.NewNetwork();
            Console.WriteLine("Network Simulator");
            Console.WriteLine();
            while (on)
            {
                Console.WriteLine("1- Prideti Router");
                Console.WriteLine("2- Sukuriti link");
                Console.WriteLine("3- Panaikint Routeri");
                Console.WriteLine("4- Panaikint link");
                Console.WriteLine("5- Siusti zinute");
                Console.WriteLine("6- Isspausdinti Routing table");
                Console.WriteLine("7- Laiko tarpo praejimas");
                Console.WriteLine("8- Iseiti");
                key = Console.ReadLine();
                switch(Int32.Parse(key))
                {//reiktu dar belekiek exceptions sumetyt bet MAN PX
                    case 1:
                        Console.WriteLine("Irasykite Routerio pavadinima");
                        n=Console.ReadLine();
                        net.AddRouter(n);
                        Console.WriteLine("Pridetas Routeris " + n);
                        break;
                    case 2:
                        Console.WriteLine("Irasykite 2 Routeriu pavadinimus atskirtus Enter");
                        n = Console.ReadLine();
                        n2 = Console.ReadLine();
                        net.AddLink(n, n2);
                        Console.WriteLine("Pridetas rysis tarp " + n + " ir " + n2);
                        break;
                    case 3:
                        Console.WriteLine("Irasykite Routerio pavadinima");
                        n = Console.ReadLine();
                        net.RemoveRouter(n);
                        Console.WriteLine("Routeris " + n + " panaikintas");
                        break;
                    case 4:
                        Console.WriteLine("Irasykite 2 Routeriu pavadinimus atskirtus Enter");
                        n = Console.ReadLine();
                        n2 = Console.ReadLine();
                        net.RemoveLink(n, n2);
                        Console.WriteLine("RYsys tarp routeriu " + n + " ir " + n2 + " istrintas");
                        break;
                    case 5:
                        Console.WriteLine("Irasykite Routerio is kurio siunciama, tada routerio i kuri siunciama pavadinimus atskirtus Enter ir tada zinute");
                        net.SendMsg(Console.ReadLine(), Console.ReadLine(), Console.ReadLine());
                        Console.WriteLine("Zinute issiusta. Turi praeiti keli laiko tarpai kad ji butu gauta");
                        break;
                    case 6:
                        Console.WriteLine("Irasykite ALL jei norite visu arba Routerio Pavadinima kurio lenteles norite");
                        n = Console.ReadLine();
                        switch(n)
                        {
                            case "ALL":
                                net.PrintRoutingTable();
                                break;
                            default:
                                net.PrintRoutingTable(n);
                                break;
                        }
                        break;
                    case 7:
                        Console.WriteLine("Irasykite skaiciu kiek laiko tarpu norite kad praeitu");
                        n = Console.ReadLine();
                        net.Time(Int32.Parse(n));
                        Console.WriteLine("Praejo truputi laiko");
                        break;
                    case 8:
                        on=false;
                        break;
                    default:
                        Console.WriteLine("Neteisingai pasirinkote. Pasirinkite skaiciu nuo 1 iki 8");
                        break;
                }
            } 
        }
    }
}
