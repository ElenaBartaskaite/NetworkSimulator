using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIPNetworkSimulator
{
    public class Network
    {
        List<Router> Routers;
        public int timmer = 0;//event pasikeitee laikas visi routeriai issiunccia ka turi

        public Network()
        {
            Routers = new List<Router>();
        }

        public Router AddRouter(string name, bool on = true)
        {
            if (GetRouterByName(name) == null)
            {
                Router router = new Router(name, on);
                Routers.Add(router);
                return router; 
            }
            else
            {
                return null;
            }
        }
        public void SendMsg(string from, string to, string msg)
        {
            GetRouterByName(from).CreateMessege(GetRouterByName(to), msg);
        }
        public void PrintRoutingTable()
        {
            foreach (Router r in Routers)
            {
                PrintRoutingTable(r);
            }
        }
        public void PrintRoutingTable(string router)
        {
            PrintRoutingTable(GetRouterByName(router));
        }
        public void PrintRoutingTable(Router router)
        {
            router.PrintTable();
        }
        public void Time(int amount)
        { 
            //gal koki eventa ipist
            while(amount>0)
            {
                DecrementTimeouts();
                SendRoutingTables();
                CheckRouterTimeouts();
                SendMesseges();
                timmer++;
                amount--;
            }
        }

        public bool RemoveRouter(string name)
        {
            Router router = GetRouterByName(name);
            if (router != null)
            {
                foreach (Router r in router.Links.Keys)
                {
                    r.RemoveLink(router);
                }
                Routers.Remove(router);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SendMesseges()
        {
            foreach (Router a in Routers.Where(x => x.messegeTimeout == 0))
            {
                a.SendMessege();
            }
        }

        public void SendRoutingTables()
        {
            foreach(Router a in Routers)
            {
                a.SendRouteTable();
            }
        }

        private void DecrementTimeouts()
        {
            foreach(Router a in Routers)
            {
                if (a.messegeTimeout == 0)
                {
                    a.messegeTimeout = a.messegeTimeoutMax;
                }
                a.messegeTimeout--;
                List<Router> links = a.Links.Keys.ToList();
                foreach(var link in links)
                {
                    a.Links[link]--;
                }
            }
        }

        private void CheckRouterTimeouts()
        {
            foreach(Router a in Routers)
            {
                a.CheckLinkTimeouts();
            }
        }

        public Router GetRouterByName(string router)
        {
            return Routers.Where(x => x.Name == router).FirstOrDefault();
        }

        public bool AddLink(string router1, string router2)
        {
            return AddLink(GetRouterByName(router1), GetRouterByName(router2));
        }

        public bool AddLink(Router router1, Router router2)
        {
            bool added = false;
            if (!Routers.Contains(router1) || !Routers.Contains(router2)) return false;
            if (router1.Links.Keys.Contains(router2) == false)
            {
                router1.AddLink(router2);
                added = true;
            }
            if (router2.Links.Keys.Contains(router1) == false)
            {
                router2.AddLink(router1);
                added = true;
            }
            return added;
        }

        public bool RemoveLink(string router1, string router2)
        {
            return RemoveLink(GetRouterByName(router1), GetRouterByName(router2));
        }
        public void NewNetwork()
        {
            //cia reiktu nuskaityt nuo file ir sudeliot
        }

        public bool RemoveLink(Router router1, Router router2)
        {
            bool removed = false;
            if (router1 != null && router2 != null)
            {
                if (router1.Links.Keys.Contains(router2) == true)
                {
                    router1.RemoveLink(router2);
                    removed = true;
                }
                if (router2.Links.Keys.Contains(router1) == true)
                {
                    router2.RemoveLink(router1);
                    removed = true;
                }
            }
            return removed;
        }
        
    }
}
