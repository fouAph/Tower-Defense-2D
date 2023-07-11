using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] LevelSelectItemData selectedLevel;
    [SerializeField] Button startButton;
    [SerializeField] Button LeftArrowMenu;
    [SerializeField] Button RightArrowMenu;

    [SerializeField] int currentLevel = 1; 

    [SerializeField] LevelSelectItemData[] levelSelectItemArray;

    private void Awake()
    {
        LeftArrowMenu.onClick.AddListener(OnLeftArrowButton_Click);
        RightArrowMenu.onClick.AddListener(OnRightArrowButton_Click);
        startButton.onClick.AddListener(OnStartButton_Click);
        ActiveOrDeactiveArrowButton();
        SetSelectedLevel();
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
        if (currentLevel + 1 <= levelSelectItemArray.Length)
        {
            RightArrowMenu.gameObject.SetActive(true);
        }

        if (currentLevel - 1 == 0)
        {
            LeftArrowMenu.gameObject.SetActive(false);
        }

        //For Right Button
        if (currentLevel + 1 > levelSelectItemArray.Length)
        {
            RightArrowMenu.gameObject.SetActive(false);
        }

        if (currentLevel - 1 > 0)
        {
            LeftArrowMenu.gameObject.SetActive(true);
        }
    }

    private void SetSelectedLevel()
    {
        selectedLevel = levelSelectItemArray[currentLevel - 1]; 
    }

    private void LoadLevelScene(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex);
    }
}
