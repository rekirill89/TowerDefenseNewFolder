using System;
using _Project.Scripts.MVP;
using UnityEngine;

namespace _Project.Scripts
{
    public interface IStructureConstructService
    {
        event Action OnStructureConstructed;

        void TryConstructStructure(StructureEnum structureEnum, Vector2 pos);
        void StopAllTowers();
        void RemoveAllTowers();
    }
}