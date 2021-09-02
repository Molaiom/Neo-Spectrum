using UnityEngine;
using UnityEngine.UI;

public class LevelSelectScreen : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public GameObject[] levelsPanel;
    public Color[] backgroundColor;
    public Image backgroundImage;
    
    private void Start() 
    {
        CreateLevelButtons();
        SetInitialScreen();
    }

    private void CreateLevelButtons()
    {
        if(GameController.instance != null)
        {
            GameController gc = GameController.instance;

            for (int i = 1; i < gc.GetLevelCount() + 1; i++)
            {
                int levelArea = 0;
                for (int j = 0; j < gc.GetLastLevelOfArea().Length; j++)
                {
                    if(i <= gc.GetLastLevelOfArea()[j])
                    {
                        levelArea = j;
                        break;
                    }
                }
                GameObject levelObj = Instantiate(levelButtonPrefab, levelsPanel[levelArea].transform.GetChild(0), false);
                levelObj.GetComponent<Level>().Initialize(i);
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

    public void ChangeToScreen(int index) // SHOWS ONLY THE SELECTED LEVEL SCREEN / TO BE CALLED ON START AND ON BUTTONS
    {
        for (int i = 0; i < levelsPanel.Length; i++)
        {
            if (i == index)
            {
                levelsPanel[i].SetActive(true);
                backgroundImage.CrossFadeColor(backgroundColor[i], 0.25f, false, false);
            }                
            else
            {
                levelsPanel[i].SetActive(false);
            }                
        }
    }
}
