using UnityEngine;

public class EnemyController : CharacterPhysics
{
    #region Attributes
    [Range(-15,0)]
    public float leftRange;
    [Range(0, 15)]
    public float rightRange;

    private float leftLimit;
    private float rightLimit;
    private float direction = 1;
    private float turnTimer;
    private ParticleSystem particles;
    #endregion

    #region Methods
    private void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        leftLimit = transform.position.x + leftRange;
        rightLimit = transform.position.x + rightRange;        
    }

    private void Update()
    {
        SetMovementAxis(direction);

        turnTimer -= 1 * Time.deltaTime;
        Mathf.Clamp(turnTimer, 0, Mathf.Infinity);

        if(transform.position.x >= rightLimit || transform.position.x <= leftLimit)
        {
            if (turnTimer <= 0)
            {
                turnTimer = 0.5f;
                direction *= -1;
            }
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision) // DESTROYS THE PLAYER OR RED TILES
    {
        if (collision.gameObject.GetComponent<TileInteractable>() != null)
        {
            TileInteractable tile = collision.gameObject.GetComponent<TileInteractable>();

            switch (collision.tag)
            {
                case "StickyTile":                    
                    tile.DestroyTile(new Color32(41, 82, 204, 255));
                    break;

                case "BouncyTile":
                    tile.DestroyTile(new Color32(153, 229, 80, 255));                    
                    break;

                case "DynamicTile":
                    tile.DestroyTile(new Color32(210, 54, 54, 255));                    
                    break;                
            }
        }
        else if(collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerController>() != null)
        {
            collision.gameObject.GetComponent<PlayerController>().Die();
        }
    }

    private void OnDrawGizmos() // DISPLAYS THE WALK RANGE
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * rightRange);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * -leftRange);
    }
    
    public void Die()
    {
        particles.Play();
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponentInChildren<Light>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        rb2d.simulated = false;

        Destroy(gameObject, 2);
    }
    #endregion
}
