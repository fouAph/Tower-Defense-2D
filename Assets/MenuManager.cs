using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] LevelSettingSO selectedLevel;

    [SerializeField] Button startButton;
    [SerializeField] Button leftArrowMenu;
    [SerializeField] Button rightArrowMenu;
    [SerializeField] Button playButton;

    [SerializeField] LevelSettingSO[] levelSettingsArray;
    [SerializeField] Image levelPreviewImage;
    [SerializeField] GameObject lockPanel;

    [SerializeField] Menu[] menus;
    [SerializeField] string currentMenu;
    [SerializeField] string previousMenu;

    [SerializeField] AudioClip selectMenuClip;
    [SerializeField] AudioClip exitSelectMenuClip;
    [SerializeField] AudioClip clickMenuClip;

    private int currentLevel = 1;


    private void Awake()
    {
        leftArrowMenu.onClick.AddListener(OnLeftArrowButton_Click);
        rightArrowMenu.onClick.AddListener(OnRightArrowButton_Click);
        startButton.onClick.AddListener(OnStartButton_Click);

        playButton.onClick.AddListener(() => startButton.gameObject.SetActive(selectedLevel.GetIsUnlocked()));
        ActiveOrDeactiveArrowButton();
        SetSelectedLevel();
    }
    private void Start()
    {
        OpenMenu("MainMenu"); 
    }

    public void OnStartButton_Click()
    {
        LoadLevelScene(selectedLevel.levelBuildIndex);
    }

    public void OnLeftArrowButton_Click()
    {
        currentLevel--;

        ActiveOrDeactiveArrowButton();
        SetSelectedLevel();
    }

    public void OnRightArrowButton_Click()
    {
        currentLevel++;
        ActiveOrDeactiveArrowButton();

        SetSelectedLevel();
    }

    private void ActiveOrDeactiveArrowButton()
    {
        //For Left Button
        if (currentLevel + 1 <= levelSettingsArray.Length)
        {
            rightArrowMenu.gameObject.SetActive(true);
        }

        if (currentLevel - 1 == 0)
        {
            leftArrowMenu.gameObject.SetActive(false);
        }

        //For Right Button
        if (currentLevel + 1 > levelSettingsArray.Length)
        {
            rightArrowMenu.gameObject.SetActive(false);
        }

        if (currentLevel - 1 > 0)
        {
            leftArrowMenu.gameObject.SetActive(true);
        }
    }

    private void SetSelectedLevel()
    {
        selectedLevel = levelSettingsArray[currentLevel - 1];
        levelPreviewImage.sprite = selectedLevel.levelPreviewSprite;

        if (selectedLevel.GetIsUnlocked() == false)
            lockPanel.SetActive(true);
        else
            lockPanel.SetActive(false);

        startButton.gameObject.SetActive(selectedLevel.GetIsUnlocked());
    }

    private void LoadLevelScene(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex);
    }

    public void OnSelectMenu()
    {
        // if (audioPoolSystem && selectMenuClip)
        {
            // audioPoolSystem.PlayAudioMenu(selectMenuClip, 1f);
        }
    }

    public void OnExitSelectMenu()
    {
        // if (audioPoolSystem && exitSelectMenuClip)
        {
            // audioPoolSystem.PlayAudioMenu(exitSelectMenuClip, 1f);
        }
    }

    public void OnClickMenu()
    {
        // if (audioPoolSystem && clickMenuClip)
        {
            // audioPoolSystem.PlayAudioMenu(clickMenuClip, 1f);
        }
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                previousMenu = currentMenu;
                menus[i].Open();
                currentMenu = menus[i].menuName;
            }
            else
            {
                menus[i].Close();
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menu.menuName)
            {
                previousMenu = currentMenu;
                menus[i].Open();
                currentMenu = menus[i].menuName;
            }
            else
            {
                menus[i].Close();
            }
        }
    }

    public void OnBackButton_Click()
    {
        OpenMenu(previousMenu);
    }
}
