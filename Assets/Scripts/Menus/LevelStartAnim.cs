using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelStartAnim : MonoBehaviour
{
    public Text levelText;
    public Text miscText;


    private void Start()
    {
        if(GameController.instance != null)
        {
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
        SetTexts();

        // FADE IN
        levelText.CrossFadeAlpha(1, 0.7f, true);
        miscText.CrossFadeAlpha(1, 0.7f, true);
        yield return new WaitForSeconds(1.6f);

        // FADE OUT
        levelText.CrossFadeAlpha(0, 1.2f, false);
        miscText.CrossFadeAlpha(0, 1.2f, false);

        // TEXTS GOES AWAY
        for (float f = 0; f < 2; f += 1 * Time.deltaTime)
        {
            // ROTATES
            transform.Rotate(Vector3.forward, (-f * 15f));

            // MOVES UP
            if (transform.position.y <= 1000)
                transform.Translate(Vector3.up * f * (f < 0.2f ? 5 : 20), Space.World);

            // SHRINKS
            float yScale = transform.localScale.y * 0.95f <= 0 ? transform.localScale.y : transform.localScale.y * 0.95f;
            float xScale = transform.localScale.x * 1.01f >= 10 ? transform.localScale.x : transform.localScale.x * 1.01f;
            transform.localScale = new Vector3(xScale , yScale, 1);

            yield return null;
        }

        // DEACTIVATE THE TEXT OBJECTS
        levelText.gameObject.SetActive(false);
        miscText.gameObject.SetActive(false);
    }

    private void SetTexts()
    {
        int levelNumber = GameController.instance.GetLevelNumber();

        // LEVEL TEXT
        levelText.text = levelText.text + " " + levelNumber;
    }
}