using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class LevelSelectScreen : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public GameObject[] levelsPanel;
    public Color[] backgroundColor;
    public Image backgroundImage;
    public Button backButton;
    public GameObject[] previousButtons;
    public GameObject[] nextButtons;

    private Button[] levelButtonsList;

    private void Start()
    {
        CreateLevelButtons();
        SetInitialScreen();
    }

    private void Update()
    {
        // IF BACK BUTTON IS PRESSED
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (GameController.instance != null)
                GameController.instance.LoadMainMenu();
        }
    }

    private void CreateLevelButtons()
    {
        if (GameController.instance != null)
        {
            GameController gc = GameController.instance;
            levelButtonsList = new Button[gc.GetLevelCount()];

            for (int i = 1; i < gc.GetLevelCount() + 1; i++)
            {
                int levelArea = 0;
                for (int j = 0; j < gc.GetLastLevelOfArea().Length; j++)
                {
                    if (i <= gc.GetLastLevelOfArea()[j])
                    {
                        levelArea = j;
                        break;
                    }
                }
                GameObject levelObj = Instantiate(levelButtonPrefab, levelsPanel[levelArea].transform.GetChild(0), false);
                levelObj.GetComponent<Level>().Initialize(i);
                levelButtonsList[i - 1] = levelObj.GetComponent<Button>();
            }
        }
    }

    private void SetInitialScreen() // SHOWS THE LAST LEVEL'S UNLOCKED SCREEN WHEN SCENE OPENS
    {
        if (GameController.instance != null)
        {
            GameController gc = GameController.instance;

            for (int i = 0; i < gc.GetLastLevelOfArea().Length; i++)
            {
                if (gc.NumberOfLevelsCompleted < gc.GetLastLevelOfArea()[i])
                {
                    ChangeToScreen(i);
                    break;
                }
                else if (gc.NumberOfLevelsCompleted == gc.GetLevelCount())
                    ChangeToScreen(0);
            }
        }
        else
        {
            ChangeToScreen(0);
            Debug.LogWarning("Game Controller instance could not be found!");
        }
    }

    public void ChangeToScreen(int index) // SHOWS ONLY THE SELECTED LEVEL AREA / TO BE CALLED ON START AND ON BUTTONS
    {
        for (int i = 0; i < levelsPanel.Length; i++)
        {
            if (i == index)
            {
                levelsPanel[i].SetActive(true);
                backgroundImage.CrossFadeColor(backgroundColor[i], 0.25f, false, false);

                SelectLevelOnAreaChange(index);
                SetBackButtonNavigation(index);
            }
            else
            {
                levelsPanel[i].SetActive(false);
            }
        }
    }

    #region MenuNavigation
    private void SelectLevelButton(int buttonIndex)
    {
        if (buttonIndex < levelButtonsList.Length && buttonIndex >= 0)
        {
            if (levelButtonsList[buttonIndex].IsActive() && levelButtonsList[buttonIndex].interactable)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(levelButtonsList[buttonIndex].gameObject);
                //print(EventSystem.current.currentSelectedGameObject.GetComponent<Level>().levelNumber);
            }
        }
    }

    private void SelectLevelOnAreaChange(int areaIndex)
    {
        // IF THE SCENE HAS JUST STARTED
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            int buttonIndex = GameController.instance.NumberOfLevelsCompleted;
            SelectLevelButton(buttonIndex >= levelButtonsList.Length ? 0 : buttonIndex);
        }
        // IF THE LEVEL AREA SCREEN CHANGES
        else
        {
            List<Button> activeButtonsOnScreen = new List<Button>();

            for (int i = 0; i < levelButtonsList.Length; i++)
            {
                if (levelButtonsList[i].IsInteractable() && i <= LastLevelIndexOfArea(areaIndex) && i >= FirstLevelIndexOfArea(areaIndex))
                {
                    activeButtonsOnScreen.Add(levelButtonsList[i]);
                }
            }

            if(activeButtonsOnScreen.Count > 0)
            {
                if (EventSystem.current.currentSelectedGameObject.name == "Previous")
                {
                    SelectLevelButton(activeButtonsOnScreen[activeButtonsOnScreen.Count - 1].GetComponent<Level>().levelNumber - 1);
                }
                else
                {
                    SelectLevelButton(activeButtonsOnScreen[0].GetComponent<Level>().levelNumber - 1);
                }
            }
            else
            {
                // IF THE PREVIOUSLY SELECTED OBJECT IS A PREVIOUS BUTTON
                if(EventSystem.current.currentSelectedGameObject.name == "Previous")
                {
                    for (int i = 0; i < nextButtons.Length; i++)
                    {
                        if (nextButtons[i].transform.parent.gameObject.activeSelf)
                        {
                            EventSystem.current.SetSelectedGameObject(null);
                            EventSystem.current.SetSelectedGameObject(nextButtons[i]);
                            break;
                        }
                    }
                }
                // IF THE PREVIOUSLY SELECTED OBJECT IS A NEXT BUTTON
                else if (EventSystem.current.currentSelectedGameObject.name == "Next")
                {
                    for (int i = 0; i < previousButtons.Length; i++)
                    {
                        if (previousButtons[i].transform.parent.gameObject.activeSelf)
                        {
                            EventSystem.current.SetSelectedGameObject(null);
                            EventSystem.current.SetSelectedGameObject(previousButtons[i]);
                            break;
                        }
                    }
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(backButton.gameObject);
                }
            }
        }
    }

    private void SetBackButtonNavigation(int areaIndex)
    {
        Navigation backButtonNavigation = backButton.navigation;
        backButtonNavigation.selectOnUp = levelButtonsList[LastLevelIndexOfArea(areaIndex) - 4];
        backButton.navigation = backButtonNavigation;
    }

    private int FirstLevelIndexOfArea(int areaIndex)
    {
        return areaIndex == 0 ? 0 : GameController.instance.GetLastLevelOfArea()[areaIndex - 1];
    }

    private int LastLevelIndexOfArea(int areaIndex)
    {
        //print(GameController.instance.GetLastLevelOfArea()[areaIndex] - 4);
        return GameController.instance.GetLastLevelOfArea()[areaIndex] - 1;
    }
    #endregion
}