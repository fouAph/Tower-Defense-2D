using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;
    public GameState gameState = GameState.Menu;
    [SerializeField]
    private int coin = 500;
    public int Coin
    {
        get { return coin; }
        private set { coin = value; }
    }
    // [SerializeField] List<LevelInfoDataSO> levelInfoDataList = new List<LevelInfoDataSO>();
    public LevelInfoDataSO currentLevelInfo;
    public int currentWave = 1;

    public event EventHandler OnWaveCompleted;

    public List<Enemy> enemiesAlive = new List<Enemy>();

    [SerializeField] CountDownHelper countDownHelper;
    private const int COUNTDOWN_TIMER = 3;

    public float CountdownTimer;
    float startCountdown;

    private void Awake()
    {
        Singleton = this;
    }

    [SerializeField] GameObject selectedTowerGO;

    private void Start()
    {
        OnWaveCompleted += GameManager_OnWaveCompleted;
        LevelSetup();
        StartCoroutine(StartGameCountdown());
    }

    private void Update()
    {
        if (gameState != GameState.InGame) return;

        if (selectedTowerGO)
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector3 pos = CodeMonkey.Utils.UtilsClass.GetMouseWorldPosition();
                TDGridNode gridobj = TowerDefenseGrid.grid.GetGridObject(pos);
                // selectedGrid = gridobj;
                if (gridobj == null) return;

                if (gridobj.GetOccupied())
                {
                    CodeMonkey.CMDebug.TextPopupMouse("Cannot Place Tower Here");
                    return;
                }

                SpawnTower();
                gridobj.SetOccupied(true);
                gridobj.TriggerGridObjectChanged();
            }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            UIManager.Singleton.ClearSelected();
            selectedTowerGO = null;
        }

    }

    private void LevelSetup()
    {
        EnemyWaveSpawner.Singleton.AddEnemyToSpawnList(currentLevelInfo.enemyDatas[currentWave - 1].enemyToSpawnList);
        countDownHelper.gameObject.SetActive(true);
        UIManager.Singleton.LevelSetup();
    }

    IEnumerator StartGameCountdown()
    {
        gameState = GameState.Menu;
        countDownHelper.SetSprite(countDownHelper._3);
        yield return new WaitForSeconds(1);
        countDownHelper.gameObject.SetActive(true);
        startCountdown = CountdownTimer;
        // AudioPoolSystem.Singleton.PlayShootAudio(countDownHelper.countdownClip);
        // MusicManager.Singleton.PlayInGameMusic();
        while (startCountdown >= 0)
        {
            startCountdown -= Time.deltaTime;
            if (startCountdown <= 2 && startCountdown >= 1)
                countDownHelper.SetSprite(countDownHelper._2);
            else if (startCountdown <= 1 && startCountdown >= 0)
                countDownHelper.SetSprite(countDownHelper._1);
            else if (startCountdown <= 0)
            {
                countDownHelper.SetSprite(countDownHelper._go);
                // playerManager.EnablePlayerController();
            }
            yield return null;
        }
        yield return new WaitForSeconds(.2f);
        countDownHelper.gameObject.SetActive(false);
        gameState = GameState.InGame;
    }

    public void SpawnTower()
    {
        TowerDefenseGrid.SpawnTower();
    }

    public GameObject GetSelectedTowerGO()
    {
        return selectedTowerGO;
    }

    public void SetSelectedTower(GameObject towerPrefab)
    {
        selectedTowerGO = towerPrefab;
    }

    public bool CheckIfNoEnemyLeft()
    {
        return enemiesAlive.Count == 0;
    }

    public void OnWaveCompleted_Invoke()
    {
        OnWaveCompleted?.Invoke(this, EventArgs.Empty);

    }

    private void GameManager_OnWaveCompleted(object sender, EventArgs e)
    {
        if (currentWave != currentLevelInfo.enemyDatas.Length)
        {
            currentWave++;
            EnemyWaveSpawner.Singleton.AddEnemyToSpawnList(currentLevelInfo.enemyDatas[currentWave - 1].enemyToSpawnList);

            StartCoroutine(StartGameCountdown());
        }

        UIManager.Singleton.LevelSetup();
    }

}
public enum GameState { Menu, InGame, GameOver, NotReady, InShop }
