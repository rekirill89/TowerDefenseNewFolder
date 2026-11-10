using UnityEngine;

namespace _Project.Scripts.GameEntities
{
    public class MoveComponent
    {
        private readonly IBody2D _body;
        private float _speed;

        public MoveComponent(IBody2D body)
        {
            _body = body;
        }

        public void Init(float speed)
        {
            _speed = speed;
        }

        public void MoveTo(Vector2 target, float deltaTime)
        {
            _body.MovePosition(GetNextPosition(target, deltaTime));
        }

        public void RotateTo(Vector2 target)
        {
            _body.MoveRotation(GetAngleTo(target));
        }

        public void MoveAndRotateTo(Vector2 target, float deltaTime)
        {
            _body.MovePositionAndRotation(GetNextPosition(target, deltaTime), GetAngleTo(target));
        }

        private Vector2 GetNextPosition(Vector2 target, float deltaTime)
        {
            var direction = (target - _body.Position).normalized;
            return _body.Position + direction * _speed * deltaTime;
        }

        private float GetAngleTo(Vector2 target)
        {
            var direction = (target - _body.Position).normalized;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
    }
}
