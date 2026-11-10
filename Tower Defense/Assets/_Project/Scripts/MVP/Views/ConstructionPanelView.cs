using System;
using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.GameEntities;

namespace _Project.Scripts.MVP
{
    public class ConstructionPanelView : BasePanelView
    {
        public event Action<TowerType> OnConstructButtonClicked;
        [field: SerializeField] public Canvas LocalCanvas { get; private set; }
        [SerializeField] private CustomButton _constructArcherButton; 
        [SerializeField] private CustomButton _constructCannonButton; 
        
        private Dictionary<TowerType, Action> _clickHandlers;
        
        private void Start()
        {
            _clickHandlers = new Dictionary<TowerType, Action>()
            {
                { TowerType.Archer, () => OnConstructButtonClickedHandler(TowerType.Archer) },
                { TowerType.Cannon, () => OnConstructButtonClickedHandler(TowerType.Cannon) },
            };
            
            _constructCannonButton.OnClick += _clickHandlers[TowerType.Cannon];
            _constructArcherButton.OnClick += _clickHandlers[TowerType.Archer];
        }

        private void OnDestroy()
        {
            _constructCannonButton.OnClick -= _clickHandlers[TowerType.Cannon];
            _constructArcherButton.OnClick -= _clickHandlers[TowerType.Archer];
        }

        public void MoveCanvasTo(Vector2 position)
        {
            LocalCanvas.transform.position = position; 
        }
        
        private void OnConstructButtonClickedHandler(TowerType towerType)
        {
            OnConstructButtonClicked?.Invoke(towerType);
        }
    }
}