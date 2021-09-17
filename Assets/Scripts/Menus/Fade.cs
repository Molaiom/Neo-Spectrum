using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Rendering/ Fade")]
// FADES IN / OUT ALL CANVAS GRAPHIC COMPONENTS AND SPRITE RENDERERS
public class Fade : MonoBehaviour
{
    public bool fadeIn;
    [Header("Fade in settings")]
    [Tooltip("If both Fade In and Fade Out are set to happen at once, results may not work as expected.")]
    public float timeToStartFadeIn;
    public float fadeInDuration;
    public float fadeInMaxAlpha = 1;

    public bool fadeOut;
    [Header("Fade out settings")]
    [Tooltip("If both Fade In and Fade Out are set to happen at once, results may not work as expected.")]
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

// ONLY SHOWS NEEDED PROPERTIES ON THE INSPECTOR
[CustomEditor(typeof(Fade))]
[CanEditMultipleObjects]
public class FadeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Fade fade = target as Fade;

        // FADE IN
        fade.fadeIn = GUILayout.Toggle(fade.fadeIn, "Fade In");
        if (fade.fadeIn)
        {
            //fade.timeToStartFadeIn = EditorGUILayout.FloatField()
            fade.timeToStartFadeIn = EditorGUILayout.FloatField("Time To Start Fade In" ,fade.timeToStartFadeIn);
            fade.fadeInDuration = EditorGUILayout.FloatField("Fade In Duration" ,fade.fadeInDuration);
            fade.fadeInMaxAlpha = EditorGUILayout.FloatField("Fade In Max Alpha" ,fade.fadeInMaxAlpha);
        }

        EditorGUILayout.Space();

        // FADE OUT
        fade.fadeOut = GUILayout.Toggle(fade.fadeOut, "Fade Out");
        if (fade.fadeOut)
        {
            fade.timeToStartFadeOut = EditorGUILayout.FloatField("Time To Start Fade Out", fade.timeToStartFadeOut);
            fade.fadeOutDuration = EditorGUILayout.FloatField("Fade Out Duration", fade.fadeOutDuration);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
