using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalette : MonoBehaviour
{
    #region Atributtes
    public static ColorPalette instance;

    [Header("Colors")]
    public GameObject whitePrefab;
    public GameObject redPrefab;
    public GameObject greenPrefab;
    public GameObject bluePrefab;    

    private GameObject currentPrefab;
    #endregion

    #region Methods
    private void Awake()
    {
        instance = this;
        currentPrefab = whitePrefab;
    }

    public GameObject GetCurrentColor()
    {
        return currentPrefab;
    }
    #endregion

    #region Buttons
    public void ChangeColorToRed()
    {
        currentPrefab = redPrefab;
        UpdatePlayerColor();        
    }

    public void ChangeColorToGreen()
    {
        currentPrefab = greenPrefab;
        UpdatePlayerColor();        
    }

    public void ChangeColorToBlue()
    {
        currentPrefab = bluePrefab;
        UpdatePlayerColor();        
    }

    private void UpdatePlayerColor()
    {
        if(FindObjectOfType<PlayerColorChange>() != null)
        {
            FindObjectOfType<PlayerColorChange>().ChangeCurrentColor();            
        }
    }
    #endregion
}
