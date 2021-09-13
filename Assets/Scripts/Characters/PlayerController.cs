using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class PlayerController : CharacterPhysics
{
    #region Attributes
    public bool levelCompleted = false;
    [Header("Player Ranges")]
    public float wallJumpRaycastRange;
    public SpriteRenderer rangeSprite;
    public float paintRange;
    [HideInInspector]
    public bool collectable = false;

    private bool playerDead = false;
    private RaycastHit2D hit;
    private Animator anim;
    private Coroutine coroutine;
    private GameObject tileIgnoredForCollision;
    private float minDistanceToIgnoreCol = .5f;
    #endregion


    #region Methods
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // KILLS THE PLAYER IF OUT OF BOUNDS
        if (transform.position.y < -5.5f || transform.position.y > 8.5f || transform.position.x < -12 || transform.position.x > 12)
        {
            if (!playerDead)
                Die();
        }

        // RE-ENABLES THE COLLISION WITH A TILE THAT WAS OVERLAPING THE PLAYER
        if(tileIgnoredForCollision != null)
        {
            if (Vector2.Distance(tileIgnoredForCollision.transform.position, transform.position) > minDistanceToIgnoreCol)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), tileIgnoredForCollision.GetComponent<Collider2D>(), false);
                tileIgnoredForCollision = null;
            }
        }
    }

    private void Update() // INPUTS AND ANIMATIONS
    {
        if (!Application.isMobilePlatform && !levelCompleted)
        {
            if (Input.GetKey(KeyCode.Space))
                SetJumpAxis(Input.GetAxis("Jump"));
            else
                SetJumpAxis(0);

            if (Input.GetKey(KeyCode.A))
                SetMovementAxis(-1);
            else if (Input.GetKey(KeyCode.D))
                SetMovementAxis(1);
            else
                SetMovementAxis(0);
        }

        UpdateAnimations();
    }

    private void UpdateAnimations() // MAKES ANIMATIONS TRANSITIONS
    {
        if (!isGrounded)
        {
            if (onWall) // WALLJUMP ANIM
            {
                anim.SetInteger("State", 3);
            }
            else if (rb2d.velocity.y < -0.05f || rb2d.velocity.y > 0.05f) // JUMP ANIM
            {
                anim.SetInteger("State", 2);
            }
        }
        else
        {
            if (rb2d.velocity.x < -0.025f || rb2d.velocity.x > 0.025f) // WALK ANIM
            {
                anim.SetInteger("State", 1);
            }
            else // IDLE ANIM
            {
                anim.SetInteger("State", 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // END OF THE LEVEL INTERACTIONS
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            HideAndStopPlayer();
            levelCompleted = true;

            if (AudioController.instance != null)
                AudioController.instance.PlayLevelCompleted();

            if (LevelController.instance != null)
            {
                LevelController.instance.playerCompletedCount++;
                if (collectable)
                    LevelController.instance.collectable = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // INTERACTIONS WITH COLORED TILES
    {
        // BOUNCY SOUND WHEN JUMPING ONTO THE BOUNCY TILE
        Vector2 position = new Vector2(transform.position.x, transform.position.y + groundCheckVerticalOffset * 2f);
        Vector2 size = new Vector2(groundRadius, groundRadius * 1.7f);
        Collider2D blockBellow = Physics2D.OverlapBox(position, size, 0, groundLayer);

        if (blockBellow != null)
        {
            if (blockBellow.gameObject.CompareTag("BouncyTile") && !BlockAboveHead() && AudioController.instance != null)
            {
                AudioController.instance.PlayBounce();
            }
        }

        // IF A TILE IS INSIDE THE PLAYER, IGNORE THE COLLISION WITH IT (RE-ENABLES ON FIXED UPDATE())
        if (collision.gameObject.GetComponent<TileInteractable>() != null 
            && Vector2.Distance(collision.transform.position, transform.position) <= minDistanceToIgnoreCol)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, true);
            tileIgnoredForCollision = collision.gameObject;
        }
    }

    protected override void WallJump() // ALLOWS THE PLAYER TO WALLJUMP ON SOME PLATFORMS
    {
        hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, wallJumpRaycastRange, groundLayer);

        if (hit.collider != null && hit.collider.CompareTag("StickyTile") && wallJumpAvailable)
        {
            onWall = true;

            if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
                joint2D.connectedBody = hit.collider.GetComponent<Rigidbody2D>();
            joint2D.enabled = true;
        }
        else
        {
            onWall = false;
            wallJumpAvailable = true;
            joint2D.enabled = false;
            joint2D.connectedBody = null;

        }
    }

    public void Die() // KILLS THE PLAYER IF CALLED
    {
        // SETS THE PLAYER DEAD ATTRIBUTE FOR OTHER CLASSES
        playerDead = true;
        if (LevelController.instance != null)
            LevelController.instance.playerDeathCount++;

        if (!levelCompleted)
        {
            // PLAYER DEATH PARTICLES        
            GetComponentInChildren<ParticleSystem>().Emit(20);

            // MAKES THE PLAYER INVISIBLE AND UNCONTROLLABLE
            HideAndStopPlayer();

            // PLAYER DEATH SOUND
            if (AudioController.instance != null)
            {
                AudioController.instance.PlayPlayerDeath();
                AudioController.instance.PlayDeath();
            }
        }
        // KILLS ALL OTHER PLAYERS, IF THERE ARE ANY
        PlayerController[] otherPlayers = FindObjectsOfType<PlayerController>();
        for (int i = 0; i < otherPlayers.Length; i++)
        {
            if (!otherPlayers[i].GetPlayerDead()) otherPlayers[i].Die();
        }
    }

    private void HideAndStopPlayer() // MAKES THE PLAYER INVISIBLE AND UNCONTROLLABLE FOR DIE() AND LEVEL COMPLETION
    {
        // MAKES PLAYER UNCONTROLLABLE
        SetJumpAxis(0);
        rb2d.simulated = false;

        // MAKES PLAYER INVISIBLE
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].enabled = false;
        }
        GetComponentInChildren<Light>().enabled = false;
    }

    public bool GetPlayerDead() // GETTER FOR THE PLAYER DEAD BOOLEAN
    {
        return playerDead;
    }

    private void OnDrawGizmos() // DISPLAYS THE WALLJUMP RANGE
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * wallJumpRaycastRange);
        Gizmos.DrawWireSphere(transform.position, paintRange);
    }

    public void PaintTiles(GameObject blockPrefab) // PAINTS ALL BLOCKS IN RANGE
    {
        // CHECKS FOR VALID BLOCKS AND PAINT THEM
        Collider2D[] blocks = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), paintRange);
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].GetComponent<TileInteractable>() != null)
            {
                blocks[i].GetComponent<TileInteractable>().PaintTile(blockPrefab);
            }
        }

        // DISPLAYS RANGE INDICATOR
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(DisplayRange());
    }

    private IEnumerator DisplayRange() // DISPLAYS AND FADES THE RANGE INDICATOR
    {
        // MAKES THE RANGE STAY IN PLACE WHEN IT APPEARS
        rangeSprite.gameObject.transform.parent = gameObject.transform;
        rangeSprite.gameObject.transform.position = gameObject.transform.position;
        rangeSprite.gameObject.transform.parent = null;
        rangeSprite.enabled = true;

        // ADJUSTS THE RANGE SPRITE SCALE TO MATCH THE PLAYER RANGE VARIABLE
        rangeSprite.gameObject.transform.localScale = new Vector3((paintRange * 1) / 4.5f, (paintRange * 1) / 4.5f, 1);

        // FADES AWAY THE RANGE SPRITE
        rangeSprite.color = new Color(rangeSprite.color.r, rangeSprite.color.g, rangeSprite.color.b, .75f);
        yield return new WaitForSeconds(0.15f);
        while (rangeSprite.color.a >= 0)
        {
            rangeSprite.color = new Color(rangeSprite.color.r, rangeSprite.color.g, rangeSprite.color.b, rangeSprite.color.a - 0.025f);
            yield return new WaitForSeconds(0.025f);
        }
        rangeSprite.enabled = false;
    }
    #endregion

}