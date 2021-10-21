using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AutoSelectButtons : MonoBehaviour
{
    public Selectable firstSelected;

    private void Start()
    {
        StopAllCoroutines();
        StartCoroutine(SelectRoutine());
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(SelectRoutine());
    }

    private IEnumerator SelectRoutine()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
    }
}
