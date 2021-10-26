using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuBackButtonController : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject creditsCanvas;
    public GameObject settingsCanvas;
    public GameObject resetDataMenu;
    public GameObject gameQuitMenu;
    public GameObject quitMenuButton;
    

    private void Update()
    {
        // IF BACK BUTTON IS PRESSED
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(gameQuitMenu.activeSelf)
            {
                CloseQuitMenu();
            }

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

    public void CloseQuitMenu()
    {
        gameQuitMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(quitMenuButton);
    }
}
