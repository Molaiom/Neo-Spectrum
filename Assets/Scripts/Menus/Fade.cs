using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [Header("Objects to Fade")]
    public Image[] imageToFade;
    public SpriteRenderer[] spriteToFade;
    public Text[] textToFade;
    // LIST FOR IMAGES AND TEXTS
    private List<Graphic> canvasComponents = new List<Graphic>();

    [Header ("Fade in settings")]
    [Tooltip("If both Fade In and Fade Out are set to happen at once, results may not work as expected")]
    public bool fadeIn;
    public float timeToStartFadeIn;
    public float fadeInDuration;

    [Header ("Fade out settings")]
    [Tooltip("If both Fade In and Fade Out are set to happen at once, results may not work as expected")]
    public bool fadeOut;
    public float timeToStartFadeOut;
    public float fadeOutDuration;


    public void Start()
    {
        AssignList();

        if (fadeIn)
            StartCoroutine(FadeIn());

        if (fadeOut)
            StartCoroutine(FadeOut());
    }

    private void AssignList()
    {
        // ASSIGN IMAGES
        for (int i = 0; i < imageToFade.Length; i++)
        {
            canvasComponents.Add(imageToFade[i]);
        }

        // ASSIGN TEXTS
        for (int i = 0; i < textToFade.Length; i++)
        {
            canvasComponents.Add(textToFade[i]);
        }
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(timeToStartFadeIn);

        // FADES IN IMAGES AND TEXTS
        for (int i = 0; i < canvasComponents.Count; i++)
        {
            canvasComponents[i].enabled = true;
            canvasComponents[i].CrossFadeAlpha(0, 0, true);
            canvasComponents[i].CrossFadeAlpha(1, fadeInDuration, true);
        }

        // FADES IN SPRITE RENDERERS
        for (int i = 0; i < spriteToFade.Length; i++)
        {
            StartCoroutine(FadeInSprites(spriteToFade[i]));
        }
    }

    private IEnumerator FadeInSprites(SpriteRenderer sprite)
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.01f);
        //float delayTime = sprite.color.a / fadeOutDuration;
        sprite.enabled = true;

        while (sprite.color.a < 1)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a * 2);
            yield return new WaitForSeconds(1 / fadeInDuration);
        }
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(timeToStartFadeOut);
    }

    /*
    private void Start()
    {
        if (gameObject.GetComponent<Image>() != null)
        {
            gameObject.GetComponent<Image>().enabled = true;
            gameObject.GetComponent<Image>().CrossFadeAlpha(0, fadeOutDuration, true);
        }

        if(gameObject.GetComponent<SpriteRenderer>() != null)
        {
            StartCoroutine(FadeOutSpriteRenderer());
        }

        Invoke("DeactivateObject", fadeOutDuration);
    }

    private void DeactivateObject()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator FadeOutSpriteRenderer()
    {
        if(gameObject.GetComponent<SpriteRenderer>() != null)
        {
            SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
            float delayTime = sprite.color.a / fadeOutDuration;

            sprite.enabled = true;
            yield return new WaitForSeconds(fadeOutDuration / 3);

            while(sprite.color.a > 0)
            {
                float fadeAmount = (sprite.color.a > 0.1f) ? 0.95f : 0.85f; 
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a * fadeAmount);
                yield return new WaitForSeconds(delayTime);
            }            
        }        
    }*/
}
