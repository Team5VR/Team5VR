using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    // Application Open
    GameObject m_player;
    GameObject m_sicknessWarning;
    [SerializeField]
    float m_sicknessTime;    
    GameObject m_surroundingsWarning;
    [SerializeField]
    float m_surroundingsTime;

    // Menu Area    
    GameObject m_arcadeMenu;
    GameObject m_tutorialObjects;    
    GameObject m_startArea;
    [SerializeField]
    GameObject m_grandstand;
    Transform m_resetSpawn;
    [SerializeField]
    GameObject m_endScoreboard;
    [SerializeField]
    GameObject m_newHighScore;

    // Game Loop Area
    [SerializeField]
    Image m_countdown;
    [SerializeField]
    List<Sprite> m_countdownSprites;
    [SerializeField]
    Transform m_gameSpawn;
    [SerializeField]
    List<TextMeshProUGUI> m_scoreboards;
    [SerializeField]
    List<TextMeshProUGUI> m_timers;
    [SerializeField]
    float m_roundTimeAmount;
    float m_timeRemaining;

    // Tutorial Area
    [SerializeField]
    List<GameObject> m_tutorialPages;
    [SerializeField]
    Button m_backButton;
    [SerializeField]
    Button m_nextButton;
    int m_currentPage = 0;

    int m_currentScore = 0;
    [SerializeField]
    float m_multiplyerDistance = 12.5f;

    [SerializeField]
    GameObject m_creditsPanel;
    [SerializeField]
    float m_creditsTime;

    //Background Audio
    [SerializeField] AudioSource m_audioSource;
    [SerializeField] AudioSource m_warningSource;
    [SerializeField] AudioClip m_menuMusic;
    [SerializeField] AudioClip m_inGameMusic;
    [SerializeField] AudioClip m_lastTenSeconds;

    private void Start()
    {
        m_player = GameObject.Find("XROrigin");
        m_sicknessWarning = GameObject.Find("SicknessPanel");
        m_surroundingsWarning = GameObject.Find("SplashImages");
        m_arcadeMenu = GameObject.Find("Arcade");
        m_tutorialObjects = GameObject.Find("TutorialObjects");
        m_startArea = GameObject.Find("StartArea");        
        m_resetSpawn = GameObject.Find("ResetSpawn").transform;        
        m_gameSpawn = GameObject.Find("SpawnPosition").transform;
        m_player.GetComponent<TeleportationProvider>().enabled = false;
        m_player.GetComponent<ActionBasedSnapTurnProvider>().enabled = false;
        StartCoroutine(Warnings());
        //Background Music Menu
        m_audioSource.clip = m_menuMusic;
        m_audioSource.Play();
    }

    IEnumerator Warnings()
    {
        yield return new WaitForSeconds(m_sicknessTime);
        m_sicknessWarning.SetActive(false);
        yield return new WaitForSeconds(m_surroundingsTime);
        m_surroundingsWarning.SetActive(false);        
        m_player.GetComponent<ActionBasedSnapTurnProvider>().enabled = true;
        m_player.GetComponent<TeleportationProvider>().enabled = true;
    }

    public void StartGame()
    {
        //Background Music - InGame
        m_audioSource.clip = m_inGameMusic;
        m_audioSource.Play();
        m_tutorialObjects.SetActive(false);
        m_arcadeMenu.SetActive(false);
        m_startArea.SetActive(false);
        m_grandstand.SetActive(true);
        m_player.transform.SetPositionAndRotation(m_gameSpawn.position, m_gameSpawn.rotation);
        m_currentScore = 0;
        foreach (TextMeshProUGUI s in m_scoreboards)
        {
            s.text = m_currentScore.ToString();
        }
        foreach(BallPodium bp in FindObjectsOfType<BallPodium>()) 
        {
            bp.ResetBall();
        }
        m_timeRemaining = m_roundTimeAmount;
        StartCoroutine(UpdateTimer());
        StartCoroutine(CountdownTimer());
    }

    IEnumerator CountdownTimer()
    {

        yield return null;
    }

    public void UpdateScores(int add)
    {
        if (Vector3.Distance(Vector3.zero, FindObjectOfType<XROrigin>().gameObject.transform.position) > m_multiplyerDistance)
        {
            m_currentScore += (add * 2);
        }
        else
        {
            m_currentScore += add;
        }
        foreach (TextMeshProUGUI s in m_scoreboards)
        {
            s.text = m_currentScore.ToString();
        }
    }

    private IEnumerator UpdateTimer()
    {
        while (m_timeRemaining >= 0)
        {
            foreach (TextMeshProUGUI t in m_timers)
            {
                float minutes = Mathf.FloorToInt(m_timeRemaining / 60);
                float seconds = Mathf.FloorToInt(m_timeRemaining % 60);
                t.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            m_timeRemaining -= Time.deltaTime;
            yield return null;
        }
        RoundEnd();
    }

    void RoundEnd()
    {
        m_startArea.SetActive(true);
        m_grandstand.SetActive(false);
        m_arcadeMenu.SetActive(true);
        m_tutorialObjects.SetActive(false);
        m_endScoreboard.SetActive(true);
        if(FindObjectOfType<HighScores>().CheckForNewHigh(m_currentScore))
        {
            m_newHighScore.SetActive(true);
        }
        else 
        {
            m_newHighScore.SetActive(false);
        }
        foreach (TextMeshProUGUI t in m_scoreboards)
        {
            t.text = m_currentScore.ToString();
        }
        m_timeRemaining = 0;
        foreach (TextMeshProUGUI t in m_timers)
        {
            float minutes = Mathf.FloorToInt(m_timeRemaining / 60);
            float seconds = Mathf.FloorToInt(m_timeRemaining % 60);
            t.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        m_player.transform.SetPositionAndRotation(m_resetSpawn.position, m_resetSpawn.rotation);

        //Change music back to Menu Music
        m_audioSource.clip = m_menuMusic;
        m_audioSource.Play();
    }

    public void Tutorial()
    {
        m_endScoreboard.SetActive(false);
        m_arcadeMenu.SetActive(false);
        m_tutorialObjects.SetActive(true);        
    }
    public void TutorialNext()
    {
        m_currentPage++;        
        if (m_currentPage == m_tutorialPages.Count)
        {            
            m_tutorialPages[m_currentPage-1].gameObject.SetActive(false);
            m_tutorialPages[0].gameObject.SetActive(true);
            m_currentPage = 0;
            StartGame();
            return;
        }
        else if (m_currentPage == 1)
        {
            m_backButton.gameObject.SetActive(true);
        }
        else if (m_currentPage == m_tutorialPages.Count - 1)
        {
            m_nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Game";
        }
        m_tutorialPages[m_currentPage - 1].gameObject.SetActive(false);
        m_tutorialPages[m_currentPage].gameObject.SetActive(true);

    }
    public void TutorialBack()
    {
        m_currentPage--;
        if (m_currentPage == 0)
        {
            m_backButton.gameObject.SetActive(false);
        }
        else if (m_currentPage == m_tutorialPages.Count - 1)
        {
            m_nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next";
        }
        m_tutorialPages[m_currentPage + 1].gameObject.SetActive(false);
        m_tutorialPages[m_currentPage].gameObject.SetActive(true);

    } 

    public IEnumerator RunCredits()
    {
        FindObjectOfType<Camera>().cullingMask = LayerMask.GetMask("Credits");
        m_creditsPanel.SetActive(true);
        yield return new WaitForSeconds(m_creditsTime);
        FindObjectOfType<Camera>().cullingMask = LayerMask.GetMask("Default") + 
            LayerMask.GetMask("Credits") + LayerMask.GetMask("UI") + LayerMask.GetMask("TransparentFX");
        m_creditsPanel.SetActive(false);
    }
}
