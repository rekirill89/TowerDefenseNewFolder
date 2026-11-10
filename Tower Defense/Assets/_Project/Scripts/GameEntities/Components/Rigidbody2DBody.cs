using UnityEngine;

namespace _Project.Scripts.GameEntities
{
    public class Rigidbody2DBody : IBody2D
    {
        private readonly Rigidbody2D _rb;

        public Rigidbody2DBody(Rigidbody2D rb)
        {
            _rb = rb;
        }

        public Vector2 Position => _rb.position;

        public void MovePosition(Vector2 position) => _rb.MovePosition(position);

        public void MoveRotation(float angle) => _rb.MoveRotation(angle);

        public void MovePositionAndRotation(Vector2 position, float angle) =>
            _rb.MovePositionAndRotation(position, angle);
    }
}
