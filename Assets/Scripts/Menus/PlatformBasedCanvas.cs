using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBasedCanvas : MonoBehaviour
{
    public GameObject[] mobileHUD;
    public GameObject[] PCHUD;


    private void Awake()
    {
#if UNITY_STANDALONE
        foreach(GameObject o in mobileHUD)
        {
            o.SetActive(false);
        }
        foreach(GameObject o in PCHUD)
        {
            o.SetActive(true);
        }

#elif UNITY_ANDROID
        foreach (GameObject o in mobileHUD)
        {
            o.SetActive(true);
        }
        foreach(GameObject o in PCHUD)
        {
            o.SetActive(false);
        }

#endif
    }
}
