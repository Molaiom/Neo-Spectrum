using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PlatformBasedText : MonoBehaviour
{
    public string PcText;
    public string MobileText;

    void Start()
    {
        if (GameController.instance != null)
        {
#if UNITY_STANDALONE
            // CHANGES TO PC TEXT
            GetComponent<Text>().text = PcText;
#elif UNITY_ANDROID
            // CHANGES TO MOBILE TEXT
            GetComponent<Text>().text = MobileText;
#endif
        }
    }
}
