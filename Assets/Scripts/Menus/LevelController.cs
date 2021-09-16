using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// THIS CLASS HANDLES END OF LEVEL INTERACTIONS (SAVES) AND PLAYER DEATH (DEATH SCREEN)
public class LevelController : MonoBehaviour
{
    // EXTERNAL ATTRIBUTES
    [SerializeField]
    private GameObject deathText;
    [SerializeField]
    private LevelCompletedScreen levelCompletedScreen;
    [SerializeField]
    private Text levelNumberText;

    // INTERNAL ATTRIBUTES
    public static LevelController instance;
    private int playerCount;
    [HideInInspector]
    public int playerCompletedCount;
    [HideInInspector]
    public int playerDeathCount;
    [HideInInspector]
    public bool collectable;

    private void Awake() // ASSIGN SINGLETON
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start() // ASSIGN PLAYER COUNT
    {
        if(FindObjectsOfType<PlayerController>() != null)
        {
            playerCount = FindObjectsOfType<PlayerController>().Length;
        }

        AssignLevelNumberText();
    }

    private void FixedUpdate()
    {
        FailLevel();
        CompleteLevel();
    }

    private void AssignLevelNumberText()
    {
        if (GameController.instance != null && levelNumberText != null)
        {
            if (GameController.instance.GetLevelNumber() != 0)
            {
                levelNumberText.text = "Level " + GameController.instance.GetLevelNumber().ToString();
            }
        }
    }

    public void FailLevel() // SHOWS THE DEATH SCREEN
    {
        if (playerDeathCount < playerCount)
            return;

        deathText.SetActive(true);
    }

    public void CompleteLevel() // SHOWS THE LEVEL COMPLETED SCREEN AND SAVES PROGRESS 
    {
        if (playerCompletedCount < playerCount)
            return;

        levelCompletedScreen.OpenLevelCompletedMenu();

        if(GameController.instance != null)
        {
            // SAVES THE LEVEL COMPLETION 
            int levelNumber = GameController.instance.GetLevelNumber();
            if (levelNumber > 0)
            {
                if (levelNumber > GameController.instance.NumberOfLevelsCompleted)
                {
                    GameController.instance.NumberOfLevelsCompleted = levelNumber;
                }

                if (collectable)
                {
                    // DISPLAYS THE "NEW EXTRA UNLOCKED" TEXT, IF THE PLAYERS UNLOCKED ONE
                    for (int i = 0; i < GameController.instance.GetCollectablesRequiredForExtra().Length; i++)
                    {
                        if (GameController.instance.NumberOfCollectablesCollected ==
                            GameController.instance.GetCollectablesRequiredForExtra()[i] - 1)
                        {
                            levelCompletedScreen.ShowNewExtraUnlockedText();
                        }
                    }

                    // SAVES THE COLLECTABLE
                    bool[] newArray = GameController.instance.CollectableFromLevel;
                    newArray[levelNumber - 1] = true;
                    GameController.instance.CollectableFromLevel = newArray;
                }
            }
        }
        else
            Debug.LogWarning("No GameController instance found in the scene!");
    }
}
