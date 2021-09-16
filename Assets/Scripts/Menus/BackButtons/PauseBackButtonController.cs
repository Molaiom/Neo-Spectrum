using UnityEngine;

public class PauseBackButtonController : MonoBehaviour
{
    private void Update()
    {
        // IF BACK BUTTON IS PRESSED
        if (Input.GetKeyUp(KeyCode.Escape) && GameController.instance != null)
        {
            GameController.instance.ChangePauseState(false);
            gameObject.SetActive(false);

        }
    }
}
