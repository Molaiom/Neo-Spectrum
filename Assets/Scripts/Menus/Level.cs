using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public Button levelButton;
    public Text levelNumberText;
    public Image collectableMarker;
    public Image lockedMarker;   
    public int levelNumber;

    
    private void Start()
    {
        UpdateLevelText();
        CheckIfLevelIsUnlocked();
        CheckLevelCollectable();
    }

    private void OnEnable()
    {
        UpdateLevelText();
        CheckIfLevelIsUnlocked();
        CheckLevelCollectable();
    }

    private void CheckIfLevelIsUnlocked() // TO BE CALLED ON START
    {
        if (GameController.instance != null)
        {   
            // DISABLES THE LEVEL BUTTON HERE IF IT'S LOCKED
            if (levelNumber - 1 > GameController.instance.NumberOfLevelsCompleted || levelNumber == 0)
            {
                levelButton.interactable = false;
                levelNumberText.CrossFadeAlpha(0.3f, 0, true);
                levelNumberText.CrossFadeColor(Color.gray, 0, true, false);                

                lockedMarker.enabled = true;
                collectableMarker.enabled = false;

                lockedMarker.CrossFadeAlpha(0.7f, 0, true);
                lockedMarker.CrossFadeColor(Color.gray, 0, true, false);
            }            
            else
            {                
                lockedMarker.enabled = false;
                collectableMarker.enabled = true;
            }
        }
    }

    private void CheckLevelCollectable() // TO BE CALLED ON START
    {
        if(GameController.instance != null)
        {
            if (levelNumber > 0 && levelNumber <= GameController.instance.CollectableFromLevel.Length)
            {                
                // IF THE PLAYER HAS ALREADY COLLECTED THE LEVEL'S COLLECTABLE
                if (GameController.instance.CollectableFromLevel[levelNumber - 1])
                {
                    collectableMarker.CrossFadeAlpha(0.7f, 0, true);
                    collectableMarker.CrossFadeColor(Color.white, 0, true, false);
                }
                // IF THE PLAYER HASN'T
                else
                {
                    collectableMarker.CrossFadeAlpha(0.3f, 0, true);
                    collectableMarker.CrossFadeColor(Color.gray, 0, true, false);
                }
            }
            else
            {
                collectableMarker.CrossFadeAlpha(0.3f, 0, true);
                collectableMarker.CrossFadeColor(Color.gray, 0, true, false);
            }
        }
    }

    private void UpdateLevelText() // TO BE CALLED ON START
    {
        if(levelNumber < 10)
            levelNumberText.text = "0" + levelNumber.ToString();
        else
            levelNumberText.text = levelNumber.ToString();
    }

    public void LoadLevel() // TO BE CALLED ON CLICK
    {
        string levelName;
        levelName = "Level_" + levelNumber.ToString();
        
        if(Application.CanStreamedLevelBeLoaded(levelName))
        {
            SceneManager.LoadScene(levelName);
        }
        else
        {
            Debug.LogWarning("Invalid Level");
        }
        
    }

}
