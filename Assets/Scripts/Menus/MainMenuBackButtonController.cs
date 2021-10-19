using UnityEngine;
using UnityEngine.UI;

public class MainMenuBackButtonController : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject creditsCanvas;
    public GameObject settingsCanvas;
    public GameObject resetDataMenu;
    

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

            if(settingsCanvas.activeSelf)
            {
                if(resetDataMenu.activeSelf)
                {
                    resetDataMenu.SetActive(false);
                }
                else
                {
                    settingsCanvas.SetActive(false);
                    menuCanvas.SetActive(true);
                }
                
            }
        }
    }
}
