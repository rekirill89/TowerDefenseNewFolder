using System;
using UnityEngine;
using _Project.Scripts.GameEntities;

namespace _Project.Scripts.Services
{
    public interface ITowerConstructionService
    {
        event Action OnTowerConstructed;

        void TryConstructTower(TowerType towerType, Vector2 position);
        void StopAllTowers();
        void RemoveAllTowers();
    }
}