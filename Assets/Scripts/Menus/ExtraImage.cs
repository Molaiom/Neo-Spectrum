using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ExtraImage : MonoBehaviour
{
    public GameObject fullImage;
    public Image lockedMarker;
    public Text collectablesText;
    public float fadeDuration;
    public int index;

    private float collectablesRequired;
    private Button button;

    // ---------------------------------------------

    private void Awake() // ASSIGN COMPONENTS
    {
        button = GetComponent<Button>();        
    }

    private void  Start() // CHECK IF UNLOCKED HERE
    {
        if (GameController.instance != null)
        {
            collectablesRequired = GameController.instance.GetCollectablesRequiredForExtra()[index];
        }
        CheckIfUnlocked();
        UpdateText();

        Invoke("CheckIfUnlocked", 0.5f);
        Invoke("UpdateText", 0.5f);
    }


    private void CheckIfUnlocked() // TO BE CALLED ON START
    {
        if(GameController.instance != null)
        {
            if (GameController.instance.NumberOfCollectablesCollected >= collectablesRequired)
                ChangeLockState(true);
            else
                ChangeLockState(false);         
        }
        else
        {
            Debug.LogError("GameController Instance not found!");
            return;
        }
    }      

    private void UpdateText() // TO BE CALLED ON START
    {
        if(GameController.instance != null)
        {
            collectablesText.text = GameController.instance.NumberOfCollectablesCollected.ToString() + " / " + collectablesRequired.ToString();
        }
        else
        {
            collectablesText.text = "Error / " + collectablesRequired.ToString();
        }
    }
    
    public void OpenFullImage() // TO BE CALLED ON CLICK
    {        
        Image image = fullImage.transform.GetChild(0).GetComponent<Image>();

        fullImage.gameObject.SetActive(true);

        fullImage.GetComponent<Image>().CrossFadeAlpha(0, 0, true);
        image.CrossFadeAlpha(0, 0, true);
        

        fullImage.GetComponent<Image>().CrossFadeAlpha(1, fadeDuration, true);
        image.CrossFadeAlpha(1, fadeDuration, true);

    }    

    private void ChangeLockState(bool unlocked) // TO BE CALLED ON CHECKIFUNLOCKED
    {
        if (!unlocked)
        {
            button.interactable = false;
            lockedMarker.enabled = true;

        }
        else
        {
            button.interactable = true;
            lockedMarker.enabled = false;
        }
    }
}
