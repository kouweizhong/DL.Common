﻿using System.Collections.Generic;
using System.Linq;
using DL.Core.Contracts;

namespace DL.Core.Data.InMemory
{
    public abstract class InMemoryRepositoryBase<TEntityType, TKey> where TEntityType : class, IIdentifiable<TKey>
    {
        protected InMemoryRepositoryBase()
        {
            this.Dictionary = new Dictionary<TKey, TEntityType>();
        }

        public Dictionary<TKey, TEntityType> Dictionary { get; private set; }

        public void Delete(TEntityType entity)
        {
            this.Dictionary.Remove(entity.Id);
        }

        public abstract TKey GetNewId();

        public void Save(TEntityType entity)
        {
            if (this.ContainsItem(entity))
            {
                this.Update(entity);
            }
            else
            {
                this.Add(entity);
            }
        }

        public IEnumerable<TEntityType> SelectAll()
        {
            return this.Dictionary.Values.Where(x => x != null).ToArray();
        }

        public TEntityType SelectById(TKey id)
        {
            return this.Dictionary.ContainsKey(id) ? this.Dictionary[id] : null;
        }

        private bool ContainsItem(TEntityType entity)
        {
            return this.Dictionary.ContainsKey(entity.Id) && this.Dictionary[entity.Id] != null;
        }

        private void Add(TEntityType entity)
        {
            TKey newId = this.GetNewId();
            this.Dictionary.Add(newId, entity);
        }

        private void Update(TEntityType entity)
        {
            this.Dictionary[entity.Id] = entity;
        }
    }
}
