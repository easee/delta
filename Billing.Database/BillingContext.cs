﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Billing.Database
{
    public class BillingContext : DbContext
    {
        public BillingContext() : base("name=Billing") { }//Znači da naslijeđuje konstruktor od bazne klase.

        //Članovi konteksta će biti oni entiteti za koje planiramo da budu dio baze podataka
        //ApiUsers i Tokens smo dodali kad smo počeli koristiti Authentikaciju
        public DbSet<ApiUser> ApiUsers { get; set; }
        public DbSet<AuthToken> Tokens { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Procurement> Procurements { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Town> Towns { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();


            modelBuilder.Ignore<Basic>();
            modelBuilder.Entity<Agent>().Map<Agent>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
            modelBuilder.Entity<Category>().Map<Category>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
            modelBuilder.Entity<Customer>().Map<Customer>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
            modelBuilder.Entity<Invoice>().Map<Invoice>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
            modelBuilder.Entity<Item>().Map<Item>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
            modelBuilder.Entity<Procurement>().Map<Procurement>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
            modelBuilder.Entity<Product>().Map<Product>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
            modelBuilder.Entity<Shipper>().Map<Shipper>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
            modelBuilder.Entity<Supplier>().Map<Supplier>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
            modelBuilder.Entity<Stock>().Map<Stock>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
            modelBuilder.Entity<Town>().Map<Town>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(p => p.State == EntityState.Deleted)) SoftDelete(entry);
            return base.SaveChanges();
        }

        private void SoftDelete(DbEntityEntry entry)
        {
            Type entryEntityType = entry.Entity.GetType();

            string tableName = GetTableName(entryEntityType);
            string primaryKeyName = GetPrimaryKeyName(entryEntityType);
            string deletequery = string.Format("UPDATE {0} SET Deleted = 1 WHERE {1} = @id", tableName, primaryKeyName);

            Database.ExecuteSqlCommand(deletequery, new SqlParameter("@id", entry.OriginalValues[primaryKeyName]));

            //entry.State = EntityState.Detached; Gigi rekao da se zakomentariše.
        }

        private static Dictionary<Type, EntitySetBase> _mappingCache =
                    new Dictionary<Type, EntitySetBase>();

        private EntitySetBase GetEntitySet(Type type)
        {
            if (!_mappingCache.ContainsKey(type))
            {
                ObjectContext octx = ((IObjectContextAdapter)this).ObjectContext;

                string typeName = ObjectContext.GetObjectType(type).Name;

                var es = octx.MetadataWorkspace
                                .GetItemCollection(DataSpace.SSpace)
                                .GetItems<EntityContainer>()
                                .SelectMany(c => c.BaseEntitySets.Where(e => e.Name == typeName))
                                .FirstOrDefault();

                if (es == null) throw new ArgumentException("Entity type not found in GetTableName", typeName);

                _mappingCache.Add(type, es);
            }

            return _mappingCache[type];
        }

        private string GetTableName(Type type)
        {
            EntitySetBase es = GetEntitySet(type);
            return string.Format("[{0}].[{1}]", es.MetadataProperties["Schema"].Value, es.MetadataProperties["Table"].Value);
        }

        private string GetPrimaryKeyName(Type type)
        {
            EntitySetBase es = GetEntitySet(type);
            return es.ElementType.KeyMembers[0].Name;
        }
    }
}
        