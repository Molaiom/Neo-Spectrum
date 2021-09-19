using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Rendering/ Fade")]
// FADES IN / OUT ALL CANVAS GRAPHIC COMPONENTS AND SPRITE RENDERERS
public class Fade : MonoBehaviour
{
    [Header("Fade in settings")]
    [Tooltip("If both Fade In and Fade Out are set to happen at once, results may not work as expected.")]
    public bool fadeIn;
    public float timeToStartFadeIn;
    public float fadeInDuration;
    public float fadeInMaxAlpha = 1;

    [Header("Fade out settings")]
    [Tooltip("If both Fade In and Fade Out are set to happen at once, results may not work as expected.")]
    public bool fadeOut;
    public float timeToStartFadeOut;
    public float fadeOutDuration;


    public void Start()
    {
        if (fadeIn)
            StartCoroutine(FadeIn());

        if (fadeOut)
            StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(timeToStartFadeIn);

        // FADES IN GRAPHIC COMPONENTS
        if (TryGetComponent(out Graphic graphic))
        {
            graphic.enabled = true;
            graphic.CrossFadeAlpha(0, 0, true);
            graphic.CrossFadeAlpha(1, fadeInDuration, true);
        }

        // FADES IN SPRITE RENDERERS
        if (TryGetComponent(out SpriteRenderer sprite))
        {
            sprite.enabled = true;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);

            for (float f = 0; f <= fadeInMaxAlpha; f += Time.deltaTime / fadeInDuration * fadeInMaxAlpha)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, f);
                yield return null;
            }
        }
    }

    private IEnumerator FadeOut()
    {
        

        // FADES OUT GRAPHIC COMPONENTS
        if (TryGetComponent(out Graphic graphic))
        {
            graphic.enabled = timeToStartFadeOut == 0 ? true : graphic.enabled;
            yield return new WaitForSeconds(timeToStartFadeOut);
            graphic.enabled = true;
            
            graphic.CrossFadeAlpha(0, fadeOutDuration, true);
            yield return new WaitForSeconds(fadeOutDuration);
            graphic.enabled = false;
        }

        // FADES OUT SPRITE RENDERERS
        if (TryGetComponent(out SpriteRenderer sprite))
        {
            sprite.enabled = timeToStartFadeOut == 0 ? true : sprite.enabled;
            yield return new WaitForSeconds(timeToStartFadeOut);
            sprite.enabled = true;

            float startingAlpha = sprite.color.a;

            for (float f = startingAlpha; f >= 0; f -= Time.deltaTime / fadeOutDuration * startingAlpha)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, f);
                yield return null;
            }
            sprite.enabled = false;
        }
    }
}
