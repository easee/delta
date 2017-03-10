using Billing.Database;
using Billing.Repository;
using System;
using System.Data;
using System.Linq;

namespace Billing.Seed
{
    public class Shippers
    {
        public static void Get()
        {
            IBillingRepository<Shipper> shippers = new BillingRepository<Shipper>(Help.Context);
            IBillingRepository<Town> towns = new BillingRepository<Town>(Help.Context);
            DataTable rawData = Help.OpenExcel("Shippers");
            int N = 0;
            foreach (DataRow row in rawData.Rows)
            {
                int oldId = Help.getInteger(row, 0);
                string zip = Help.getString(row, 1);
                Town town = towns.Get().FirstOrDefault(x => x.Zip == zip);
                Shipper ship = new Shipper()
                {
                    Name = Help.getString(row, 2),
                    Address = Help.getString(row, 3),
                    Town = town
                };
                N++;
                shippers.Insert(ship);
                shippers.Commit();
                Help.dicShip.Add(oldId, ship.Id);
            }
            Console.WriteLine(N);
        }
    }
}
