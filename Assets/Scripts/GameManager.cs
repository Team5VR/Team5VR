using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    // Application Open
    [SerializeField]
    GameObject m_player;
    [SerializeField]
    GameObject m_sicknessWarning;
    [SerializeField]
    float m_sicknessTime;
    [SerializeField]
    GameObject m_surroundingsWarning;
    [SerializeField]
    float m_surroundingsTime;

    // Menu Area    
    [SerializeField]
    GameObject m_arcadeMenu;
    [SerializeField]
    GameObject m_tutorialObjects;
    [SerializeField]
    GameObject m_startArea;
    [SerializeField]
    GameObject m_grandstand;
    [SerializeField]
    Transform m_resetSpawn;
    [SerializeField]
    GameObject m_endScoreboard;
    [SerializeField]
    GameObject m_newHighScore;

    // Game Loop Area
    [SerializeField]
    GameObject m_handMenu;
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
    [SerializeField]
    Button m_playButton;
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
    [SerializeField] AudioClip m_inGameMusic;
    [SerializeField] AudioClip m_lastTenSeconds;
    [SerializeField] AudioSource m_cheerSource;
    [SerializeField] List<AudioClip> m_cheers;

    private void Start()
    {         
        m_player.GetComponent<TeleportationProvider>().enabled = false;
        m_player.GetComponent<ActionBasedSnapTurnProvider>().enabled = false;
        StartCoroutine(Warnings());               
    }

    IEnumerator Warnings()
    {
        yield return new WaitForSeconds(m_sicknessTime);
        m_sicknessWarning.SetActive(false);
        yield return new WaitForSeconds(m_surroundingsTime);
        m_surroundingsWarning.SetActive(false);
        m_arcadeMenu.SetActive(true);
        m_player.GetComponent<ActionBasedSnapTurnProvider>().enabled = true;
        m_player.GetComponent<TeleportationProvider>().enabled = true;
    }

    public void StartGame()
    {
        m_handMenu.SetActive(true);
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
        foreach (BallPodium bp in FindObjectsOfType<BallPodium>())
        {
            bp.ResetBall();
        }
        m_timeRemaining = m_roundTimeAmount;
        StartCoroutine(UpdateTimer());
        StartCoroutine(CountdownTimer());
    }

    IEnumerator CountdownTimer()
    {
        m_countdown.gameObject.SetActive(true);        
        m_countdown.sprite = m_countdownSprites[0];
        yield return new WaitForSeconds(1);
        m_countdown.sprite = m_countdownSprites[1];
        yield return new WaitForSeconds(1);
        m_countdown.sprite = m_countdownSprites[2];
        yield return new WaitForSeconds(1);
        m_countdown.sprite = m_countdownSprites[3];
        yield return new WaitForSeconds(1);
        m_countdown.gameObject.SetActive(false);
    }

    public void UpdateScores(int add)
    {
        m_cheerSource.clip = m_cheers[Random.Range(0, m_cheers.Count)];
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
        bool warning = true;
        while (m_timeRemaining >= 0)
        {
            foreach (TextMeshProUGUI t in m_timers)
            {
                float minutes = Mathf.FloorToInt(m_timeRemaining / 60);
                float seconds = Mathf.FloorToInt(m_timeRemaining % 60);
                t.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            m_timeRemaining -= Time.deltaTime;
            if (m_timeRemaining <= 10)
            {
                if (warning)
                {
                    m_warningSource.clip = m_lastTenSeconds;
                    m_warningSource.Play();
                    warning = false;
                }
            }
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
        if (FindObjectOfType<HighScores>().CheckForNewHigh(m_currentScore))
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

        BallPodium[] bps = FindObjectsOfType<BallPodium>();
        for (int i = 0; i < bps.Count(); i++)
        {
            bps[i].ResetBall();
        }
        m_handMenu.SetActive(false);
    }

    public void Tutorial()
    {
        m_endScoreboard.SetActive(false);
        m_arcadeMenu.SetActive(false);
        m_tutorialObjects.SetActive(true);
        m_nextButton.gameObject.SetActive(true);
        m_playButton.gameObject.SetActive(false);
    }
    public void TutorialNext()
    {
        m_currentPage++;
        if (m_currentPage == m_tutorialPages.Count)
        {
            m_tutorialPages[m_currentPage - 1].gameObject.SetActive(false);
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
            m_nextButton.gameObject.SetActive(false);
            m_playButton.gameObject.SetActive(true);
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
            m_nextButton.gameObject.SetActive(true);
            m_playButton.gameObject.SetActive(false);
        }
        m_tutorialPages[m_currentPage + 1].gameObject.SetActive(false);
        m_tutorialPages[m_currentPage].gameObject.SetActive(true);

    }

    public IEnumerator RunCredits()
    {
          FindObjectOfType<Camera>().cullingMask = LayerMask.GetMask("Credits");
          m_creditsPanel.SetActive(true);
          yield return new WaitForSeconds(m_creditsTime);
          FindObjectOfType<Camera>().cullingMask = ~0;
          m_creditsPanel.SetActive(false);
    }
}
