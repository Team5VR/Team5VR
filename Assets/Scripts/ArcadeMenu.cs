using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ArcadeMenu : MonoBehaviour
{
    public GameObject arrowStart;
    public GameObject arrowTutorial;
    public GameObject arrowQuit;

    // Start is called before the first frame update
    void Start()
    {
        arrowStart.SetActive(false);
        arrowTutorial.SetActive(false);
        arrowQuit.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        // SceneManager.LoadScene(GAME)
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartArrowEnable()
    {
        arrowStart.SetActive(true);
    }

    public void StartArrowDisable()
    {
        arrowStart.SetActive(false);
    }

    public void TutorialArrowEnable()
    {
        arrowTutorial.SetActive(true);
    }

    public void TutorialArrowDisable()
    {
        arrowTutorial.SetActive(false);
    }

    public void QuitArrowEnable()
    {
        arrowQuit.SetActive(true);
    }

    public void QuitArrowDisable()
    {
        arrowQuit.SetActive(false);
    }
}
