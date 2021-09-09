using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    // END OF LEVEL IMPLEMENTATIONS (HUD / COLLECTABLE / SAVE)
    // PLAYER DEATH IMPLEMENTATIONS (HUD)
    [SerializeField] 
    private GameObject deathText;
    private PlayerController[] players;

    private void Start()
    {
        if(FindObjectsOfType<PlayerController>() != null) players = FindObjectsOfType<PlayerController>();
    }
}
