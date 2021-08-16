using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class RangeController : MonoBehaviour // THIS SCRIPT MAKES THE RANGE APPEAR AND FADE OUT WHENEVER THE PLAYER TOUCHES THE SCREEN
{
    public SpriteRenderer touchParticles;
    public TrailRenderer trail;
    public float touchParticlesMaxScale = 1;
    public float touchParticlesMinScale = 1;
    public Sprite[] backgroundImage = new Sprite[4];

    public static RangeController instance;

    private float touchParticlesScale;
    private bool touchParticlesScaleGoingUp = false;
    private Color touchParticlesOriginalColor;
    private SpriteRenderer rangeSprite;
    private float rangeSpriteOriginalAlpha;


    private void Awake()
    {
        instance = this;
    }

    private void Start() // ASSIGN VALUES
    {
        if (FindObjectOfType<PlayerController>() != null)
        {
            // GETS A REFERENCE TO THE PLAYER
            GameObject player = FindObjectOfType<PlayerController>().gameObject;

            // FINDS THE PLAYER'S RANGE INDICATOR OBJECT
            for (int i = 0; i < player.transform.childCount; i++)
            {
                if (player.transform.GetChild(i).name == "Range Indicator")
                {
                    rangeSprite = player.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
                    rangeSpriteOriginalAlpha = rangeSprite.color.a;
                }
            }
        }

        Input.simulateMouseWithTouches = false;
        AssignBackgroundImage();

        touchParticlesOriginalColor = touchParticles.color;
        touchParticles.transform.parent = null;
        touchParticles.transform.position = Vector3.zero;
        touchParticlesScale = 1f;
        touchParticles.transform.localScale = new Vector3(touchParticlesScale, touchParticlesScale, 1);
    }

    public void Update()
    {
        if (PlayerController.instance != null)
        {
            if (!PlayerController.instance.GetPlayerDead() && Input.touchCount > 0 && Time.timeScale != 0 && !PlayerController.instance.levelCompleted)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);

                    if (touchPosition.y > -5)
                    {
                        // CLICK SOUNDS
                        if (Input.GetTouch(i).phase == TouchPhase.Began)
                        {
                            if (AudioController.instance != null)
                                AudioController.instance.PlayClick();
                        }
                        if (Input.GetTouch(i).phase == TouchPhase.Stationary || Input.GetTouch(i).phase == TouchPhase.Moved || Input.GetTouch(i).phase == TouchPhase.Began)
                        {
                            // DISPLAYS THE RANGE INDICATOR IF THE PLAYER TOUCHES OUTSIDE THE RANGE
                            if (Vector2.Distance(touchPosition, PlayerController.instance.gameObject.transform.position) >= 4.5f)
                            {
                                DisplayRangeIndicator();
                            }
                            // IF THE PLAYER TOUCHES INSIDE THE RANGE
                            else
                            {
                                // DISPLAYS THE TOUCH PARTICLES
                                DisplayTouchParticles(touchPosition);

                                // PAINT VALID TILES
                                PaintTile(touchPosition);
                            }
                        }
                    }
                    else
                    {
                        trail.enabled = false;
                        StartCoroutine(TouchParticlesFadeOut());
                    }
                }
            }
            else
            {
                StartCoroutine(TouchParticlesFadeOut());
                trail.enabled = false;
            }
        }
    }

    public void DisplayRangeIndicator()
    {
        rangeSprite.enabled = true;
        StopAllCoroutines();
        SetRangeSpriteAlpha(rangeSpriteOriginalAlpha);
        StartCoroutine(RangeFadeOut());
        trail.enabled = false;
        StartCoroutine(TouchParticlesFadeOut());
    }

    public void DisplayTouchParticles(Vector2 particlesPosition)
    {
        touchParticles.gameObject.transform.position = particlesPosition;
        AnimateTouchParticlesScale();
        touchParticles.enabled = true;
        trail.enabled = true;
        trail.startColor = touchParticles.color;
        trail.endColor = new Color(touchParticles.color.r, touchParticles.color.g, touchParticles.color.b, 0);
        SetTouchParticlesAlpha(touchParticlesOriginalColor.a);
    }

    public void PaintTile(Vector2 touchPosition)
    {
        TileInteractable[] tiles = FindObjectsOfType<TileInteractable>();
        for (int i = 0; i < tiles.Length; i++)
        {
            float diferenceX = touchPosition.x - tiles[i].transform.position.x;
            float diferenceY = touchPosition.y - tiles[i].transform.position.y;

            if ((diferenceX <= 0.5f && diferenceX >= -0.5f) && (diferenceY <= 0.5f && diferenceY >= -0.5f))
            {
                tiles[i].PaintTile();
            }
        }
    }

    private IEnumerator RangeFadeOut()
    {
        yield return new WaitForSeconds(0.15f);
        while (rangeSprite.color.a >= 0)
        {
            SetRangeSpriteAlpha(rangeSprite.color.a - 0.025f);
            yield return new WaitForSeconds(0.025f);
        }
        rangeSprite.enabled = false;
    }

    public IEnumerator TouchParticlesFadeOut()
    {
        while (touchParticles.color.a >= 0)
        {
            SetTouchParticlesAlpha(touchParticles.color.a - 0.025f);
            yield return new WaitForSeconds(0.1f);
        }
        touchParticles.enabled = false;
    }

    private void SetRangeSpriteAlpha(float value)
    {
        rangeSprite.color = new Color(rangeSprite.color.r, rangeSprite.color.g, rangeSprite.color.b, value);
    }

    private void SetTouchParticlesAlpha(float value)
    {
        touchParticles.color = rangeSprite.color;
        touchParticles.color = new Color(touchParticles.color.r, touchParticles.color.g, touchParticles.color.b, value);
    }

    private void AnimateTouchParticlesScale()
    {
        if (touchParticlesScaleGoingUp)
        {
            touchParticlesScale += 0.5f * Time.deltaTime;
            if (touchParticlesScale >= touchParticlesMaxScale)
                touchParticlesScaleGoingUp = false;
        }
        else
        {
            touchParticlesScale -= 0.5f * Time.deltaTime;
            if (touchParticlesScale < touchParticlesMinScale)
                touchParticlesScaleGoingUp = true;
        }

        trail.startWidth = touchParticlesScale;
        touchParticles.transform.localScale = new Vector3(touchParticlesScale, touchParticlesScale, 1);

    }

    private void AssignBackgroundImage() // THIS CHANGES THE BACKGROUND IMAGE TO CORREPOND THE AREA 
    {
        if (GameController.instance != null)
        {
            if (GameController.instance.GetLevelNumber() < 6)
            {
                GetComponent<Image>().sprite = backgroundImage[0];
            }
            else if (GameController.instance.GetLevelNumber() < 13)
            {
                GetComponent<Image>().sprite = backgroundImage[1];
            }
            else if (GameController.instance.GetLevelNumber() < 21)
            {
                GetComponent<Image>().sprite = backgroundImage[2];
            }
            else
            {
                GetComponent<Image>().sprite = backgroundImage[3];
            }
        }
    }
}
