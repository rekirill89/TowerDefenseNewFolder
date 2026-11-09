using Cysharp.Threading.Tasks;

namespace _Project.Scripts
{
    public class Archer : BaseTower
    {
        private void Start()
        {
            ShootComponent.AttackAsync(Cts).Forget();
        }

        protected override ObjectPool<Projectile> GetProjectilePool(IPoolProvider poolProvider) =>
            poolProvider.Get<Projectile, Arrow>();
    }
}
