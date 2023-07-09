using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWaveSO", menuName = "Tower Defense/EnemyWaveSO", order = 0)]
public class EnemyWaveLevelSO : ScriptableObject
{
    [SerializeField] int level;
    public EnemyWaveData[] enemyDatas;

    [System.Serializable]
    public class EnemyWaveData
    {
        [SerializeField] int waveLevel = 1;
        public List<GameObject> enemyObject;
    }
}
