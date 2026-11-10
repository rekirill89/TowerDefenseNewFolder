using System;
using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.GameEntities;

namespace _Project.Scripts.MVP
{
    public class StrongholdUpgradePanelView : BasePanelView
    {
        public event Action<StrongholdUpgradeType> OnUpgradeClicked;
        [SerializeField] private CustomButton _upgradeDamageButton;
        [SerializeField] private CustomButton _upgradeAttackRateButton;
        [SerializeField] private CustomButton _upgradeHealthButton;

        private Dictionary<StrongholdUpgradeType, Action> _clickHandlers;

        private void Awake()
        {
            _clickHandlers = new Dictionary<StrongholdUpgradeType, Action>()
            {
                { StrongholdUpgradeType.Damage, () => { OnUpgradeClicked?.Invoke(StrongholdUpgradeType.Damage); } },
                { StrongholdUpgradeType.AttackRate, () => { OnUpgradeClicked?.Invoke(StrongholdUpgradeType.AttackRate); } },
                { StrongholdUpgradeType.Health, () => { OnUpgradeClicked?.Invoke(StrongholdUpgradeType.Health); } },
            };
        }

        private void Start()
        {
            _upgradeAttackRateButton.OnClick += _clickHandlers[StrongholdUpgradeType.AttackRate];
            _upgradeDamageButton.OnClick += _clickHandlers[StrongholdUpgradeType.Damage];
            _upgradeHealthButton.OnClick += _clickHandlers[StrongholdUpgradeType.Health];
        }

        private void OnDestroy()
        {
            _upgradeAttackRateButton.OnClick -= _clickHandlers[StrongholdUpgradeType.AttackRate];
            _upgradeDamageButton.OnClick -= _clickHandlers[StrongholdUpgradeType.Damage];
            _upgradeHealthButton.OnClick -= _clickHandlers[StrongholdUpgradeType.Health];
        }
    }
}