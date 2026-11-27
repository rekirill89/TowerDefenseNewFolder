using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace _Project.Scripts.GameEntities
{
    public class ScreenInputHandler : MonoBehaviour
    {
        public event Action<Vector2> OnBuildableClicked;
        public event Action OnStrongholdClicked; 
        
        private const string BUILDABLE_LAYER_NAME = "Buildable";
        private const string STRONGHOLD_LAYER_NAME = "Stronghold";

        private LayerMask _buildableLayerMask;
        private LayerMask _strongholdLayerMask;

        private Camera _mainCamera;
        private InputActions _input;

        [Inject]
        private void Construct(InputActions input) => _input = input;
        
        private void Start()
        {
            _buildableLayerMask = LayerMask.GetMask(BUILDABLE_LAYER_NAME);
            _strongholdLayerMask = LayerMask.GetMask(STRONGHOLD_LAYER_NAME);
            
            _mainCamera = Camera.main;
        }
        
        private void OnEnable() => _input.Gameplay.Enable();

        private void OnDisable() => _input.Gameplay.Disable();

        private void Update()
        {
            if (!_input.Gameplay.Select.WasPerformedThisFrame())
                return;
            
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            HandleClick(_input.Gameplay.PointerPosition.ReadValue<Vector2>());
        }
        
        private void HandleClick(Vector2 screenPosition)
        {
            if (_mainCamera == null)
                return;

            Vector2 worldPosition = _mainCamera.ScreenToWorldPoint(screenPosition);
            
            Collider2D strongholdHit = Physics2D.OverlapPoint(worldPosition, _strongholdLayerMask);
            if (strongholdHit != null)
            {
                OnStrongholdClicked?.Invoke();
                return; 
            }

            Collider2D buildableHit = Physics2D.OverlapPoint(worldPosition, _buildableLayerMask);
            if (buildableHit != null)
            {
                OnBuildableClicked?.Invoke(worldPosition);
            }
        }
    }
}