using Cysharp.Threading.Tasks;

namespace _Project.Scripts
{
    public class Archer : BaseTower
    {
        protected override void Start()
        {
            base.Start();
            ShootComponent.AttackAsync(Cts).Forget();
        }
    }
}