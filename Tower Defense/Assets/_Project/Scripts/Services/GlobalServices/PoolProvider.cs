using System;
using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.GameEntities;

namespace _Project.Scripts.Services
{
    public class PoolProvider : IPoolProvider
    {
        private readonly IEntityFactory _entityFactory;
        private readonly Dictionary<Type, object> _pools = new();

        public PoolProvider(IEntityFactory entityFactory) => _entityFactory = entityFactory;

        public ObjectPool<TEntity> Get<TEntity>() where TEntity : GameEntity, IPoolable<TEntity>
            => Get<TEntity, TEntity>();

        public ObjectPool<TPooled> Get<TPooled, TEntity>()
            where TPooled : GameEntity, IPoolable<TPooled>
            where TEntity : TPooled
        {
            if (!_pools.TryGetValue(typeof(TEntity), out var pool))
            {
                pool = new ObjectPool<TPooled>(() => _entityFactory.Create<TEntity>(Vector2.zero));
                _pools.Add(typeof(TEntity), pool);
            }
            return (ObjectPool<TPooled>)pool;
        }
    }

    public interface IPoolProvider
    {
        public ObjectPool<TEntity> Get<TEntity>() where TEntity : GameEntity, IPoolable<TEntity>;

        public ObjectPool<TPooled> Get<TPooled, TEntity>()
            where TPooled : GameEntity, IPoolable<TPooled>
            where TEntity : TPooled;
    }
}
