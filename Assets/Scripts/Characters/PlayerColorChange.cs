﻿using System.Collections;
using UnityEngine;

public class PlayerColorChange : MonoBehaviour
{
    private Color currentColor = Color.white;
    public SpriteRenderer heartSprite;
    public SpriteRenderer rangeSprite;
    public Light playerLight;

    private float velocity = 0;


    private void LateUpdate()
    {
        FadePlayerColor();
    }

    public void ChangeCurrentColor(GameObject blockPrefab)
    {
        switch (blockPrefab.tag)
        {
            case "StickyTile":
                currentColor = new Color32(41, 82, 204, 255);
                break;

            case "BouncyTile":
                currentColor = new Color32(153, 229, 80, 255);
                break;

            case "DynamicTile":
                currentColor = new Color32(210, 54, 54, 255);
                break;

            default:
                currentColor = Color.white;
                break;
        }        

        if (currentColor != rangeSprite.color)
        {
            rangeSprite.color = new Color(currentColor.r, currentColor.g, currentColor.b, rangeSprite.color.a);
        }
    }
    
    private void FadePlayerColor() // SLOWLY FADES THE HEART AND LIGHT COLORS
    {
        float R = playerLight.color.r;
        float G = playerLight.color.g;
        float B = playerLight.color.b;

        R = Mathf.SmoothDamp(R, currentColor.r, ref velocity, 0.1f, 1, Time.deltaTime * 5);
        G = Mathf.SmoothDamp(G, currentColor.g, ref velocity, 0.1f, 1, Time.deltaTime * 5);
        B = Mathf.SmoothDamp(B, currentColor.b, ref velocity, 0.1f, 1, Time.deltaTime * 5);

        if (currentColor != heartSprite.color)
        {
            heartSprite.color = new Color(R, G, B, heartSprite.color.a);
        }

        if (currentColor != playerLight.color)
        {
            playerLight.color = new Color(R, G, B, playerLight.color.a);
        }
    }
}