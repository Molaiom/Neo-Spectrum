using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletedScreen : MonoBehaviour
{
    private Image blankImage;
    private GameObject levelCompletedPanel;
    private ParticleSystem particles;
    private bool isScreenOpen;
    [SerializeField]
    private GameObject newExtraUnlockedObj;
    public float timeToOpen = 1;

    private void Awake() // ASSIGN COMPONENTS
    {
        blankImage = GetComponent<Image>();
        levelCompletedPanel = transform.GetChild(0).gameObject;
        particles = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
    }

    private void Update() // INPUTS
    {
        if (isScreenOpen && (Input.GetAxisRaw("Submit")) > 0 && GameController.instance != null)
        {
            GameController.instance.LoadNextLevel();
        }

        // IF BACK BUTTON IS PRESSED
        if (Input.GetKeyUp(KeyCode.Escape) && GameController.instance != null)
        {
            GameController.instance.LoadLevelSelect();

        }
    }

    public void OpenLevelCompletedMenu() // CALLS THE OPEN LEVEL COMPLETED COROUTINE
    {
        if (!isScreenOpen)
            StartCoroutine(OpenLevelCompletedMenuRoutine());
    }

    public void ShowNewExtraUnlockedText()
    {
        newExtraUnlockedObj.SetActive(true);
    }


    private IEnumerator OpenLevelCompletedMenuRoutine() // ANIMATES THE LEVEL COMPLETED SCREEN
    {
        isScreenOpen = true;
        blankImage.enabled = true;
        blankImage.CrossFadeAlpha(0, 0, true);
        blankImage.CrossFadeAlpha(1, 0.5f, true);

        yield return new WaitForSeconds(0.25f);
        OpenPanel(0.6f);
        StartCoroutine(AnimatePanel());

        yield return new WaitForSeconds(0.25f);
        EmitParticles();

        yield return new WaitForSeconds(0.5f);
        EmitParticles();
    }

    private void EmitParticles() // GETS THE PARTICLE SYSTEM COMPONENT, DEPARENT IT, SETS IT TO THE CENTER OF THE SCREEN AND ACTIVATES IT
    {
        particles.transform.SetParent(null);
        particles.transform.position = new Vector3(0, 0, -10);
        particles.transform.localScale = new Vector3(1, 1, 1);

        particles.Emit(40);
    }

    private void OpenPanel(float timeToFadeIn) // FADES IN ALL IMAGES AND TEXTS IN THE PANEL
    {
        // FADE IN THE PANEL ITSELF
        levelCompletedPanel.SetActive(true);
        levelCompletedPanel.GetComponent<Image>().CrossFadeAlpha(0, 0, true);
        levelCompletedPanel.GetComponent<Image>().CrossFadeAlpha(1, timeToFadeIn, true);

        // GET ALL IMAGES / BUTTONS
        Image[] childImages = new Image[levelCompletedPanel.transform.childCount];

        for (int i = 0; i < childImages.Length; i++)
        {
            if (levelCompletedPanel.transform.GetChild(i).GetComponent<Image>() != null)
                childImages[i] = levelCompletedPanel.transform.GetChild(i).GetComponent<Image>();
        }

        // FADE IN IMAGES / BUTTONS
        for (int i = 0; i < childImages.Length; i++)
        {
            if (childImages[i] != null)
            {
                childImages[i].CrossFadeAlpha(0, 0, true);
                childImages[i].CrossFadeAlpha(1, timeToFadeIn, true);
            }
        }

        // GET ALL TEXTS
        Text[] childTexts = new Text[levelCompletedPanel.transform.childCount];

        for (int i = 0; i < childTexts.Length; i++)
        {
            if (levelCompletedPanel.transform.GetChild(i).GetComponent<Text>() != null)
                childTexts[i] = levelCompletedPanel.transform.GetChild(i).GetComponent<Text>();
        }

        // FADE IN TEXT
        for (int i = 0; i < childTexts.Length; i++)
        {
            if (childTexts[i] != null)
            {
                childTexts[i].CrossFadeAlpha(0, 0, true);
                childTexts[i].CrossFadeAlpha(1, timeToFadeIn, true);
            }
        }
    }

    private IEnumerator AnimatePanel()
    {
        float actualScale = 0.2f;

        levelCompletedPanel.transform.localScale = new Vector3(actualScale, actualScale, actualScale);

        for (float f = 0; f < 1; f += Time.deltaTime / timeToOpen)
        {
            actualScale = actualScale >= 1 ? 1 : actualScale += (0.025f * 1 / timeToOpen);
            levelCompletedPanel.transform.localScale = new Vector3(actualScale, actualScale, actualScale);
            yield return null;
        }

        levelCompletedPanel.transform.localScale = new Vector3(1, 1, 1);
    }
}
