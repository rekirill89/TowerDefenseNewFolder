using UnityEngine;

namespace _Project.Scripts.GameEntities
{
    public class EnemyVisual : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _rotationSpeed = 10f;

        private const string IS_MOVE = "IsMove";
        private const string ATTACK = "Attack";

        private EnemyModel _model;

        public void Bind(EnemyModel model)
        {
            Unbind();
            _model = model;
            _model.OnAttacked += OnAttackedHandler;
        }

        private void OnDestroy() => Unbind();

        private void OnDisable()
        {
            transform.localRotation = Quaternion.identity;
        }

        private void Update()
        {
            if (_model == null)
                return;

            _animator.SetBool(IS_MOVE, _model.State == EnemyState.Moving);

            if (_model.State is EnemyState.Moving or EnemyState.Attacking)
                RotateToTarget();
        }

        private void Unbind()
        {
            if (_model != null)
                _model.OnAttacked -= OnAttackedHandler;
            _model = null;
        }

        private void OnAttackedHandler()
        {
            _animator.SetTrigger(ATTACK);
        }

        private void RotateToTarget()
        {
            Vector2 direction = _model.Destination - _model.Position;
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            float deltaAngle = Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle);

            transform.Rotate(0f, deltaAngle * Time.deltaTime * _rotationSpeed, 0f);
        }
    }
}
