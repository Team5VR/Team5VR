using System.Collections;
using System.Collections.Generic;
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

    // Game Loop Area
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
    int m_currentPage = 0;
    [SerializeField]
    Button m_backButton;
    [SerializeField]
    Button m_nextButton;

    
    int m_currentScore = 0;
    [SerializeField]
    float m_multiplyerDistance = 12.5f;

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
        m_surroundingsWarning.GetComponentInParent<Canvas>().enabled = false;
        m_player.GetComponent<ActionBasedSnapTurnProvider>().enabled = true;
        m_player.GetComponent<TeleportationProvider>().enabled = true;
    }

    public void StartGame()
    {
        m_tutorialObjects.SetActive(false);
        m_arcadeMenu.SetActive(false);
        m_startArea.SetActive(false);
        m_grandstand.SetActive(true);
        m_player.transform.SetPositionAndRotation(m_gameSpawn.position, m_gameSpawn.rotation);
        m_currentScore = 0;
        StartCoroutine(UpdateTimer());
    }

    public void UpdateScores(int add)
    {
        if (Vector3.Distance(Vector3.zero, FindObjectOfType<XROrigin>().gameObject.transform.position)>m_multiplyerDistance)
        {
            m_currentScore += (add * 2);
        }
        else
        {
            m_currentScore += add;
        }
        foreach(TextMeshProUGUI s in m_scoreboards)
        {
            s.text = m_currentScore.ToString();
        }
    }

    private IEnumerator UpdateTimer()
    {
        while(m_timeRemaining >= 0)
        {
            foreach(TextMeshProUGUI t in m_timers)
            {
                t.text = $"{m_timeRemaining / 60:00}:{m_timeRemaining % 60:00}";
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
        foreach(TextMeshProUGUI t in m_scoreboards)
        {
            t.text = m_currentScore.ToString();
        }
        m_timeRemaining = 0;
        foreach (TextMeshProUGUI t in m_timers)
        {
            t.text = $"{m_timeRemaining / 60:00}:{m_timeRemaining % 60:00}";
        }
        m_player.transform.SetPositionAndRotation(m_resetSpawn.position, m_resetSpawn.rotation);
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
        if(m_currentPage >m_tutorialPages.Count)
        {
            StartGame();
        }
        else 
        {
            if(m_currentPage == m_tutorialPages.Count)
            {
                m_nextButton.GetComponent<TextMeshProUGUI>().text = "Start Game";
            }
            m_backButton.gameObject.SetActive(true);
            m_tutorialPages[m_currentPage-1].gameObject.SetActive(false);
            m_tutorialPages[m_currentPage].gameObject.SetActive(true);
        }
    }
    public void TutorialBack()
    {
        m_currentPage--;
        if (m_currentPage == 0)
        {
            m_backButton.gameObject.SetActive(false);
        }
        else
        {
            if(m_currentPage == m_tutorialPages.Count - 1)
            {
                m_nextButton.GetComponent<TextMeshProUGUI>().text = "Next";
            }
            m_tutorialPages[m_currentPage + 1].gameObject.SetActive(false);
            m_tutorialPages[m_currentPage].gameObject.SetActive(true);
        }
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
