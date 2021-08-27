using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalette : MonoBehaviour
{
    public void Paint(GameObject blockPrefab)
    { 
        if(FindObjectOfType<PlayerController>() != null) // CALL THE PAINT FUNCTION FROM THE PLAYER
        {
            FindObjectOfType<PlayerController>().PaintTiles(blockPrefab);
        }

        if (FindObjectOfType<PlayerColorChange>() != null) // UPDATES PLAYER'S SPRITES COLORS
        {
            FindObjectOfType<PlayerColorChange>().ChangeCurrentColor(blockPrefab);
        }
    }
}
