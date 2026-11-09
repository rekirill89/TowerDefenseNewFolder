using UnityEngine;

namespace _Project.Scripts
{
    public interface IEntityFactory
    {
        public TEntity Create<TEntity>(Vector2 position) where TEntity : GameEntity;
    }
}
