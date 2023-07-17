using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;
    public GameState gameState = GameState.Menu;
    [SerializeField] int startingCoin = 500;
    private int coin;

    // [SerializeField] List<LevelInfoDataSO> levelInfoDataList = new List<LevelInfoDataSO>();
    public LevelSettingSO currentLevelSetting;
    [SerializeField] float maxHealth = 5;
    [SerializeField] CountDownHelper countDownHelper;

    public event EventHandler OnWaveCompleted;

    public int currentWave = 1;
    public int nextWave { get { return currentWave - 1; } }
    [NonSerialized] public List<Enemy> enemiesAlive = new List<Enemy>();
    private const int COUNTDOWN_TIMER = 3;

    [SerializeField] Slider healthBar;
    private Tower selectedTower;
    private float CountdownTimer = 3;
    private float startCountdown;
    private float currentHealth;

    public event EventHandler OnLevelComplete;
    public event EventHandler OnLevelFailed;
    public event EventHandler OnCoinChanged;
    private int maxWave;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        coin = startingCoin;
        OnWaveCompleted += GameManager_OnWaveCompleted;
        OnLevelComplete += GameManager_OnLevelComplete;
        OnLevelFailed += GameManager_OnLevelFailed;
        OnCoinChanged += GameManager_OnCoinChanged;
        LevelSetup();
        StartCoroutine(StartGameCountdown());

        maxWave = currentLevelSetting.enemyDatas.Length;
        OnCoinChanged?.Invoke(this, EventArgs.Empty);
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
                if (!CheckIfEnoughCoinForPrice(selectedTower.towerStatsSO.towerPrice))
                {
                    UIManager.Singleton.ClearSelected();
                    selectedTower = null;
                }
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
        EnemyWaveSpawner.Singleton.AddEnemyToSpawnList(currentLevelSetting.enemyDatas[nextWave].enemyToSpawnList);
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

    public int GetMaxWave()
    {
        return maxWave;
    }

    public void AddCoin(int value)
    {
        coin += value;
        OnCoinChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SubstractCoin(int value)
    {
        coin -= value;
        OnCoinChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedTower(Tower towerPrefab)
    {
        selectedTower = towerPrefab;
    }
    #endregion

    public bool CheckIfNoEnemyLeft()
    {
        return enemiesAlive.Count == 0;
    }

    public void OnWaveCompleted_Invoke() => OnWaveCompleted?.Invoke(this, EventArgs.Empty);

    private void GameManager_OnWaveCompleted(object sender, EventArgs e)
    {
        if (currentWave != maxWave)
        {
            currentWave++;
            EnemyWaveSpawner.Singleton.AddEnemyToSpawnList(currentLevelSetting.enemyDatas[currentWave - 1].enemyToSpawnList);

            StartCoroutine(StartGameCountdown());
        }
        else
            OnLevelComplete?.Invoke(this, EventArgs.Empty);


        UIManager.Singleton.LevelSetup();
    }

    private void GameManager_OnCoinChanged(object sender, EventArgs e)
    {

    }

    public void OnDamaged(int damage)
    {
        currentHealth -= damage;
        // health = Mathf.Clamp(health, health, 0);
        UpdateHealthBarUI();
        // PoolSystem.Singleton.SpawnFromPool(hitVFXPrefab, transform.position, Quaternion.identity, transform);
        if (currentHealth <= 0)
        {
            OnLevelFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    public void GameManager_OnLevelComplete(object sender, EventArgs e)
    {
        gameState = GameState.GameOver;
    }

    public void GameManager_OnLevelFailed(object sender, EventArgs e)
    {
        gameState = GameState.GameOver;
    }

    private void UpdateHealthBarUI()
    {
        float healthPercentage = (float)currentHealth / maxHealth;
        healthBar.value = (float)healthPercentage;
    }

    public bool CheckIfEnoughCoinForPrice(int price)
    {
        return coin >= price;
    }
}
public enum GameState { Menu, InGame, GameOver }
