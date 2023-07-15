using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSetting", menuName = "Tower Defense/Level Setting", order = 0)]
public class LevelSettingSO : ScriptableObject
{
    public int levelBuildIndex;
    public int level = 1;
    public bool isUnlocked;

    public Sprite levelPreviewSprite;
    public EnemyWaveData[] enemyDatas;

    [System.Serializable]
    public class EnemyWaveData
    {
        [SerializeField] string waveName;
        public List<Enemy> enemyToSpawnList;
    }

    public bool GetIsUnlocked()
    {
        return isUnlocked == true;
    }

    public void SetIsUnlocked(bool value)
    {
        isUnlocked = value;
    }

}