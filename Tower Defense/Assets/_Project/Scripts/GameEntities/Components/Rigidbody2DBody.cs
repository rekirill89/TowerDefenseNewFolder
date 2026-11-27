using UnityEngine;

namespace _Project.Scripts.GameEntities
{
    public class Rigidbody2DBody : IBody2D
    {
        private readonly Rigidbody2D _rigidbody2D;

        public Rigidbody2DBody(Rigidbody2D rigidbody2D)
        {
            _rigidbody2D = rigidbody2D;
        }

        public Vector2 Position => _rigidbody2D.position;

        public void MovePosition(Vector2 position) => _rigidbody2D.MovePosition(position);

        public void MoveRotation(float angle) => _rigidbody2D.MoveRotation(angle);

        public void MovePositionAndRotation(Vector2 position, float angle) =>
            _rigidbody2D.MovePositionAndRotation(position, angle);
    }
}
