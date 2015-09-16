﻿using System.Collections.Generic;
using GeneralTools.Contracts;
using GeneralTools.Services;

namespace CkgDomainLogic.General.Services
{
    public class PersistanceServiceSql : PersistanceBaseService 
    {
        protected override IEnumerable<IPersistableObjectContainer> ReadPersistedObjectContainers(string ownerKey, string groupKey)
        {
            var ct = CreateDbContext();

            var dbItems = ct.ReadPersistedObjectsFor(ownerKey, groupKey);

            return dbItems;
        }

        protected override void PersistObject(string objectKey, string ownerKey, string groupKey, string userName, string objectType, string objectData, string objectType2, string objectData2)
        {
            var ct = CreateDbContext();

            ct.PersistObject(objectKey, ownerKey, groupKey, userName, 
                (objectData.Contains(SaveIgnoreHint) ? "" : objectType),  objectData,
                (objectData2.Contains(SaveIgnoreHint) ? "" : objectType2), objectData2);
        }

        protected override void DeletePersistedObject(string objectKey)
        {
            var ct = CreateDbContext();

            ct.DeletePersistedObject(objectKey);
        }


        private static PersistanceSqlDbContext CreateDbContext()
        {
            return new PersistanceSqlDbContext();
        }
    }
}