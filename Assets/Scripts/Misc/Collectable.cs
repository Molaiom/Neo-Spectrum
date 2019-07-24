using UnityEngine;

public class Collectable : MonoBehaviour
{
    private void Start() // DESTROYS THE COLLECTABLE IF THE PLAYER ALREADY HAS IT
    {
        if(GameController.instance != null)
        {
            int levelNumber = GameController.instance.GetLevelNumber();

            if(GameController.instance.GetLevelNumber() != 0)
            {
                if (GameController.instance.CollectableFromLevel[levelNumber - 1])
                {
                    Destroy(gameObject);
                }
            }            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // MAKES THE PLAYER COLLECT IT
    {
        if(collision.GetComponent<PlayerController>() != null)
        {
            if(AudioController.instance != null)
            {
                AudioController.instance.PlayCollectableCollected();
            }
            PlayerController player = collision.GetComponent<PlayerController>();
            player.collectable = true;
            Destroy(gameObject);
        }
    }
}
