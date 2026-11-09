using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    public class ConfigProvider
    {
        private readonly Dictionary<Type, BaseConfig> _configs = new();
        private readonly Dictionary<Type, GameEntityConfig> _entityConfigs = new();

        public ConfigProvider(IEnumerable<BaseConfig> configs)
        {
            foreach (var config in configs)
            {
                if (config == null)
                {
                    Debug.LogError("Config list contains an empty slot");
                    continue;
                }

                if (config is GameEntityConfig entityConfig)
                    AddEntityConfig(entityConfig);
                else if (!_configs.TryAdd(config.GetType(), config))
                    Debug.LogError($"Duplicate config of type {config.GetType().Name}", config);
            }
        }

        public TConfig GetConfig<TConfig>() where TConfig : BaseConfig
        {
            if (_configs.TryGetValue(typeof(TConfig), out var config))
                return (TConfig)config;
            throw new KeyNotFoundException($"No config of type {typeof(TConfig).Name}");
        }

        public GameEntityConfig GetEntityConfig<TEntity>() where TEntity : GameEntity
        {
            if (_entityConfigs.TryGetValue(typeof(TEntity), out var config))
                return config;
            throw new KeyNotFoundException($"No config with prefab {typeof(TEntity).Name}");
        }

        public TConfig GetEntityConfig<TEntity, TConfig>()
            where TEntity : GameEntity
            where TConfig : GameEntityConfig
            => (TConfig)GetEntityConfig<TEntity>();

        private void AddEntityConfig(GameEntityConfig config)
        {
            if (config.Prefab == null)
            {
                Debug.LogError($"{config.name} has no prefab", config);
                return;
            }

            var entityType = config.Prefab.GetType();
            if (!_entityConfigs.TryAdd(entityType, config))
                Debug.LogError($"Duplicate config for {entityType.Name}", config);
        }
    }
}
