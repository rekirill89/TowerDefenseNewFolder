using System;
using UnityEngine;

namespace _Project.Scripts.GameEntities
{
    public interface ITarget : IDamageable
    {
        event Action<ITarget> OnDespawned;

        Vector2 Position { get; }
    }
}
