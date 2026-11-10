using UnityEngine;
using _Project.Scripts.MVP;

namespace _Project.Scripts.GameEntities
{
    public class StrongholdUI : MonoBehaviour
    {
        [field: SerializeField] public StrongholdUpgradePanelView UpgradePanel { get; private set; }
    }
}
