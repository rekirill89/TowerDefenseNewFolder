using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameEntities
{
    [RequireComponent(typeof(Collider2D))]
    public class EnemyDetector : MonoBehaviour
    {
        private float _detectionRadius;

        private List<Enemy> _enemies;
        private CircleCollider2D _circleCollider2D;
        
        private void Awake()
        {
            _circleCollider2D = GetComponent<CircleCollider2D>();
            
            _enemies = new List<Enemy>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
            {
                if (!_enemies.Contains(enemy))
                {
                    enemy.OnDespawned += OnEnemyDespawnedHandler;
                    _enemies.Add(enemy);
                }}
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
            {
                if (_enemies.Contains(enemy))
                {
                    enemy.OnDespawned -= OnEnemyDespawnedHandler;
                    _enemies.Remove(enemy);
                }
            }
        }

        public void Init(float detectionRadius = -1)
        {
            _detectionRadius = Mathf.Approximately(detectionRadius, -1f) ? _detectionRadius :  detectionRadius;
            SetDetectionRadius(detectionRadius);
        }

        public Enemy GetClosestEnemy()
        {
            if (_enemies.Count == 0)
                return null;

            var closestEnemy = _enemies[0];
            float closestSqrDistance = float.MaxValue;
            foreach (var enemy in _enemies)
            {
                float sqrDistance = (enemy.transform.position - transform.position).sqrMagnitude;
                if (sqrDistance < closestSqrDistance)
                {
                    closestSqrDistance = sqrDistance;
                    closestEnemy = enemy;
                }
            }
            return closestEnemy;
        }

        private void SetDetectionRadius(float detectionRadius)
        {
            _detectionRadius = detectionRadius;
            _circleCollider2D.radius = detectionRadius;
        }

        private void OnEnemyDespawnedHandler(Enemy enemy)
        {
            enemy.OnDespawned -= OnEnemyDespawnedHandler;
            _enemies.Remove(enemy);
        }
    }
}