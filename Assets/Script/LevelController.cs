using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelController : MonoBehaviour
{
    public static LevelController Singleton;
    private void Awake()
    {
        Singleton = this;
    }

    [Header("Save Settings")]
    public string saveName = "save";
    public SaveData saveData;
    private int currentLevelIndex;
    private int nextLevelSettingSOIndex { get { return currentLevelIndex + 1; } }
    [SerializeField] LevelSettingSO[] levelSettingList;

    private void Start()
    {
        LoadGameData();
        DontDestroyOnLoad(gameObject);
    }


    private void LoadGameData()
    {
        SaveData save = SaveData.Current = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/" + saveName + ".save");
        if (save != null)
        {
            saveData = save;
        }
        else
        {
            SaveData.Current = saveData;
            SaveData.Current.levels = new LevelSOData[levelSettingList.Length];
            for (int i = 0; i < levelSettingList.Length; i++)
            {
                //   SaveData.Current.levels[i].levelName = levelSettingList[i].GetLevelName();
                if (i == 0)
                {
                    SaveData.Current.levels[i].isUnlocked = true;
                }
            }
            SerializationManager.Save(saveName, SaveData.Current);
            print("creating new save");
        }

        for (int i = 0; i < saveData.levels.Length; i++)
        {
            // levelSettingList[i].SetStarScore(saveData.levels[i].starScore);
            levelSettingList[i].SetIsUnlocked(saveData.levels[i].isUnlocked);
        }
    }

    private void SaveLevelData()
    {
        if (levelSettingList.Length > nextLevelSettingSOIndex)
            SaveData.Current.levels[nextLevelSettingSOIndex].isUnlocked = levelSettingList[nextLevelSettingSOIndex].GetIsUnlocked();
        SerializationManager.Save(saveName, SaveData.Current);

        print("Level saved");
    }

    public void UnlockNextLevel()
    {
        if (levelSettingList.Length > nextLevelSettingSOIndex)
        {
            //Unlock next level on levelSettingList
            levelSettingList[nextLevelSettingSOIndex].SetIsUnlocked(true);

            SaveLevelData();
        }
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(levelSettingList[currentLevelIndex].levelBuildIndex);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(levelSettingList[nextLevelSettingSOIndex].levelBuildIndex);
    }

    public void LoadMainMenu()
    {

        SceneManager.LoadScene(0);
    }
}