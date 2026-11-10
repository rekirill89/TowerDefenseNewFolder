using _Project.Scripts.Services;

namespace _Project.Scripts.GameEntities
{
    public class Cannon : BaseTower
    {
        private void Start() => Model.Activate();

        protected override ObjectPool<Projectile> GetProjectilePool(IPoolProvider poolProvider) =>
            poolProvider.Get<Projectile, Cannonball>();
    }
}
