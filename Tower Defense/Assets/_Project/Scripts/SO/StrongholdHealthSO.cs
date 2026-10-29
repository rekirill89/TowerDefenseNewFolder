using UnityEngine;

namespace _Project.Scripts
{
    [CreateAssetMenu(fileName = "StrongholdHealthSO", menuName = "Scriptable Objects/StrongholdHealthSO")]
    public class StrongholdHealthSO : ScriptableObject
    {
        [field: SerializeField] public int MaxHealth { get; set; }
        [field: SerializeField] public float TakeDamageTimer { get; set; }
    }
}