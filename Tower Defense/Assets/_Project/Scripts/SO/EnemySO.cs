using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    [field: SerializeField] public int MaxHealth { get; set; }
    [field: SerializeField] public int Damage { get; set; }
    [field: SerializeField] public float AttackRate { get; set; }
    [field: SerializeField] public float Speed { get; set; }
}
