using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// THIS SCRIPT CHANGES THE BACKGROUND IMAGES AND GROUND MATERIALS SO THEY CORRESPOND THE LEVEL'S AREA.
[RequireComponent(typeof(Image))]
public class LevelAreaModifier : MonoBehaviour
{
    [Header("Background images")]
    [SerializeField]
    private Sprite[] backgroundImage;

    [Header("Ground materials")]
    [SerializeField]
    private Material materialToChange;
    [SerializeField]
    private Material[] areaMaterial;


    private void Start()
    {
        // CHECKS FOR GAMECONTROLLER INSTANCE
        if (GameController.instance != null)
        {
            int levelArea = GameController.instance.GetLevelArea();

            // IF THE LEVEL AREA IS VALID
            if (levelArea >= 0 && levelArea <= areaMaterial.Length && levelArea <= areaMaterial.Length)
            {
                //print(backgroundImage[levelArea].name + ", " + areaMaterial[levelArea].name);
                materialToChange.CopyPropertiesFromMaterial(areaMaterial[GameController.instance.GetLevelArea()]);
                GetComponent<Image>().sprite = backgroundImage[levelArea];
            }
            // IF THE LEVEL AREA ISN'T VALID, SET A DEFAULT MATERIAL AND IMAGE
            else
            {
                materialToChange.CopyPropertiesFromMaterial(areaMaterial[0]);
            }
        }
    }

    private void OnApplicationQuit()
    {
        materialToChange.CopyPropertiesFromMaterial(areaMaterial[0]);
    }
}
