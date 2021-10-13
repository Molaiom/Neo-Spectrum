using UnityEngine;

public class ExtrasBackButtonController : MonoBehaviour
{
    public GameObject[] images = new GameObject[4];

    private void Update()
    {
        // IF BACK BUTTON IS PRESSED
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            bool imageActive = false;

            for (int i = 0; i < images.Length; i++)
            {
                if (images[i].activeSelf)
                {
                    imageActive = true;
                    images[i].SetActive(false);
                }
                else if (!imageActive && GameController.instance != null)
                {
                    GameController.instance.LoadMainMenu();
                }
            }
        }
    }

}
