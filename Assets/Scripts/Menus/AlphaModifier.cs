using UnityEngine;
using UnityEngine.UI;

public class AlphaModifier : MonoBehaviour
{
    Image img;
    bool goingDown = true;
    public GameObject deathText;

    [Range(0,1)]
    public float maxAlpha;
    [Range(0, 1)]
    public float minAlpha;

    void Start ()
    {
        img = GetComponent<Image>();
        img.CrossFadeAlpha(0, 0.0f, true);
        InvokeRepeating("ModifyAlpha", 0, 0.5f);
	}

    private void Update()
    {
        if (PlayerController.instance != null)
        {
            if (PlayerController.instance.GetPlayerDead())
                deathText.gameObject.SetActive(true);
        }
    }

    void ModifyAlpha()
    {
        if (PlayerController.instance != null)
        {
            if (PlayerController.instance.GetPlayerDead())
            {

                if (goingDown)
                {
                    img.CrossFadeAlpha(minAlpha, 0.4f, true);
                    goingDown = false;
                }
                else
                {
                    img.CrossFadeAlpha(maxAlpha, 0.4f, true);
                    goingDown = true;
                }
            }
        }
    }
}
