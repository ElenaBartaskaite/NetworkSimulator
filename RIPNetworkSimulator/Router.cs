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
        public event SentHandler routeTableSent;
        public string messege = null;

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
            On = on;
        }

        public void AddLink(Router otherRouter, int cost)//cost nesvarbu RIP algoritme bet o jeigu prireiks
        {
            Links.Add(otherRouter, cost);

            //tikrai buvo galima graziau parasyt, gal perrasyk
            RoutingTable temp = new RoutingTable();
            temp.Add(otherRouter, new Dictionary<Router, int>() { { otherRouter, 1 } });//i kur, per kur ir keliones ilgis(hop count).
            UpdateRouteTable(otherRouter, temp);
            //as esu fucking durna ir nezinau naudot this ar otherRouter
            otherRouter.routeTableSent += new SentHandler(UpdateRouteTable);
        }

        public void RemoveLink(Router otherRouter)
        {
            Links.Remove(otherRouter);
            //table.Remove(otherRouter);
            otherRouter.routeTableSent -= new SentHandler(UpdateRouteTable);
        }

        public void SendRouteTable()
        {
            routeTableSent(this, table);
        }

        public void SendMessege(string m, Router to)
        {
            Router via;
            via = findPath(to);
            if (via == null) Console.WriteLine("Router is unreacheable");
            else
            {
                via.messege = m;
                via.GetMessege(to);//reiks pakeist sita arba send funkcija nes per greitai
            }
        }

        public void GetMessege(Router to)
        {
            if (to == this) Console.WriteLine(messege);
            else SendMessege(messege, to);
            messege = null;
        }

        public void UpdateRouteTable(Router sender, RoutingTable recievedTable)
        {
            foreach(Router rTo in recievedTable.Keys.Where(x=>x!=this))
            {
                foreach (Router via in recievedTable[rTo].Keys.Where(x=> x!=this))
                {
                    if (++recievedTable[rTo][via] < 16)//cia siaip reiktu exception mest bet px
                    {
                        //jeigu dar neturi into apie routeri ta
                        if (!table.ContainsKey(rTo) && !table[rTo].ContainsKey(sender)) table.Add(rTo, new Dictionary<Router, int>() { { sender, recievedTable[rTo][via] } });
                        //updatina jei rastas trumpesnis kelias
                        else if (table[rTo][sender] > recievedTable[rTo][via]) table[rTo][sender] = recievedTable[rTo][via];
                    }
                       
                }
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
