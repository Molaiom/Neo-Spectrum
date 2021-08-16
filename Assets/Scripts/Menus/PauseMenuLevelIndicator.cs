using UnityEngine;
using UnityEngine.UI;

public class PauseMenuLevelIndicator : MonoBehaviour
{
    private Text levelText;

    private void Start()
    {
        if(GetComponent<Text>() != null)
        {
            levelText = GetComponent<Text>();

            if (GameController.instance != null)
            {
                if (GameController.instance.GetLevelNumber() != 0)
                {
                    levelText.text = "Level " + GameController.instance.GetLevelNumber().ToString();

                }
            }
        }        
    }
}
