using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    #region Attributes

    [Header("Player")]
    public AudioClip playerJump;
    public AudioClip playerDeath;
    public AudioClip collectableCollected;

    [Header("Enemies")]
    public AudioClip bossLaugh;

    [Header("Blocks")]
    public AudioClip blockPlaced;
    public AudioClip blockImpact;
    public AudioClip bounce;
    public AudioClip laserTile;

    [Header("Misc Effects")]
    public AudioClip buttonPressed;
    public AudioClip death;
    public AudioClip levelCompleted;
    public AudioClip click;

    [Header("Music")]
    public AudioClip musicMenu;
    public AudioClip musicLevel;
    public float musicMaxVolume;
    
    private AudioSource effectsAudioSource;
    private AudioSource musicAudioSource;
    public static AudioController instance;

    private bool isChangingVolume;
    #endregion


    private void Awake()
    {
        // CHECK FOR SINGLETON INSTANCE
        CheckForInstance();

        // ASSIGN AUDIO SOURCES
        if (GetComponents<AudioSource>() != null)
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();
            effectsAudioSource = audioSources[0];
            musicAudioSource = audioSources[1];
        }

        StartCoroutine(ChangeMusic(musicMenu));
    }

    private void Update()
    {
        CheckForPauseState();        
    }

    private void CheckForInstance() // CHECK FOR SINGLETON INSTANCE
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void CheckForPauseState() // PAUSES EFFECTS AND CHANGE MUSIC VOLUME ON PAUSE
    {
        if (Time.timeScale == 0)
        {
            effectsAudioSource.Pause();                        
            isChangingVolume = false;
            musicAudioSource.volume = musicMaxVolume * 0.2f;
        }
        else
        {
            effectsAudioSource.UnPause();
            if(!isChangingVolume)
            {
                musicAudioSource.volume = musicMaxVolume;
            }
        }
    }

    public void SetEffectsVolume(float volume)
    {
        effectsAudioSource.volume = volume;
    }

    public void OnSceneChanged() // CALLS THE CHANGEMUSIC METHOD DEPENDING ON SCENE
    {
        effectsAudioSource.Stop();

        if(GameController.instance != null)
        {
            // IF THE PLAYER IS IN A MENU SCENE
            if(GameController.instance.GetLevelNumber() == 0)
            {
                if (musicAudioSource.clip != musicMenu)
                    StartCoroutine(ChangeMusic(musicMenu));
            }
            // IF THE PLAYER IS IN A LEVEL
            else
            {
                if (musicAudioSource.clip != musicLevel)
                    StartCoroutine(ChangeMusic(musicLevel));
            }
        }
    }

    #region Audio Effects
    // PLAYER
    public void PlayPlayerJump()
    {
        effectsAudioSource.PlayOneShot(playerJump);
    }

    public void PlayPlayerDeath()
    {
        effectsAudioSource.PlayOneShot(playerDeath);
    }

    public void PlayCollectableCollected()
    {
        effectsAudioSource.PlayOneShot(collectableCollected);
    }

    // ENEMIES   
    public void PlayBossLaugh()
    {
        effectsAudioSource.PlayOneShot(bossLaugh);
    }

    // BLOCKS
    public void PlayBlockPlaced()
    {
        effectsAudioSource.PlayOneShot(blockPlaced);
    }

    public void PlayBlockImpact()
    {
        effectsAudioSource.PlayOneShot(blockImpact);
    }

    public void PlayBounce()
    {
        effectsAudioSource.PlayOneShot(bounce);
    }

    public void PlayLaserTile()
    {
        effectsAudioSource.clip = laserTile;
        effectsAudioSource.Play();
    }

    // MISC EFFECTS
    public void PlayButton()
    {
        if (instance != null)
        {
            AudioController ac = instance;
            ac.effectsAudioSource.PlayOneShot(buttonPressed, 0.5f);
        }
    }

    public void PlayDeath()
    {
        effectsAudioSource.PlayOneShot(death);
    }

    public void PlayLevelCompleted()
    {
        effectsAudioSource.PlayOneShot(levelCompleted);
    }

    public void PlayClick()
    {
        effectsAudioSource.PlayOneShot(click);
    }
    #endregion

    #region Music Methods
    private IEnumerator FadeInMusicVolume(float delay) // FADES IN THE MUSIC VOLUME
    {        
        musicAudioSource.volume = 0;

        while(musicAudioSource.volume < musicMaxVolume)
        {

            musicAudioSource.volume += 0.025f;
            yield return new WaitForSeconds(delay);
        }

        isChangingVolume = false;
        musicAudioSource.volume = musicMaxVolume;        
    }       

    private IEnumerator FadeOutMusicVolume(float delay) // FADES OUT THE MUSIC VOLUME
    {        
        while(musicAudioSource.volume > 0)
        {
            musicAudioSource.volume -= 0.025f;
            yield return new WaitForSeconds(delay);
        }        
    }

    private IEnumerator ChangeMusic(AudioClip musicToPlay) // CHANGES THE MUSIC
    {
        isChangingVolume = true;
        StartCoroutine(FadeOutMusicVolume(0.1f));

        yield return new WaitForSecondsRealtime(0.25f);

        musicAudioSource.Stop();
        musicAudioSource.clip = musicToPlay;
        musicAudioSource.Play();

        StartCoroutine(FadeInMusicVolume(0.25f));        
    }
    #endregion
}
