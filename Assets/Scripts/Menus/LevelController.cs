using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// THIS CLASS HANDLES END OF LEVEL INTERACTIONS (SAVES), PLAYER DEATH (DEATH SCREEN) AND HUD INTERACTIONS
public class LevelController : MonoBehaviour
{
    // EXTERNAL ATTRIBUTES
    [SerializeField]
    private GameObject deathScreen;
    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private LevelCompletedScreen levelCompletedScreen;
    [SerializeField]
    private Text pauseLevelNumberText;
    [SerializeField]
    private Text hudLevelNumberText;

    // INTERNAL ATTRIBUTES
    public static LevelController instance;
    private int playerCount;
    private bool levelCompleted;

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
        if (FindObjectsOfType<PlayerController>() != null)
        {
            playerCount = FindObjectsOfType<PlayerController>().Length;
        }

        AssignLevelNumberText();
        levelCompleted = false;
    }

    private void FixedUpdate() // CHECKS FOR LEVEL COMPLETION/FAIL
    {
        if (playerCount > 0)
        {
            FailLevel();
            CompleteLevel();
        }
    }

    private void Update() // PAUSES THE LEVEL WITH THE ESCAPE BUTTON
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !levelCompleted)
        {
            if (GameController.instance != null)
            {
                if (pauseScreen.activeSelf)
                {
                    pauseScreen.SetActive(false);
                    GameController.instance.ChangePauseState(false);
                }
                else
                {
                    pauseScreen.SetActive(true);
                    GameController.instance.ChangePauseState(true);
                }
            }
        }
    }

    private void AssignLevelNumberText()
    {
        if (GameController.instance != null)
        {
            string levelText = "Level " + GameController.instance.GetLevelNumber().ToString();
            pauseLevelNumberText.text = levelText;

            if (hudLevelNumberText.IsActive())
                hudLevelNumberText.text = levelText;

        }
    }

    public void FailLevel() // SHOWS THE DEATH SCREEN
    {
        if (playerDeathCount < playerCount)
            return;

        deathScreen.SetActive(true);
        StartCoroutine(AutoRestart());
    }

    private IEnumerator AutoRestart() // AUTO RESTART THE LEVEL AFTER THE PLAYER DIES
    {
        yield return new WaitForSeconds(3);

        if (GameController.instance != null) GameController.instance.RestartScene();
    }

    public void CompleteLevel() // SHOWS THE LEVEL COMPLETED SCREEN AND SAVES PROGRESS 
    {
        if (playerCompletedCount >= playerCount && !levelCompleted)
        {
            levelCompletedScreen.OpenLevelCompletedMenu();
            levelCompleted = true;

            if (GameController.instance != null)
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
}
