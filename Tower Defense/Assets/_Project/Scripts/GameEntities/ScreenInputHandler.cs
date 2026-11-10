using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
        
        private void Start()
        {
            _buildableLayerMask = LayerMask.GetMask(BUILDABLE_LAYER_NAME);
            _strongholdLayerMask = LayerMask.GetMask(STRONGHOLD_LAYER_NAME);
            
            _mainCamera = Camera.main;
        }

        private void Update()
        {

            if (Mouse.current.rightButton.wasPressedThisFrame)
                HandleClick();
        }
        
        private void HandleClick()
        {
            if (_mainCamera == null) return;

            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 mouseWorldPos = _mainCamera.ScreenToWorldPoint(mousePos);

            Collider2D strongholdHit = Physics2D.OverlapPoint(mouseWorldPos, _strongholdLayerMask);
            if (strongholdHit != null)
            {
                OnStrongholdClicked?.Invoke();
                return; 
            }

            Collider2D buildableHit = Physics2D.OverlapPoint(mouseWorldPos, _buildableLayerMask);
            if (buildableHit != null)
            {
                OnBuildableClicked?.Invoke(mouseWorldPos);
            }
        }
    }
}