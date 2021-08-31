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
        if (PlayerController.instance != null)
        {
            PlayerController.instance.SetMovementAxis(value);
        }

        if (value != 0)
            image.color = new Color32(142, 142, 142, 255);
        else
            image.color = new Color32(255, 255, 255, 255);
    }

    public void JumpAxisInput(float value)
    {
        if (PlayerController.instance != null)
        {
            PlayerController.instance.SetJumpAxis(value);
        }

        if (value != 0)
            image.color = new Color32(142, 142, 142, 255);
        else
            image.color = new Color32(255, 255, 255, 255);
    }
}
