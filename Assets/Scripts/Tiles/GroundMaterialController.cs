using System.Collections;
using UnityEngine;

public class GroundMaterialController : MonoBehaviour
{
    public Material materialToChange;
    public Material area1Material;
    public Material area2Material;
    public Material area3Material;
    public Material area4Material;

    private void Start()
    {
        SetLevelMaterial();
    }

    private void SetLevelMaterial()
    {
        materialToChange.renderQueue = 3000;        

        // CHECKS FOR GAMECONTROLLER INSTANCE
        if (GameController.instance != null)
        {
            if(GameController.instance.GetLevelNumber() != 0)
            {
                // SETS AREA 1 MATERIAL
                if (GameController.instance.GetLevelNumber() <= 5)
                {
                    materialToChange.CopyPropertiesFromMaterial(area1Material);
                }
                // SETS AREA 2 MATERIAL
                else if (GameController.instance.GetLevelNumber() <= 12)
                {
                    materialToChange.CopyPropertiesFromMaterial(area2Material);
                }
                // SETS AREA 3 MATERIAL
                else if (GameController.instance.GetLevelNumber() <= 20)
                {
                    materialToChange.CopyPropertiesFromMaterial(area3Material);
                }
                // SETS AREA 4 MATERIAL
                else
                {
                    materialToChange.CopyPropertiesFromMaterial(area4Material);
                }
            }            
        }        
    }

    private void OnApplicationQuit()
    {
        materialToChange.CopyPropertiesFromMaterial(area1Material);
    }
}
