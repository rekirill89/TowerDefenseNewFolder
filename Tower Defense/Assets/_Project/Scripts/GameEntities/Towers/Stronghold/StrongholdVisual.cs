using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.GameEntities
{
    public class StrongholdVisual : MonoBehaviour
    {
        [SerializeField] private Image _image;

        private StrongholdModel _model;

        public void Bind(StrongholdModel model)
        {
            Unbind();
            _model = model;
            _model.OnHealthChanged += OnHealthChangedHandler;
            OnHealthChangedHandler(_model.CurrentHealth, _model.MaxHealth);
        }

        private void OnDestroy() => Unbind();

        private void Unbind()
        {
            if (_model != null)
                _model.OnHealthChanged -= OnHealthChangedHandler;
            _model = null;
        }

        private void OnHealthChangedHandler(int currentHealth, int maxHealth)
        {
            _image.fillAmount = maxHealth > 0 ? Mathf.Clamp01((float)currentHealth / maxHealth) : 0f;
        }
    }
}
