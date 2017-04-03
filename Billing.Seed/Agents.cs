using Billing.Database;
using Billing.Repository;
using System;
using System.Data;
using System.Linq;

namespace Billing.Seed
{
    public class Agents
    {
        public static void Get()
        {
            IBillingRepository<Agent> agents = new BillingRepository<Agent>(Help.Context);
            IBillingRepository<Town> towns = new BillingRepository<Town>(Help.Context);
            DataTable rawData = Help.OpenExcel("Agents");
            int N = 0;
            foreach (DataRow row in rawData.Rows)
            {
                int oldId = Help.getInteger(row, 0);
                Agent agent = new Agent()
                {
                    Name = Help.getString(row, 1)
                };
                N++;
                string[] Zone = Help.getString(row, 2).Split('.');
                foreach (string Z in Zone)
                {
                    Database.Region R = (Database.Region)Convert.ToInt32(Z);
                    var area = towns.Get().Where(x => x.Region == R).ToList();
                    foreach (var city in area)
                    {
                        agent.Towns.Add(city);
                    }
                }
                agents.Insert(agent);
                agents.Commit();
                Help.dicAgen.Add(oldId, agent.Id);
            }
            Console.WriteLine(N);
        }
    }
}