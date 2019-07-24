using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : CharacterPhysics
{
    #region Attributes
    [Header("Player Ranges")]
    public float wallJumpRaycastRange;
    public GameObject rangeIndicator;
    [HideInInspector]
    public bool PC;
    [HideInInspector]
    public bool collectable = false;

    public static PlayerController instance;

    private bool playerDead = false;
    private RaycastHit2D hit;
    private Animator anim;
    #endregion


    #region Methods
    protected override void Awake()
    {
        base.Awake();
        instance = this;
        anim = GetComponent<Animator>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // KILLS THE PLAYER IF OUT OF BOUNDS
        if(transform.position.y < -5.5f || transform.position.y > 8.5f || transform.position.x < -12 || transform.position.x > 12)
        {
            if(!playerDead)
                Die();
        }
    }

    private void Update() // INPUTS AND ANIMATIONS
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            PC = true;



        // KEYBOARD INPUTS (FOR TESTING ONLY)
        if (PC)
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
        if (GameController.instance != null)
        {
            // END OF THE LEVEL
            if (collision.gameObject.CompareTag("Finish"))
            {
                int levelNumber = GameController.instance.GetLevelNumber();

                // SAVES THE LEVEL COMPLETION 
                if (levelNumber > 0)
                {
                    if (levelNumber > GameController.instance.NumberOfLevelsCompleted)
                    {
                        GameController.instance.NumberOfLevelsCompleted = levelNumber;
                    }

                    if (collectable)
                    {
                        bool[] newArray = GameController.instance.CollectableFromLevel;
                        newArray[levelNumber - 1] = true;
                        GameController.instance.CollectableFromLevel = newArray;
                    }
                }

                // OPENS THE "LEVEL COMPLETED" SCREEN
                if (FindObjectOfType<LevelCompleted>() != null)
                {
                    StartCoroutine(FindObjectOfType<LevelCompleted>().OpenLevelCompletedMenu());

                    rb2d.simulated = false;
                    SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
                    for (int i = 0; i < sprites.Length; i++)
                    {
                        sprites[i].enabled = false;
                    }
                    GetComponentInChildren<Light>().enabled = false;

                }
                else
                    GameController.instance.LoadLevelSelect();
            }

        }
        else
        {
            Debug.LogWarning("No GameController instance found in the scene!");
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
        // AUDIO ------------------
        if (AudioController.instance != null)
            AudioController.instance.PlayDeathSound();

        rb2d.simulated = false;
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].enabled = false;
        }
        GetComponentInChildren<Light>().enabled = false;
        GetComponentInChildren<ParticleSystem>().Emit(20);

        playerDead = true;
    }

    public bool GetPlayerDead() // GETTER FOR THE PLAYER DEAD BOOLEAN
    {
        return playerDead;
    }

    private void OnDrawGizmos() // DISPLAYS THE WALLJUMP RANGE
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * wallJumpRaycastRange);
    }
    #endregion

}