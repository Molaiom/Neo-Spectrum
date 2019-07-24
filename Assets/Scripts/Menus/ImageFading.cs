using UnityEngine;
using UnityEngine.UI;

public class ImageFading : MonoBehaviour
{
    private Image image;

    private void Start()
    {
        if (GetComponent<Image>() != null)
            image = GetComponent<Image>();

        InvokeRepeating("FadeOut", 0, 4);
        InvokeRepeating("FadeIn", 2, 4);
    }

    private void FadeIn()
    {
        image.CrossFadeAlpha(1, 1.95f, true);
        
    }

    private void FadeOut()
    {
        image.CrossFadeAlpha(0.6f, 1.95f, true);
        
    }
}
