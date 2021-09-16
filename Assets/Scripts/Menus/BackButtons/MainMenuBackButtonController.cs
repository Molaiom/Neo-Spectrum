using UnityEngine;

public class MainMenuBackButtonController : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject creditsCanvas;
    

    private void Update()
    {
        // IF BACK BUTTON IS PRESSED
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(creditsCanvas.activeSelf)
            {
                creditsCanvas.SetActive(false);
                menuCanvas.SetActive(true);
            }
        }
    }
}
