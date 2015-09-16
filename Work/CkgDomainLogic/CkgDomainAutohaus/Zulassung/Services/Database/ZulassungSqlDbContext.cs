﻿using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.Autohaus.Models;

namespace CkgDomainLogic.Autohaus.Services
{
    public class ZulassungSqlDbContext : DbContext
    {
        public ZulassungSqlDbContext()
            : base(ConfigurationManager.AppSettings["Connectionstring"])
        {
        }

        public IEnumerable<Kennzeichengroesse> GetKennzeichengroessen()
        {
            return Database.SqlQuery<Kennzeichengroesse>("SELECT * FROM KennzeichGroesse WHERE Groesse IS NOT NULL");
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer<ZulassungSqlDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


        public IEnumerable<PersistableObjectContainer> GetPersistableObjectsFor(string ownerKey, string groupKey)
        {
            return Database.SqlQuery<PersistableObjectContainer>("SELECT * FROM PersistableObject WHERE OwnerKey = {0} and GroupKey = {0}", ownerKey, groupKey);
        }
    }
}