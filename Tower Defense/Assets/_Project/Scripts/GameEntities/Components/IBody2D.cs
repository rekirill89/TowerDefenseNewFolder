using UnityEngine;

namespace _Project.Scripts.GameEntities
{
    public interface IBody2D
    {
        Vector2 Position { get; }

        void MovePosition(Vector2 position);
        void MoveRotation(float angle);
        void MovePositionAndRotation(Vector2 position, float angle);
    }
}
