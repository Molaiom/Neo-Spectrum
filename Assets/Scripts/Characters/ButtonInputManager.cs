using UnityEngine;
using UnityEngine.UI;

public class ButtonInputManager : MonoBehaviour
{
    private Image image;
    PlayerController[] players;

    private void Awake()
    {
        image = GetComponent<Image>();
        Input.simulateMouseWithTouches = false;
    }

    private void Start()
    {
        if (FindObjectsOfType<PlayerController>() != null)
        {
            players = FindObjectsOfType<PlayerController>();
        }
    }

    public void HorizontalAxisInput(float value)
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetMovementAxis(value);
        }

        if (value != 0)
            image.color = new Color32(142, 142, 142, 255);
        else
            image.color = new Color32(255, 255, 255, 255);
    }

    public void JumpAxisInput(float value)
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetJumpAxis(value);
        }

        if (value != 0)
            image.color = new Color32(142, 142, 142, 255);
        else
            image.color = new Color32(255, 255, 255, 255);
    }
}
