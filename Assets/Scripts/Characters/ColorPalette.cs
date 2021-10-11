using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalette : MonoBehaviour
{
    [SerializeField]
    private GameObject[] blocks = new GameObject[3];
    [SerializeField]
    private GameObject[] buttons = new GameObject[3];
    private bool[] isInputPressed = new bool[3];

    private void Update() // KEYBOARD INPUT
    {
        if (!Application.isMobilePlatform)
        {
            // MADE SO THE PLAYER ONLY PAINTS ON THE FIRST FRAME A INPUT IS GIVEN FOR EACH COLOR
            // RED
            if (Input.GetAxisRaw("Red") > 0 && buttons[0].activeSelf && !isInputPressed[0])
            {
                Paint(blocks[0]);
                isInputPressed[0] = true;
            }
            else if (Input.GetAxisRaw("Red") <= 0)
                isInputPressed[0] = false;

            // GREEN
            if (Input.GetAxisRaw("Green") > 0 && buttons[1].activeSelf && !isInputPressed[1])
            {
                Paint(blocks[1]);
                isInputPressed[1] = true;
            }
            else if (Input.GetAxisRaw("Green") <= 0)
                isInputPressed[1] = false;

            //BLUE
            if (Input.GetAxisRaw("Blue") > 0 && buttons[2].activeSelf && !isInputPressed[2])
            {
                Paint(blocks[2]);
                isInputPressed[2] = true;
            }
            else if (Input.GetAxisRaw("Blue") <= 0)
                isInputPressed[2] = false;
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
                if (!players[i].GetPlayerDead() && !players[i].levelCompleted) players[i].PaintTiles(blockPrefab);
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
