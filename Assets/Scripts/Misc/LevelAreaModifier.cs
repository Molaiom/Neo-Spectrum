using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// THIS SCRIPT CHANGES THE BACKGROUND IMAGES AND GROUND MATERIALS SO THEY CORRESPOND THE LEVEL'S AREA.
[RequireComponent(typeof(Image))]
public class LevelAreaModifier : MonoBehaviour
{
    [SerializeField]
    private Sprite[] backgroundImage;

    private void Awake()
    {
        // CHECKS FOR GAMECONTROLLER INSTANCE
        if (GameController.instance != null)
        {
            int levelArea = GameController.instance.GetLevelArea();

            SetBackgroundImage(levelArea);
            SetGroundSprite(levelArea);
        }
    }

    private void SetBackgroundImage(int levelArea)
    {
        // IF THE LEVEL AREA IS VALID
        if (levelArea >= 0 && levelArea <= backgroundImage.Length)
        {
            GetComponent<Image>().sprite = backgroundImage[levelArea];

            if(levelArea == 0)
            {
                if(TryGetComponent(out BgColorChange bgColorChange))
                {
                    bgColorChange.enabled = true;
                }
            }
        }
    }

    private void SetGroundSprite(int levelArea)
    {
        GroundTileSprites[] groundTile = FindObjectsOfType<GroundTileSprites>();
        for (int i = 0; i < groundTile.Length; i++)
        {
            if (groundTile[i].spriteFromArea[levelArea] != null)
            {
                groundTile[i].gameObject.GetComponent<SpriteRenderer>().sprite = groundTile[i].spriteFromArea[levelArea];
            }
        }

    }
}
