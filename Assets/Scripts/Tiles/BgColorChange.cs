using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BgColorChange : MonoBehaviour
{
    private Image image;
    private bool fadingColor = false;
    private bool fadinAlpha = false;
    private float timeBetweenCoroutines = 4f;
    private float timeBetweenColors;    
    private Color[] colorsToFade = new Color[7];

    private void Awake()
    {
        image = GetComponent<Image>();
        timeBetweenColors = (timeBetweenCoroutines / colorsToFade.Length);

        colorsToFade[0] = new Color32(141, 90, 90, 255); // RED        
        colorsToFade[1] = new Color32(140, 134, 90, 255); // YELLOW
        colorsToFade[2] = new Color32(97, 138, 90, 255); // GREEN
        colorsToFade[3] = new Color32(91, 138, 131, 255); // TEAL
        colorsToFade[4] = new Color32(91, 104, 138, 255); // BLUE
        colorsToFade[5] = new Color32(108, 91, 138, 255); // PURPLE
        colorsToFade[6] = new Color32(138, 91, 118, 255); // MAGENTA
    }

    private void Start()
    {
        image.CrossFadeColor(colorsToFade[0], 0, true, false);
    }

    private void Update()
    {
        if (!fadingColor)
        {
            fadingColor = true;
            StartCoroutine(ChangeColor());            
        }
        
        if(!fadinAlpha)
        {
            //fadinAlpha = true;
            //StartCoroutine(ChangeAlpha());
        }
    }

    private IEnumerator ChangeColor()
    {
        for (int i = 0; i < colorsToFade.Length; i++)
        {
            image.CrossFadeColor(colorsToFade[i], timeBetweenColors, false, false);
            yield return new WaitForSeconds(timeBetweenColors);
        }

        fadingColor = false;
    }

    private IEnumerator ChangeAlpha()
    {
        for (float i = 1; i > 0.5f; i -= 0.1f)
        {
            image.CrossFadeAlpha(i, 0.4f, false);
            yield return new WaitForSeconds(0.4f);
        }

        for (float i = 0.5f; i < 1; i += 0.1f)
        {
            image.CrossFadeAlpha(i, 0.4f, false);
            yield return new WaitForSeconds(0.4f);
        }

        fadinAlpha = false;
    }
}
