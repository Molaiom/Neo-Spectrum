using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalette : MonoBehaviour
{
    [SerializeField]
    private GameObject[] blocks = new GameObject[3];

    private void Update() // KEYBOARD INPUT
    {
        if(!Application.isMobilePlatform)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1)) { Paint(blocks[0]); }
            if(Input.GetKeyDown(KeyCode.Alpha2)) { Paint(blocks[1]); }
            if(Input.GetKeyDown(KeyCode.Alpha3)) { Paint(blocks[2]); }
        }
    }

    public void Paint(GameObject blockPrefab)
    {
        // CALLS THE PAINT FUNCTION FROM ALL PLAYERS
        if (FindObjectsOfType<PlayerController>() != null)
        {
            PlayerController[] players = FindObjectsOfType<PlayerController>();
            for (int i = 0; i < players.Length; i++)
            {
                if(!players[i].GetPlayerDead() && !players[i].levelCompleted) players[i].PaintTiles(blockPrefab);
            }
        }

        // UPDATES PLAYER'S SPRITE COLORS
        if (FindObjectsOfType<PlayerColorChange>() != null)
        {
            PlayerColorChange[] playerColors = FindObjectsOfType<PlayerColorChange>();
            for (int i = 0; i < playerColors.Length; i++)
            {
                playerColors[i].ChangeCurrentColor(blockPrefab);
            }
        }
    }
}
