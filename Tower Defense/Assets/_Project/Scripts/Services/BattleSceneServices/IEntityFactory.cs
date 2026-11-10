using UnityEngine;
using _Project.Scripts.GameEntities;

namespace _Project.Scripts.Services
{
    public interface IEntityFactory
    {
        public TEntity Create<TEntity>(Vector2 position) where TEntity : GameEntity;
    }
}
