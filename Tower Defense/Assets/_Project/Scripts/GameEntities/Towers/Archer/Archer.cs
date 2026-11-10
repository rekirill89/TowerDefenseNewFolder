using _Project.Scripts.Services;

namespace _Project.Scripts.GameEntities
{
    public class Archer : BaseTower
    {
        private void Start() => Model.Activate();

        protected override ObjectPool<Projectile> GetProjectilePool(IPoolProvider poolProvider) =>
            poolProvider.Get<Projectile, Arrow>();
    }
}
