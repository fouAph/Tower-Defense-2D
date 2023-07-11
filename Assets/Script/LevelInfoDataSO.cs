using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfoDataSO", menuName = "Tower Defense/LevelInfoData", order = 0)]
public class LevelInfoDataSO : ScriptableObject
{
    public int level;
    public EnemyWaveData[] enemyDatas;

    [System.Serializable]
    public class EnemyWaveData
    {
        [SerializeField] int waveLevel = 1;
        public List<Enemy> enemyToSpawnList;
    }
}