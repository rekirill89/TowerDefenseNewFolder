using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameEntities
{
    [RequireComponent(typeof(Collider2D))]
    public class EnemyDetector : MonoBehaviour
    {
        private float _detectionRadius;
        
        public List<Enemy> Enemies { get; private set; }
        private CircleCollider2D _circleCollider2D;
        private Enemy _closestEnemy;
        
        private void Awake()
        {
            _circleCollider2D = GetComponent<CircleCollider2D>();
            
            Enemies = new List<Enemy>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
            {
                if (!Enemies.Contains(enemy))
                {
                    enemy.OnDespawned += OnEnemyDespawnedHandler;
                    Enemies.Add(enemy);
                }}
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
            {
                if (Enemies.Contains(enemy))
                {
                    enemy.OnDespawned -= OnEnemyDespawnedHandler;
                    Enemies.Remove(enemy);
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
            if (Enemies.Count == 0)
                return null;

            _closestEnemy = Enemies[0];
            float closestDistance = float.MaxValue;
            foreach (var enemy in Enemies)
            {
                var currentDistance = Vector2.Distance(transform.position, enemy.transform.position);
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    _closestEnemy = enemy;
                }
            }
            return _closestEnemy;
        }

        private void SetDetectionRadius(float detectionRadius)
        {
            _detectionRadius = detectionRadius;
            _circleCollider2D.radius = detectionRadius;
        }

        private void OnEnemyDespawnedHandler(Enemy enemy)
        {
            enemy.OnDespawned -= OnEnemyDespawnedHandler;
            Enemies.Remove(enemy);
        }
    }
}