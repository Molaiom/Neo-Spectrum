using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AutoSelectButtons : MonoBehaviour
{
    public Selectable firstSelected;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        firstSelected.Select();
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        firstSelected.Select();
    }
}
