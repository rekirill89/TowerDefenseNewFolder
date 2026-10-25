using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts
{
    public class Cannon : BaseTower
    {
        protected override void Start()
        {
            base.Start();
            ShootComponent.AttackAsync(Cts).Forget();
        }
    }
}