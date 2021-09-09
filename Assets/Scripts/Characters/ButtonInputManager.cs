using UnityEngine;
using UnityEngine.UI;

public class ButtonInputManager : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        Input.simulateMouseWithTouches = false;
    }

    public void HorizontalAxisInput(float value)
    {
        if (FindObjectsOfType<PlayerController>() != null)
        {
            PlayerController[] players = FindObjectsOfType<PlayerController>();
            for (int i = 0; i < players.Length; i++)
            {
                players[i].SetMovementAxis(value);
            }
        }

        if (value != 0)
            image.color = new Color32(142, 142, 142, 255);
        else
            image.color = new Color32(255, 255, 255, 255);
    }

    public void JumpAxisInput(float value)
    {
        if (FindObjectsOfType<PlayerController>() != null)
        {
            PlayerController[] players = FindObjectsOfType<PlayerController>();
            for (int i = 0; i < players.Length; i++)
            {
                players[i].SetJumpAxis(value);
            }
        }

        if (value != 0)
            image.color = new Color32(142, 142, 142, 255);
        else
            image.color = new Color32(255, 255, 255, 255);
    }
}
