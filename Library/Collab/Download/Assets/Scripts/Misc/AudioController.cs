using UnityEngine;

public class AudioController : MonoBehaviour
{
    #region Attributes
    public AudioClip blockPlaced;
    public AudioClip death;
    public AudioClip Pause;
    public AudioClip Button;

    private AudioSource aSource;
    public static AudioController instance;
    #endregion

    #region Methods
    private void Awake()
    {
        aSource = GetComponent<AudioSource>();
        instance = this;

    }
    #endregion

    #region Sound Effects 
    public void PlayBlockPlaced()
    {
        aSource.PlayOneShot(blockPlaced, 0.5f);
    }

    public void PlayDeathSound()
    {
        aSource.PlayOneShot(death, 1f);
    }
    #endregion

    #region Menu Sounds
    public void PlayPause()
    {
        aSource.PlayOneShot(Pause, 0.7f);
    }

    public void PlayButton()
    {
        aSource.PlayOneShot(Button, 0.7f);
    }
    
    public void PlayMusic()
    {
        if(!aSource.isPlaying)
        aSource.Play(0);
    }
    #endregion
}
