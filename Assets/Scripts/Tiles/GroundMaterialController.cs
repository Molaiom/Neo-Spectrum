using System.Collections;
using UnityEngine;

public class GroundMaterialController : MonoBehaviour
{
    public Material materialToChange;
    public Material[] areaMaterial;

    private void Start() 
    { 
        SetLevelMaterial();
    }

    private void SetLevelMaterial() // CHANGES THE MATERIAL TO CORRESPOND THE LEVEL'S AREA
    {
        // CHECKS FOR GAMECONTROLLER INSTANCE
        if (GameController.instance != null)
        {
            if(GameController.instance.GetLevelArea() >= 0 && GameController.instance.GetLevelArea() <= areaMaterial.Length)
            {
                materialToChange.CopyPropertiesFromMaterial(areaMaterial[GameController.instance.GetLevelArea() - 1]);
            }
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
