using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelStartAnim : MonoBehaviour
{
    private Text levelText;
    private Text miscText;

    private void Awake()
    {
        levelText = transform.GetChild(0).GetComponent<Text>();
        miscText = transform.GetChild(1).GetComponent<Text>();
    }

    private void Start()
    {        
        if(GameController.instance != null)
        {
            SetTexts();
            StartCoroutine(Animate());
        }        
    }

    private IEnumerator Animate() // ANIMATES THE TEXTS
    {
        // ACTIVATE THE TEXT OBJECTS
        levelText.gameObject.SetActive(true);
        miscText.gameObject.SetActive(true);

        // CHANGES THEIR ALPHA TO ZERO SO THEY CAN FADE IN
        levelText.CrossFadeAlpha(0, 0, true);
        miscText.CrossFadeAlpha(0, 0, true);
        yield return new WaitForEndOfFrame();

        // FADE IN
        levelText.CrossFadeAlpha(1, 0.7f, true);
        miscText.CrossFadeAlpha(1, 0.7f, true);
        yield return new WaitForSeconds(1.4f);

        // FADE OUT
        levelText.CrossFadeAlpha(0, 1.2f, false);
        miscText.CrossFadeAlpha(0, 1.2f, false);

        // TEXTS GOES AWAY
        for (int i = 0; i < 27; i++)
        {
            // ROTATES
            transform.Rotate(Vector3.forward, (-i * 1.5f));

            // MOVES UP
            if(i < 5)
                transform.Translate(Vector3.up * i, Space.World);
            else
                transform.Translate(Vector3.up * (i * 5), Space.World);

            // SHRINKS
            transform.localScale = new Vector3(transform.localScale.x * 1.01f, transform.localScale.y * 0.95f, 1);


            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
        }
        

        yield return new WaitForSeconds(1.1f);        

        // DEACTIVATE THE TEXT OBJECTS
        levelText.gameObject.SetActive(false);
        miscText.gameObject.SetActive(false);
    }

    private void SetTexts()
    {
        int levelNumber = GameController.instance.GetLevelNumber();

        // LEVEL TEXT
        levelText.text = "Level " + levelNumber;


        // MISC TEXT
        switch (levelNumber)
        {
            case 1:
                miscText.text = "Welcome !";
                break;

            case 2:
                miscText.text = "Coloring !";
                break;

            case 3:
                miscText.text = "Platforms !";
                break;

            case 5:
                miscText.text = "Long Push !";
                break;

            case 6:
                miscText.text = "Green !";
                break;

            case 7:
                miscText.text = "Both Colors !";
                break;

            default:
                miscText.text = "";
                break;

            /* COPY PASTE
            case 0:
                miscText.text = "";
                break;
            */
        }
    }
}