using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;
    public GameState gameState = GameState.Menu;
    [SerializeField]
    private int coin = 500;

    // [SerializeField] List<LevelInfoDataSO> levelInfoDataList = new List<LevelInfoDataSO>();
    public LevelSettingSO levelSetting;
    [SerializeField] float maxHealth = 5;
    [SerializeField] CountDownHelper countDownHelper;
    public event EventHandler OnWaveCompleted;

    [NonSerialized] public int currentWave = 1;
    [NonSerialized] public List<Enemy> enemiesAlive = new List<Enemy>();
    private const int COUNTDOWN_TIMER = 3;

    [SerializeField] Slider healthBar;
    private Tower selectedTower;
    private float CountdownTimer = 3;
    private float startCountdown;
    private float currentHealth;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        OnWaveCompleted += GameManager_OnWaveCompleted;
        LevelSetup();
        StartCoroutine(StartGameCountdown());
    }

    private void Update()
    {
        if (gameState != GameState.InGame) return;

        if (selectedTower)
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

                if (!selectedTower.CanBuyTower())
                {
                    CodeMonkey.CMDebug.TextPopupMouse("Not Enough Coin");
                    return;
                }

                SpawnTower();

                gridobj.SetOccupied(true);
                gridobj.TriggerGridObjectChanged();
                SubstractCoin(selectedTower.towerStatsSO.towerPrice);
                UIManager.Singleton.UpdateCoinUI();
            }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            UIManager.Singleton.ClearSelected();
            selectedTower = null;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnDamaged(1);
        }
    }

    private void LevelSetup()
    {
        currentHealth = maxHealth;
        EnemyWaveSpawner.Singleton.AddEnemyToSpawnList(levelSetting.enemyDatas[currentWave - 1].enemyToSpawnList);
        countDownHelper.gameObject.SetActive(true);
        UIManager.Singleton.LevelSetup();
    }

    IEnumerator StartGameCountdown()
    {
        gameState = GameState.Menu;
        countDownHelper.SetSprite(countDownHelper._3);
        // yield return new WaitForSeconds(1);
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
    #region Getter And Setter
    public Tower GetSelectedTowerGO()
    {
        return selectedTower;
    }

    public int GetCoin()
    {
        return coin;
    }

    public void AddCoin(int value)
    {
        coin += value;
    }

    public void SubstractCoin(int value)
    {
        coin -= value;
    }

    public void SetSelectedTower(Tower towerPrefab)
    {
        selectedTower = towerPrefab;
    }
    #endregion

    public bool CanBuy(int price)
    {
        return coin >= price;
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
        if (currentWave != levelSetting.enemyDatas.Length)
        {
            currentWave++;
            EnemyWaveSpawner.Singleton.AddEnemyToSpawnList(levelSetting.enemyDatas[currentWave - 1].enemyToSpawnList);

            StartCoroutine(StartGameCountdown());
        }

        UIManager.Singleton.LevelSetup();
    }

    public void OnDamaged(int damage)
    {
        currentHealth -= damage;
        // health = Mathf.Clamp(health, health, 0);
        UpdateHealth();
        // PoolSystem.Singleton.SpawnFromPool(hitVFXPrefab, transform.position, Quaternion.identity, transform);
        if (currentHealth <= 0)
        {
            //TODO Spawn GameOver Panel
        }
    }

    private void UpdateHealth()
    {
        float healthPercentage = (float)currentHealth / maxHealth;
        // float healthBarSize = healthBar. * healthPercentage;
        print(healthPercentage);
        healthBar.value = (float)healthPercentage;
    }
}
public enum GameState { Menu, InGame, GameOver, NotReady, InShop }
