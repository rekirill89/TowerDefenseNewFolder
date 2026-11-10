using UnityEngine;
using _Project.Scripts.GameEntities;

namespace _Project.Scripts.Configs
{
    public abstract class GameEntityConfig : BaseConfig
    {
        [field: SerializeField] public GameEntity Prefab { get; private set; }

        protected virtual void OnValidate()
        {
            if (Prefab != null && !Prefab.ConfigType.IsInstanceOfType(this))
                Debug.LogError($"{name}: prefab {Prefab.name} expects {Prefab.ConfigType.Name}", this);
        }
    }
}
