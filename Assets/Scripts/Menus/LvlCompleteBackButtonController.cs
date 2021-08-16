using UnityEngine;

public class LvlCompleteBackButtonController : MonoBehaviour
{
    private void Update()
    {
        // IF BACK BUTTON IS PRESSED
        if (Input.GetKeyUp(KeyCode.Escape) && GameController.instance != null)
        {
            GameController.instance.LoadLevelSelect();

        }
    }
}
