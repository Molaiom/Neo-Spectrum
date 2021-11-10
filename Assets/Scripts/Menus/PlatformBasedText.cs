using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PlatformBasedText : MonoBehaviour
{
    public string PcText;
    public string MobileText;

    void Start()
    {
        if(GameController.instance != null)
        {
            // CHANGES TO PC TEXT
            if (GameController.instance.targetPlatform == GameController.e_TargetPlatform.Pc)
                GetComponent<Text>().text = PcText;

            // CHANGES TO MOBILE TEXT
            if (GameController.instance.targetPlatform == GameController.e_TargetPlatform.Mobile)
                GetComponent<Text>().text = MobileText;
        }
    }
}
