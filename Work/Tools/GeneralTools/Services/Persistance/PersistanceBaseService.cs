﻿using System;
using System.Collections.Generic;
using System.Linq;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace GeneralTools.Services
{
    public abstract class PersistanceBaseService : IPersistanceService
    {
        protected abstract IEnumerable<IPersistableObjectContainer> ReadPersistedObjectContainers(string ownerKey, string groupKey);

        protected abstract void PersistObject(string objectKey, string ownerKey, string groupKey, string userName, string objectType, string objectData);

        protected abstract void DeletePersistedObject(string objectKey);


        public IEnumerable<IPersistableObjectContainer> GetObjectContainers(string ownerKey, string groupKey)
        {
            var objectContainers = ReadPersistedObjectContainers(ownerKey, groupKey).ToListOrEmptyList();

            objectContainers.ForEach(objectContainer =>
                {
                    var type = Type.GetType(objectContainer.ObjectType);
                    if (type == null)
                        return;

                    var o = XmlService.XmlDeserializeFromString(objectContainer.ObjectData, type);
                    if (o is IPersistableObject)
                    {
                        var p = (o as IPersistableObject);
                        p.ObjectKey = objectContainer.ObjectKey;
                        p.EditUser = objectContainer.EditUser;
                        p.EditDate = objectContainer.EditDate;
                    }
                    objectContainer.Object = o;
                });

            return objectContainers;
        }

        public object SaveObject(string objectKey, string ownerKey, string groupKey, string userName, object o)
        {
            var type = o.GetType();
            var typeName = type.GetFullTypeName();
            var objectData = XmlService.XmlSerializeToString(o);

            var persistedContainersBeforeSave = (IEnumerable<IPersistableObjectContainer>)null;
            if (objectKey.IsNullOrEmpty())
                persistedContainersBeforeSave = GetObjectContainers(ownerKey, groupKey);


            PersistObject(objectKey, ownerKey, groupKey, userName, typeName, objectData);


            var persistedContainersAfterSave = (IEnumerable<IPersistableObjectContainer>)null;
            if (objectKey.IsNullOrEmpty())
                persistedContainersAfterSave = GetObjectContainers(ownerKey, groupKey);

            if (persistedContainersBeforeSave == null || persistedContainersAfterSave == null)
                return o;

            var persistedObjectsBeforeSave = persistedContainersBeforeSave.Where(pc => pc.Object as IPersistableObject != null).Select(pc => pc.Object as IPersistableObject);
            var persistedObjectsAfterSave = persistedContainersAfterSave.Where(pc => pc.Object as IPersistableObject != null).Select(pc => pc.Object as IPersistableObject);

            var newItem = persistedObjectsAfterSave.Except(persistedObjectsBeforeSave, new IPersistableObjectComparer()).FirstOrDefault();
            if (newItem != null)
            {
                objectKey = newItem.ObjectKey;
                objectData = XmlService.XmlSerializeToString(newItem);
                
                PersistObject(objectKey, ownerKey, groupKey, userName, typeName, objectData);
            }

            return newItem;
        }

        public void DeleteObject(string objectKey)
        {
            DeletePersistedObject(objectKey);
        }
    }
}
