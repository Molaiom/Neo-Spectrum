using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgImageChange : MonoBehaviour
{
    [SerializeField]
    private Sprite[] backgroundImage = new Sprite[4];

    private void Start()
    {
        AssignBackgroundImage();
    }

    private void AssignBackgroundImage() // THIS CHANGES THE BACKGROUND IMAGE TO CORREPOND THE AREA 
    {
        if (GameController.instance != null)
        {
            if (GameController.instance.GetLevelNumber() < 6)
            {
                GetComponent<Image>().sprite = backgroundImage[0];
            }
            else if (GameController.instance.GetLevelNumber() < 13)
            {
                GetComponent<Image>().sprite = backgroundImage[1];
            }
            else if (GameController.instance.GetLevelNumber() < 21)
            {
                GetComponent<Image>().sprite = backgroundImage[2];
            }
            else
            {
                GetComponent<Image>().sprite = backgroundImage[3];
            }
        }
    }
}
