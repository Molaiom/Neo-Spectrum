using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [Header("Duration in Seconds")]
    public float fadeDuration = 0.5f;

    private void Start()
    {
        if (gameObject.GetComponent<Image>() != null)
        {
            gameObject.GetComponent<Image>().enabled = true;
            gameObject.GetComponent<Image>().CrossFadeAlpha(0, fadeDuration, true);            
        }

        if(gameObject.GetComponent<SpriteRenderer>() != null)
        {
            StartCoroutine(FadeOutSpriteRenderer());
        }

        Invoke("DeactivateObject", fadeDuration);
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
            float delayTime = sprite.color.a / fadeDuration;

            sprite.enabled = true;
            yield return new WaitForSeconds(fadeDuration / 3);

            while(sprite.color.a > 0)
            {
                float fadeAmount = (sprite.color.a > 0.1f) ? 0.95f : 0.85f; 
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a * fadeAmount);
                yield return new WaitForSeconds(delayTime);
            }            
        }        
    }
}
