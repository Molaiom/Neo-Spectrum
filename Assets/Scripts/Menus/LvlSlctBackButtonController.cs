using UnityEngine;

public class LvlSlctBackButtonController : MonoBehaviour
{
    private void Update()
    {
        // IF BACK BUTTON IS PRESSED
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (GameController.instance != null)
                GameController.instance.LoadMainMenu();

        }
    }
}
