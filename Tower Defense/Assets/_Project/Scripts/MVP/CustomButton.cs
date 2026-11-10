using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.MVP
{
    [RequireComponent(typeof(Button))]
    public class CustomButton : MonoBehaviour
    {
        public event Action OnClick;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClickHandler);
            SetInteractable(true);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void SetInteractable(bool isInteractable)
        {
            _button.interactable = isInteractable;
        }
        
        private void OnClickHandler()
        {
            OnClick?.Invoke();
        }
    }
}