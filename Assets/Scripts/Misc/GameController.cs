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
    private float volumeEffects;
    private float volumeMusic;

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
            {
                Debug.LogError("Array Index out of range. \nReseting saved data.");
                ClearAllSavedData();
                RestartScene();
            }
        }
    }

    public float VolumeEffects
    {
        get
        {
            return volumeEffects;
        }
        set
        {
            volumeEffects = (value >= 0 && value <= 1) ? value : volumeEffects;
            SetVolume();

            PlayerPrefs.SetFloat(k_VolumeEffects, volumeEffects);
            PlayerPrefs.Save();
        }
    }

    public float VolumeMusic
    {
        get
        {
            return volumeMusic;
        }
        set
        {
            volumeMusic = (value >= 0 && value <= 1) ? value : volumeMusic;
            SetVolume();

            PlayerPrefs.SetFloat(k_VolumeMusic, volumeMusic);
            PlayerPrefs.Save();
        }
    }

    // KEYS
    private readonly string k_Levels = "numberOfLevelsCompleted";
    private readonly string k_Collectables = "numberOfCollectablesCollected";
    private readonly string k_CollectablesArray = "collectableFromLevel";
    private readonly string k_VolumeEffects = "volumeEffects";
    private readonly string k_VolumeMusic = "volumeMusic";

    // MISC
    public static GameController instance;
    private int levelCount;
    [SerializeField]
    private int[] lastLevelOfArea; // USED TO DETERMINE WICH AREA A LEVEL IS FROM
    [SerializeField]
    private int[] collectablesRequiredForExtra; // USED TO DETERMINE HOW MANY COLLECTABLES ARE NEEDED FOR EACH EXTRA
    #endregion

    //------------------------------------------------------

    #region Setters and Getters
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

        try
        {
            levelCount = SceneManager.sceneCountInBuildSettings - 3;
            CheckForSavedData();
        }
        catch (ArgumentOutOfRangeException)
        {
            ClearAllSavedData();
            RestartScene();
            throw;
        }

        ChangePauseState(false);
    }

    private void Start()
    {
        SetVolume();
        TriggerSceneChanged();

        if (AudioController.instance != null && GetLevelNumber() == 0 && Time.time > 0.1f)
            AudioController.instance.PlayButton();
    }

    private void CheckForSavedData()
    {
        if (!PlayerPrefs.HasKey(k_Levels))
            PlayerPrefs.SetInt(k_Levels, 0);

        if (!PlayerPrefs.HasKey(k_Collectables))
            PlayerPrefs.SetInt(k_Collectables, 0);

        if (!PlayerPrefs.HasKey(k_VolumeEffects))
            PlayerPrefs.SetFloat(k_VolumeEffects, 1);

        if (!PlayerPrefs.HasKey(k_VolumeMusic))
            PlayerPrefs.SetFloat(k_VolumeMusic, 0.5f);

        // ASSIGN VALUES
        NumberOfLevelsCompleted = PlayerPrefs.GetInt(k_Levels);
        NumberOfCollectablesCollected = PlayerPrefs.GetInt(k_Collectables);
        VolumeEffects = PlayerPrefs.GetFloat(k_VolumeEffects);
        VolumeMusic = PlayerPrefs.GetFloat(k_VolumeMusic);

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
        PlayerPrefs.DeleteKey(k_Levels);
        PlayerPrefs.DeleteKey(k_Collectables);
        PlayerPrefsX.SetBoolArray(k_CollectablesArray, new bool[levelCount]);
    }

    public int GetLevelArea()
    {
        for (int i = 0; i < lastLevelOfArea.Length; i++)
        {
            if (GetLevelNumber() <= lastLevelOfArea[i] && GetLevelNumber() != 0)
            {
                return i;
            }
        }
        return (GetLevelNumber() > 0 && GetLevelNumber() <= levelCount ? lastLevelOfArea.Length : 0);
    }

    public void SetVolume() // SETS VOLUME ON SCENE GAMEOBJECTS
    {
        if(AudioController.instance != null)
        {
            AudioController.instance.musicMaxVolume = VolumeMusic;
            AudioController.instance.SetEffectsVolume(VolumeEffects);
        }
    }

    public int GetLevelCount() { return levelCount; }

    public int[] GetLastLevelOfArea() { return lastLevelOfArea; }

    public int[] GetCollectablesRequiredForExtra() { return collectablesRequiredForExtra; }
    #endregion

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
   
    public void QuitGame()
    {
        Application.Quit();
        print("Game build closed.");
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
        if (Input.GetAxisRaw("Restart") > 0 && Time.timeSinceLevelLoad > 1 && GetLevelNumber() != 0)
        {
            RestartScene();
        }
        
        if (Input.GetKeyDown(KeyCode.F12) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            UnlockAllLevels();
        }
    }

    public void UnlockAllLevels()
    {
        NumberOfLevelsCompleted = levelCount;
        print("All levels unlocked!");
        RestartScene();
    }
}
