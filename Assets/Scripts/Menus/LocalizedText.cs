using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour
{
    [Tooltip("Is this text supposed to be different on mobile / console platforms? If not, Pc text will be used.")]
    public bool platformBasedText;

    public string englishDefaultText;
    public string englishMobileText;
    public string portugueseDefaultText;
    public string portugueseMobileText;
    public string spanishDefaultText;
    public string spanishMobileText;

    private enum e_currentPlatform { Pc, Mobile }
    private e_currentPlatform currentPlatform;
    private Text myText;

    private void Awake()
    {
        myText = GetComponent<Text>();
        SetCurrentPlatform();
        SetText();
    }

    private void SetText()
    {
        switch (GameController.instance.Language)
        {
            // ENGLISH
            case 0: default:
                if (!platformBasedText || currentPlatform == e_currentPlatform.Pc)
                    myText.text = englishDefaultText;

                else if (currentPlatform == e_currentPlatform.Mobile)
                    myText.text = englishMobileText;
                break;

            // PORTUGUESE / BRAZILIAN
            case 1:
                if (!platformBasedText || currentPlatform == e_currentPlatform.Pc)
                    myText.text = portugueseDefaultText;

                else if (currentPlatform == e_currentPlatform.Mobile)
                    myText.text = portugueseMobileText;
                break;

            // SPANISH
            case 2:
                if (!platformBasedText || currentPlatform == e_currentPlatform.Pc)
                    myText.text = spanishDefaultText;

                else if (currentPlatform == e_currentPlatform.Mobile)
                    myText.text = spanishMobileText;
                break;
        }
    }

    private void SetCurrentPlatform()
    {
#if UNITY_STANDALONE
        currentPlatform = e_currentPlatform.Pc;

#elif UNITY_ANDROID
        currentPlatform = e_currentPlatform.Mobile;
#endif
    }
}
