using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteractable : MonoBehaviour
{
    #region Attributes
    public LayerMask playerLayer;
    protected float timerToPaint = 0.1f;
    protected float timerToFade = 1.5f;
    protected float lightIntensity;
    protected Color lightColor;
    protected Color originalColor;
    protected Light componentLight;
    public GameObject particlesPrefab;

    protected float velocity = 0;
    protected bool destroyed = false;
    #endregion

    #region Methods
    public virtual void Awake()
    {
        if (GetComponentInChildren<Light>() != null)
        {
            componentLight = GetComponentInChildren<Light>();
            lightIntensity = componentLight.intensity;
            lightColor = componentLight.color;

            componentLight.color = new Color(lightColor.r / 2, lightColor.g / 2, lightColor.b / 2, lightColor.a / 2);
            componentLight.intensity = lightIntensity / 1.5f;
        }

        if (GetComponent <SpriteRenderer>() != null)
            originalColor = GetComponent<SpriteRenderer>().color;
    }

    private void Update()
    {
        timerToPaint -= 1 * Time.deltaTime;
        Mathf.Clamp(timerToPaint, 0, Mathf.Infinity);

        timerToFade -= 1 * Time.deltaTime;
        Mathf.Clamp(timerToFade, 0, Mathf.Infinity);

        if (componentLight != null && timerToFade > 0)
            FadeColor();
    }

    public void PaintTile(GameObject blockPrefab)
    {
        if (IsInsidePlayer()) // IF THE PLAYER OVERLAPS THE BLOCK, THEN FLASH IT RED
        {
            StopAllCoroutines();
            StartCoroutine(FlashRed());
        }
        else if (!blockPrefab.CompareTag(gameObject.tag)) // IF THE PLAYER DOESN'T OVERLAP THE BLOCK, PAINT IT
        {
            Instantiate(blockPrefab, transform.position, transform.rotation);
            if (AudioController.instance != null)
            {
                AudioController.instance.PlayBlockPlaced();
            }
            Destroy(gameObject);
        }
    }

    private bool IsInsidePlayer() // CHECKS IF THE TILE IS INSIDE THE PLAYER
    {
        if (gameObject.tag == "WhiteTile")
            return Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.05f), 0.4f, playerLayer);
        else
            return false;
    }

    private void FadeColor()
    {
        float R = componentLight.color.r;
        float G = componentLight.color.g;
        float B = componentLight.color.b;
        float A = componentLight.color.a;

        R = Mathf.SmoothDamp(R, lightColor.r, ref velocity, 0.1f, 1f, Time.deltaTime * 2);
        G = Mathf.SmoothDamp(G, lightColor.g, ref velocity, 0.1f, 1f, Time.deltaTime * 2);
        B = Mathf.SmoothDamp(B, lightColor.b, ref velocity, 0.1f, 1f, Time.deltaTime * 2);
        A = Mathf.SmoothDamp(A, lightColor.a, ref velocity, 0.1f, 1f, Time.deltaTime * 2);
        componentLight.intensity = Mathf.SmoothDamp(componentLight.intensity, lightIntensity, ref velocity, 0.1f, 1.25f, Time.deltaTime * 5);

        componentLight.color = new Color(R, G, B, A);
    }

    private IEnumerator FlashRed()
    {
        SpriteRenderer blockSprite = GetComponent<SpriteRenderer>();
        blockSprite.color = new Color(1, 0, 0, originalColor.a);

        float G = blockSprite.color.g;
        float B = blockSprite.color.b;

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.2f);
            G = G >= 1 ? 1 : G + (i > 5 ? 0.15f : 0.05f); 
            B = B >= 1 ? 1 : B + (i > 5 ? 0.15f : 0.05f);
            blockSprite.color = new Color(blockSprite.color.r, G, B, originalColor.a);
        }
    }

    public void DestroyTile(Color particlesColor)
    {
        if (AudioController.instance != null)
        {
            AudioController.instance.PlayDeath();
        }

        GameObject obj = Instantiate(particlesPrefab, transform.position, transform.rotation, transform);
        ParticleSystem particles = obj.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mainModule = particles.main;
        mainModule.startColor = particlesColor;
        destroyed = true;

        particles.Emit(20);

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponentInChildren<Light>().enabled = false;

        Destroy(gameObject, 2);
    }
    #endregion

}
