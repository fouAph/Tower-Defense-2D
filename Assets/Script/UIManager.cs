using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Singleton;
    private void Awake()
    {
        Singleton = this;
    }

    public GameObject gameOverPanel;

    [SerializeField] GameObject levelComplete_Group;
    [SerializeField] GameObject levelFail_Group;

    [SerializeField] Button mainMenuButton;
    [SerializeField] Button nextLevelButton;
    [SerializeField] Button retryButton;

    [SerializeField] TMP_Text coin_TMP;
    [SerializeField] TMP_Text levelInfo_TMP;
    [SerializeField] TMP_Text waveLevel_TMP;
    [SerializeField] TMP_Text waveClear_TMP;

    private TowerItemUI selectedTowerItem;

    private void Start()
    {
        GameManager.Singleton.OnWaveCompleted += UIManager_OnWaveCompleted;
        UpdateCoinUI();
        nextLevelButton.onClick.AddListener(() => LevelController.Singleton.LoadNextLevel());
        retryButton.onClick.AddListener(() => LevelController.Singleton.ReloadLevel());
        mainMenuButton.onClick.AddListener(() => LevelController.Singleton.LoadMainMenu());


        GameManager.Singleton.OnLevelComplete += GameManage_OnLevelCompleted;
        GameManager.Singleton.OnLevelFailed += GameManage_OnLevelFailed;
        HideGameOverPanel();
    }

    private void Update()
    {
        if (selectedTowerItem)
        {
            Vector3 spawnPosition = UtilsClass.GetMouseWorldPosition();
            spawnPosition = TowerDefenseGrid.ValidateWorldGridPosition(spawnPosition);
            spawnPosition += new Vector3(1, 1, 0) * TowerDefenseGrid.grid.GetCellSize() * .5f;

            selectedTowerItem.SetTowerPreviewPosition(spawnPosition);

            if (TowerDefenseGrid.grid.GetGridObject(spawnPosition) == null
            || TowerDefenseGrid.grid.GetGridObject(spawnPosition).GetOccupied() == true)
            {
                selectedTowerItem.HideTowerPreview();
                TowerUpgrade.Singleton.HideSensorOverlay();
            }
            else
            {
                selectedTowerItem.ShowTowerPreview();
                if (selectedTowerItem.towerPreviewPrefab != null)
                    TowerUpgrade.Singleton.ShowSensorOverlayForPreview(selectedTowerItem);
            }
        }
    }

    public void GameManage_OnLevelCompleted(object sender, EventArgs e)
    {
        ShowLevelComplete();
    }

    public void GameManage_OnLevelFailed(object sender, EventArgs e)
    {
        ShowLevelFail();
    }
    public void LevelSetup()
    {
        var gm = GameManager.Singleton;
        levelInfo_TMP.text = "Level: " + gm.currentLevelSetting.level.ToString();
        waveLevel_TMP.text = "Wave: " + gm.currentWave.ToString();
    }

    public void UpdateCoinUI()
    {
        coin_TMP.text = "$" + GameManager.Singleton.GetCoin();
    }

    public void SetSelectedTowerItem(TowerItemUI newTowerItemUI)
    {
        ClearSelected();
        selectedTowerItem = newTowerItemUI;
    }

    public void ClearSelected()
    {
        if (selectedTowerItem)
            selectedTowerItem.Deselected();

        TowerUpgrade.Singleton.HideSensorOverlay();
        selectedTowerItem = null;
    }

    private void WaveClearPopup()
    {
        var gm = GameManager.Singleton;
        string wvClearText = waveClear_TMP.text;
        waveClear_TMP.text = gm.currentWave != gm.GetMaxWave() ? wvClearText : "Level Compeleted";
        float startX = waveClear_TMP.transform.position.x;
        float toX = -333f;
        float endX = 1155f;
        LeanTween.moveLocalX(waveClear_TMP.gameObject, toX, .5f)
        .setOnComplete(() => LeanTween.moveLocalX(waveClear_TMP.gameObject, endX, .5f).setDelay(1f));
    }

    private void UIManager_OnWaveCompleted(object sender, EventArgs e)
    {
        WaveClearPopup();
    }

    public void ShowLevelComplete()
    {
        gameOverPanel.SetActive(true);
        levelComplete_Group.SetActive(true);
    }

    public void ShowLevelFail()
    {
        gameOverPanel.SetActive(true);
        levelFail_Group.SetActive(true);
    }

    public void HideGameOverPanel()
    {
        gameOverPanel.SetActive(false);
        levelComplete_Group.SetActive(false);
        levelFail_Group.SetActive(false);
    }
}
