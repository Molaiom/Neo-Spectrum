using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    // END OF LEVEL IMPLEMENTATIONS (HUD / COLLECTABLE / SAVE)
    // PLAYER DEATH IMPLEMENTATIONS (HUD)
    // EXTERNAL ATTRIBUTES
    [SerializeField]
    private GameObject deathText;
    [SerializeField]
    private LevelCompletedScreen levelCompletedScreen;
    private int playerCount;

    // INTERNAL ATTRIBUTES
    public static LevelController instance;
    public int playerCompletedCount;
    public int playerDeathCount;
    public bool collectable;

    private void Awake() // ASSIGN SINGLETON
    {
        instance = this;
    }

    private void Start() // ASSIGN PLAYER COUNT
    {
        if(FindObjectsOfType<PlayerController>() != null) playerCount = FindObjectsOfType<PlayerController>().Length;
    }

    private void FixedUpdate()
    {
        FailLevel();
        CompleteLevel();
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
