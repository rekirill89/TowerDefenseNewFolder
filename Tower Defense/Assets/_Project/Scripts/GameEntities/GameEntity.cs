using System;
using UnityEngine;

namespace _Project.Scripts
{
    public abstract class GameEntity : MonoBehaviour
    {
        public abstract Type ConfigType { get; }

        public abstract void Init(GameEntityConfig config);
    }

    public abstract class GameEntity<TConfig> : GameEntity where TConfig : GameEntityConfig
    {
        public override Type ConfigType => typeof(TConfig);

        public sealed override void Init(GameEntityConfig config) => OnInit((TConfig)config);

        protected abstract void OnInit(TConfig config);
    }
}
