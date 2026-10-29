using System;
using System.Threading;
using _Project.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts
{
    public class BaseTower : MonoBehaviour
    {
        [field: SerializeField] public AttackStructureSO AttackStructureSo { get;private set; }
        [SerializeField] private EnemyDetector _enemyDetector;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private ProjectileSO _projectileSO;
        public ShootComponent ShootComponent {get; private set;}
        protected  CancellationTokenSource Cts = new CancellationTokenSource();
        private Enemy _closestEnemy;
        
        public void StopActing() => Cts.Cancel();
        
        protected virtual void Awake()
        {
            ShootComponent = new ShootComponent(
                _projectilePrefab, 
                transform, 
                _closestEnemy, 
                _projectileSO,
                AttackStructureSo);
        }

        protected virtual void Start()
        {
            _enemyDetector.Init(AttackStructureSo.AttackRange);
        }

        protected virtual void Update()
        {
            CheckTargets();
        }

        protected virtual void OnDestroy()
        {
            Cts.Cancel();
        }
        
        protected void CheckTargets()
        {
            if (_enemyDetector.Enemies.Count == 0)
            {
                _closestEnemy =  null;
                ShootComponent.SetTarget(_closestEnemy);
                return;
            }
            
            _closestEnemy = _enemyDetector.GetClosestEnemy();
            ShootComponent.SetTarget(_closestEnemy);
        }
    }
}