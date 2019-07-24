using UnityEngine;
using UnityEngine.UI;

public class LevelSelectScreen : MonoBehaviour
{
    public GameObject[] levelsPanel;
    public Color[] backgroundColor;
    public Image backgroundImage;    
    
    private void Start() // SHOWS THE LAST LEVEL'S UNLOCKED SCREEN WHEN SCENE OPENS
    {
        if(GameController.instance != null)
        {
            if(GameController.instance.NumberOfLevelsCompleted < 5)
            {
                ChangeToScreen(0);
            }
            else if(GameController.instance.NumberOfLevelsCompleted < 12)
            {
                ChangeToScreen(1);
            }
            else if(GameController.instance.NumberOfLevelsCompleted < 20)
            {
                ChangeToScreen(2);
            }
            else
            {
                ChangeToScreen(3);
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
