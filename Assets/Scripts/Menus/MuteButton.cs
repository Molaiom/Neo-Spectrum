using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    [SerializeField] private Sprite unmuteImage;
    [SerializeField] private Sprite muteImage;
    private Image buttonImage;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();        
    }

    private void OnEnable()
    {
        CheckForMuteState();
    }

    public void CheckForMuteState()
    {
        if (GameController.instance != null)
        {            
            if (GameController.instance.AudioMuted)
            {
                buttonImage.sprite = unmuteImage;
            }
            else
            {
                buttonImage.sprite = muteImage;
            }
        }
    }

    public void OnButtonPress()
    {
        if (GameController.instance != null)
        {
            GameController.instance.AudioMuted = !GameController.instance.AudioMuted;

            if (GameController.instance.AudioMuted)
            {                
                buttonImage.sprite = unmuteImage;
            }
            else
            {
                buttonImage.sprite = muteImage;
            }
        }
    }
}
