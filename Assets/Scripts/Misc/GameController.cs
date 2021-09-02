using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    #region Attributes
    // DATA TO BE SAVED
    private int numberOfLevelsCompleted;
    private int numberOfCollectablesCollected;
    private bool[] collectableFromLevel;
    private bool audioMuted;

    public int NumberOfLevelsCompleted
    {
        get
        {
            return numberOfLevelsCompleted;
        }

        set
        {
            if (value >= levelCount)
                numberOfLevelsCompleted = levelCount;
            else
                numberOfLevelsCompleted = value;

            PlayerPrefs.SetInt(k_Levels, numberOfLevelsCompleted);
            PlayerPrefs.Save();
        }
    }

    public int NumberOfCollectablesCollected
    {
        get
        {
            return numberOfCollectablesCollected;
        }

        set
        {
            if (value >= levelCount)
                numberOfCollectablesCollected = levelCount;
            else
                numberOfCollectablesCollected = value;

            PlayerPrefs.SetInt(k_Collectables, numberOfCollectablesCollected);
            PlayerPrefs.Save();
        }
    }

    public bool[] CollectableFromLevel
    {
        get
        {
            return collectableFromLevel;
        }

        set
        {
            if (value.Length == levelCount)
            {
                collectableFromLevel = value;
                PlayerPrefsX.SetBoolArray(k_CollectablesArray, collectableFromLevel);

                int tempCollectablesCollected = 0;

                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i])
                    {
                        tempCollectablesCollected += 1;
                    }
                }

                NumberOfCollectablesCollected = tempCollectablesCollected;
                PlayerPrefs.Save();
            }
            else
                Debug.LogError("Array Index out of range.");
        }
    }    

    public bool AudioMuted
    {
        get
        {
            return audioMuted;
        }
        set
        {
            audioMuted = value;
            CheckForMuteState();

            PlayerPrefsX.SetBool(k_AudioMuted, audioMuted);
            PlayerPrefs.Save();
        }
    }

    // KEYS
    private readonly string k_Levels = "numberOfLevelsCompleted";
    private readonly string k_Collectables = "numberOfCollectablesCollected";
    private readonly string k_CollectablesArray = "collectableFromLevel";
    private readonly string k_AudioMuted = "audioMuted";

    // MISC
    public static GameController instance;
    private int levelCount;
    [SerializeField]
    private int[] LastLevelOfArea;//USED TO DETERMINE WICH AREA A LEVEL IS FROM
    #endregion

    //------------------------------------------------------

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        levelCount = SceneManager.sceneCountInBuildSettings - 3;
        CheckForSavedData();
        ChangePauseState(false);
    }

    private void Start()
    {
        TriggerSceneChanged();
    }

    private void CheckForSavedData()
    {
        if (!PlayerPrefs.HasKey(k_Levels))
            PlayerPrefs.SetInt(k_Levels, 0);


        if (!PlayerPrefs.HasKey(k_Collectables))
            PlayerPrefs.SetInt(k_Collectables, 0);

        if (!PlayerPrefs.HasKey(k_AudioMuted))
            PlayerPrefsX.SetBool(k_AudioMuted, false);

        // ASSIGN VALUES
        NumberOfLevelsCompleted = PlayerPrefs.GetInt(k_Levels);
        NumberOfCollectablesCollected = PlayerPrefs.GetInt(k_Collectables);
        AudioMuted = PlayerPrefsX.GetBool(k_AudioMuted);

        if (PlayerPrefsX.GetBoolArray(k_CollectablesArray).Length != 0)
        {
            CollectableFromLevel = PlayerPrefsX.GetBoolArray(k_CollectablesArray);
        }
        else
        {
            CollectableFromLevel = new bool[levelCount];
            PlayerPrefsX.SetBoolArray(k_CollectablesArray, CollectableFromLevel);
        }

        PlayerPrefs.Save();
    }

    public void ClearAllSavedData()
    {
        PlayerPrefsX.SetBoolArray(k_CollectablesArray, new bool[levelCount]);
        PlayerPrefs.DeleteAll();
    }

    public int GetLevelArea()
    {
        for (int i = 0; i < LastLevelOfArea.Length; i++)
        {
            if (GetLevelNumber() <= LastLevelOfArea[i] && GetLevelNumber() != 0)
            {
                return i;
            }
        }
        return (GetLevelNumber() > 0 && GetLevelNumber() <= levelCount ? LastLevelOfArea.Length : 0);
    }

    public int GetLevelCount() { return levelCount; }

    public int[] GetLastLevelOfArea() { return LastLevelOfArea; }

    #region SceneManagement
    public void RestartScene()
    {        
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void LoadMainMenu()
    {        
        SceneManager.LoadScene("Main_Menu");
    }

    public void LoadExtras()
    {        
        SceneManager.LoadScene("Extras");
    }

    public void LoadNextLevel()
    {
        if(Application.CanStreamedLevelBeLoaded("Level_" + (GetLevelNumber() + 1)))
        {            
            SceneManager.LoadScene("Level_" + (GetLevelNumber() + 1));
        }
        else
        {            
            Debug.LogWarning("There was an error loading the next level.");
            LoadLevelSelect();
        }
    }

    public void LoadLevelSelect()
    {        
        SceneManager.LoadScene("Level_Select");
    }

    public void ChangePauseState(bool paused)
    {
        if (paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public int GetLevelNumber()
    {
        string currentLevelName = SceneManager.GetActiveScene().name;
        if (currentLevelName.Split('_')[0] == "Level" && Int32.TryParse(currentLevelName.Split('_')[1], out _))
        {
            return Int32.Parse(currentLevelName.Split('_')[1]);
        }
        else
        {
            return 0;
        }
    }

    public void CheckForMuteState()
    {
        if (Camera.main.gameObject.GetComponent<AudioListener>() != null)
        {
            if (AudioMuted)
                AudioListener.volume = 0;
            else
                AudioListener.volume = 1;
        }
    }
   
    // TELLS AUDIO CONTROLLER THE SCENE HAS CHANGED
    private void TriggerSceneChanged()
    {
        if(AudioController.instance != null)
        {
            AudioController.instance.OnSceneChanged();
        }
    }
    #endregion

    // DEBUG ----------------------------------------------- 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }
        if (Input.GetKeyDown(KeyCode.F12) && (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)))
        {
            UnlockAllLevels();
            print("All levels unlocked!");
        }
    }

    public void UnlockAllLevels()
    {
        NumberOfLevelsCompleted = levelCount;
        RestartScene();
    }
}
