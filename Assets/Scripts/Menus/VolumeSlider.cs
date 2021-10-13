using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Slider))]
public class VolumeSlider : MonoBehaviour
{
    public enum VolumeType { Effects, Music};
    public VolumeType type;


    public void OnEnable()
    {
        if(GameController.instance != null)
        {
            if (type == VolumeType.Effects)
            {
                GetComponent<Slider>().value = GameController.instance.VolumeEffects;
            }
            else if (type == VolumeType.Music)
            {
                GetComponent<Slider>().value = GameController.instance.VolumeMusic;
            }
        }
    }
}
