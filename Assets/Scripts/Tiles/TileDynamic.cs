using System.Collections.Generic;
using UnityEngine;

public class TileDynamic : TileInteractable
{
    #region Attributes
    public GameObject whiteTilePrefab;
    public float smoothFallMultiplier = 1;
    private Vector3 startingPosition;
    private Rigidbody2D rb2d;
    private Color redTileColor = new Color32(210, 54, 54, 255);

    private bool canMakeImpactSound = true;
    private float airTime;
    #endregion

    #region Methods
    protected override void Awake() // ASSIGN COMPONENTS
    {
        base.Awake();
        startingPosition = transform.position;
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        SmoothFall();

        // DESTROYS THE BLOCK IF OUT OF BOUNDS
        if (transform.position.y < -5.5f || transform.position.y > 8.5f || transform.position.x < -12 || transform.position.x > 12)
        {
            if(!destroyed)
                DestroyTile(redTileColor);
        }
    }

    private void SmoothFall() // MAKES THE BLOCK FALL FASTER
    {
        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * smoothFallMultiplier * Time.deltaTime;
            airTime += Time.deltaTime;
            if (airTime >= 0.25f)
                canMakeImpactSound = true;
        }
        else
        {
            airTime = 0;
        }
    }
    #endregion

    #region Collisions
    private List<GameObject> allCollisions = new List<GameObject>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // IF THE BLOCK ENTERS CONTACT WITH THE "Ground" LAYER
        if (collision.gameObject.layer == 9)
        {
            if (allCollisions.Contains(collision.gameObject)) return;
            else
            {
                allCollisions.Add(collision.gameObject);

                // IF THE BLOCK COLLIDES WITH THE "Ground" LAYER, PLAY A SOUND
                if (canMakeImpactSound && AudioController.instance != null && airTime >= 0.22f)
                {
                    bool isTouchingBouncyTile = false;
                    for (int i = 0; i < allCollisions.Count; i++)
                    {
                        if (allCollisions[i].CompareTag("BouncyTile")) isTouchingBouncyTile = true;
                    }
                    
                    if(isTouchingBouncyTile)
                    {
                        AudioController.instance.PlayBounce();
                    }
                    else if (IsGrounded() || (rb2d.velocity.x > 0.1f || rb2d.velocity.x < -0.1f))
                    {
                        AudioController.instance.PlayBlockImpact();
                    }
                }
            }            

            canMakeImpactSound = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // IF THE BLOCK EXITS COLLISION WITH THE "GROUND" LAYER, MAKES SO IT CAN PLAY A SOUND
        if (collision.gameObject.layer == 9 && !rb2d.IsTouchingLayers(9))
        {
            canMakeImpactSound = true;

            if(allCollisions.Contains(collision.gameObject))
                allCollisions.Remove(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) // IF THE DYNAMIC TILE TOUCHES A WHITE TILE
    {
        if (collision.gameObject.CompareTag("WhiteTile"))
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().Sleep();
            if (collision.gameObject.GetComponent<TileInteractable>() != null)
                collision.gameObject.GetComponent<TileInteractable>().FlashCoroutine();
            GameObject newTile = Instantiate(whiteTilePrefab, startingPosition, transform.rotation);
            newTile.GetComponent<TileInteractable>().FlashCoroutine();
            Destroy(gameObject);
        }
    }

    private bool IsGrounded()
    {
        bool result = false;

        Vector2 position = new Vector2(transform.position.x, transform.position.y - 0.3f);
        Vector2 size = new Vector2(0.9f, 1f);
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(position, size, 0, LayerMask.GetMask("Ground"));

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].transform != transform)
            {
                result = true;
            }
        }

        return result;
    }
    #endregion
}
