using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIPNetworkSimulator
{
    public delegate void SentHandler(Router sender, RoutingTable sentTable);
    public class Router
    {
        string name;
        bool on;
        RoutingTable table;
        Dictionary<Router, int> links;
        public Messege messege;
        int routerTimeout = 3;
        public int messegeTimeoutMax = 1;
        public int messegeTimeout = 1;

        public string Name
        {
            get
            {
                return name;
            }
            private set
            {
                name = value;
            }
        }

        public bool On
        {
            get
            {
                return on;
            }
            private set
            {
                on = value;
            }
        }

        public Dictionary<Router, int> Links
        {
            get
            {
                return links;
            }
            private set
            {
                links = value;
            }
        }

        public Router(string name, bool on = true)
        {
            Name = name;
            Links = new Dictionary<Router, int>();
            table = new RoutingTable();
            On = on;
        }

        public void AddLink(Router otherRouter)//cost nesvarbu RIP algoritme bet o jeigu prireiks
        {
            if (Links.ContainsKey(otherRouter)) return;
            Links.Add(otherRouter, routerTimeout);
            //tikrai buvo galima graziau parasyt, gal perrasyk
            RoutingTable temp = new RoutingTable();
            temp.Add(otherRouter, new Dictionary<Router, int>() { { otherRouter, 0 } });//i kur, per kur ir keliones ilgis(hop count).
            UpdateRouteTable(otherRouter, temp);
            //as esu fucking durna ir nezinau naudot this ar otherRouters
        }

        public void RemoveLink(Router otherRouter)
        {
            Links.Remove(otherRouter);
            //table.Remove(otherRouter);
        }

        public void SendRouteTable()
        {
            foreach (Router r in Links.Keys)
            {
                r.UpdateRouteTable(this, table);
            }
        }

        public void CheckLinkTimeouts()
        {
            List<Router> linkedRouters = Links.Keys.ToList();
            foreach (Router r in linkedRouters)
            {
                if (Links[r] == 0)
                {
                    Links[r] = 16;
                }
            }
        }

        public Messege CreateMessege(Router to, string m)
        {
            return messege = new Messege() { destination = to, messege = m};
        }

        public void SendMessege()
        {
            if (messege != null)
            {
                Router via = findPath(messege.destination);
                if (via == null)
                {
                    Console.WriteLine("Router is unreacheable");
                }
                else
                {
                    via.GetMessege(messege);
                }
                messege = null;
            }
        }

        public void GetMessege(Messege m)
        {
            if (m.destination == this)
            {
                Console.WriteLine(m.messege);
                messege = null;
                return;
            }
            messege = m;
        }

        public void UpdateRouteTable(Router sender, RoutingTable recievedTable)
        {
            Links[sender] = routerTimeout;
            foreach(Router rTo in recievedTable.Keys.Where(x=>x!=this))
            {
                if (!table.ContainsKey(rTo))
                {
                    table.Add(rTo, new Dictionary<Router, int>());
                }
                if (!table[rTo].ContainsKey(sender))
                {
                    table[rTo].Add(sender, recievedTable[rTo].First().Value + 1);
                }
                table[rTo][sender] = Math.Min(16, recievedTable[rTo].Values.Min() + 1);
            }
        }

        public Router findPath(Router to)
        {
            Router next=null;
            int i = 16;
            foreach (Router a in table[to].Keys) if (table[to][a] < i) next = a;
            return next;
        }

        public void PrintTable()
        {
            Console.WriteLine();
            Console.WriteLine("Router " + this.name + " routing table");
            foreach(Router a in table.Keys)
            {
                Console.Write("To " + a.name + "    ");
                foreach (Router b in table[a].Keys)
                {
                    Console.Write("via  " + b.name + " " + table[a][b] + " hops ");
                }
            }
            Console.WriteLine();
        }


    }
}
