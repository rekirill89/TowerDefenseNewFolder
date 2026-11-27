using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "WavesConfig", menuName = "Configs/WavesConfig")]
    public class WavesConfig : BaseConfig
    {
        [field:Header("Spawn direction and distance")]
        [field:SerializeField] public float MaxDirectionX { get; private set; }
        [field:SerializeField] public float MinDirectionX { get; private set; }
        [field:SerializeField] public float MaxDirectionY { get; private set; }
        [field:SerializeField] public float MinDirectionY { get; private set; }
        [field:SerializeField] public float MinSpawnDistance { get; private set; }
        [field:SerializeField] public float MaxSpawnDistance { get; private set; }
        
        [field:Header("Wave params")]
        [field:SerializeField] public float SpawnInterval { get; private set; }
        [field:SerializeField] public float WaveDuration { get; private set; }
        [field:SerializeField] public float BreakBetweenWaves { get; private set; }
        [field:SerializeField] public float SpawnIntervalDecreaseFactor { get; private set; }
        
    }
}