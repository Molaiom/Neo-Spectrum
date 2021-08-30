﻿using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class PlayerController : CharacterPhysics
{
    #region Attributes
    [Header("Player Ranges")]
    public float wallJumpRaycastRange;
    public SpriteRenderer rangeSprite;
    [HideInInspector]
    public bool PC;
    [HideInInspector]
    public bool collectable = false;

    public static PlayerController instance;

    private bool playerDead = false;
    public bool levelCompleted = false;
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
                levelCompleted = true;

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

                    if (AudioController.instance != null)
                        AudioController.instance.PlayLevelCompleted();
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

    private void OnCollisionEnter2D(Collision2D collision) // BOUNCY SOUND WHEN JUMPING INTO THE BOUNCY TILE
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y + groundCheckVerticalOffset * 2f);
        Vector2 size = new Vector2(groundRadius, groundRadius * 1.7f);
        Collider2D blockBellow = Physics2D.OverlapBox(position, size, 0, groundLayer);

        if(blockBellow != null)
        {            
            if (blockBellow.gameObject.CompareTag("BouncyTile") && !BlockAboveHead() && AudioController.instance != null)
            {
                AudioController.instance.PlayBounce();                
            }
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
        if(AudioController.instance != null)
        {
            AudioController.instance.PlayPlayerDeath();
            AudioController.instance.PlayDeath();
        }
        SetJumpAxis(0);
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
    
    public void PaintTiles(GameObject blockPrefab) // PAINTS ALL BLOCKS IN RANGE
    {
        // CHECKS FOR VALID BLOCKS AND PAINT THEM
        Collider2D[] blocks = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 4.5f);
        for (int i = 0; i < blocks.Length; i++)
        {
            if(blocks[i].GetComponent<TileInteractable>() != null)
            {
                blocks[i].GetComponent<TileInteractable>().PaintTile(blockPrefab);
            }
        }

        // DISPLAYS RANGE INDICATOR
        StartCoroutine(DisplayRange());
    }

    private IEnumerator DisplayRange()
    {        
        rangeSprite.color = new Color(rangeSprite.color.r, rangeSprite.color.g, rangeSprite.color.b, .75f);
        rangeSprite.enabled = true;

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