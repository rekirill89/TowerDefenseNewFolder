using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts
{
    [CreateAssetMenu(fileName = "WavesSO", menuName = "Scriptable Objects/WavesSO")]
    public class WavesSO : ScriptableObject
    {
        [field:Header("Spawn direction and distance")]
        [field:SerializeField] public float MaxDirX { get; private set; }
        [field:SerializeField] public float MinDirX { get; private set; }
        [field:SerializeField] public float MaxDirY { get; private set; }
        [field:SerializeField] public float MinDirY { get; private set; }
        [field:SerializeField] public float MinSpawnDistance { get; private set; }
        [field:SerializeField] public float MaxSpawnDistance { get; private set; }
        
        [field:Header("Wave params")]
        [field:SerializeField] public float SpawnTimestep { get; private set; }
        [field:SerializeField] public float WaveTimer { get; private set; }
        [field:SerializeField] public float BreakBetweenWavesTimer { get; private set; }
        [field:SerializeField] public float DecreaseSpawnTimestepMultiplier { get; private set; }
        
    }
}