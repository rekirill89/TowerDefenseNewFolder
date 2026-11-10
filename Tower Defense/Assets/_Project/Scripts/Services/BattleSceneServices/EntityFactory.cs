using UnityEngine;
using Zenject;
using _Project.Scripts.GameEntities;

namespace _Project.Scripts.Services
{
    public class EntityFactory : IEntityFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly ConfigProvider _configProvider;

        public EntityFactory(IInstantiator instantiator, ConfigProvider configProvider)
        {
            _instantiator = instantiator;
            _configProvider = configProvider;
        }

        public TEntity Create<TEntity>(Vector2 position) where TEntity : GameEntity
        {
            var config = _configProvider.GetEntityConfig<TEntity>();
            var entity = _instantiator
                .InstantiatePrefabForComponent<TEntity>((TEntity)config.Prefab, position, Quaternion.identity, null);
            entity.Init(config);
            return entity;
        }
    }
}
