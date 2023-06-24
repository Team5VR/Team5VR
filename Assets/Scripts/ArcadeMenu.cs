using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Content.Interaction;

public class ArcadeMenu : MonoBehaviour
{
    [SerializeField]
    GameObject m_mainPanel;    
    [SerializeField]
    List<Button> m_ArcadeButtons;
    [SerializeField]
    List<Image> m_imageHolders;
    [SerializeField]
    List<Sprite> m_buttonImages;
    [SerializeField]
    List<Sprite> m_highlightedButtonImages;

    bool m_change = true;
    bool m_mainActive = true;
    int m_currentButton = 0;

    [SerializeField]
    GameObject m_leftHandPusher;    
    [SerializeField]
    GameObject m_rightHandPusher;

    [SerializeField]
    GameObject m_highscorePanel;
    [SerializeField]
    List<TextMeshProUGUI> m_highscores;

    void Start()
    {
        m_ArcadeButtons[0].Select();                
    }

    private void OnEnable()
    {
        m_ArcadeButtons[0].Select();
        m_leftHandPusher.SetActive(true);
        m_rightHandPusher.SetActive(true);
    }

    private void OnDisable()
    {
        if (m_leftHandPusher != null)
        {
            m_leftHandPusher.SetActive(false);
            m_rightHandPusher.SetActive(false);
        }
    }

    public void SwitchButtons(float value)
    {        
        if(value == 0)
        {
            m_change = true;
        }
        if (m_change && value != 0 && m_mainActive)
        {
            m_change = false;
            m_imageHolders[m_currentButton].sprite = m_buttonImages[m_currentButton];
            m_currentButton += (int)value;
            if (m_currentButton < 0)
            {
                m_currentButton = m_ArcadeButtons.Count - 2;
            }
            else if (m_currentButton > m_ArcadeButtons.Count - 2)
            {
                m_currentButton = 0;
            }
            m_ArcadeButtons[m_currentButton].Select();
            m_imageHolders[m_currentButton].sprite = m_highlightedButtonImages[m_currentButton];            
        }
    }

    public void PushArcadeButton()
    {
        m_ArcadeButtons[m_currentButton].onClick.Invoke();
    }

    public void PlayButton()
    {
        FindObjectOfType<GameManager>().StartGame();
    }
    public void TutorialButton()
    {
        FindObjectOfType<GameManager>().Tutorial();
    }
    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void CreditsButton()
    {
        StartCoroutine(FindObjectOfType<GameManager>().RunCredits());
    }
    public void HighscoreButton()
    {
        m_mainActive = false;
        m_mainPanel.SetActive(false);
        m_highscorePanel.SetActive(true);
        for(int i = 0; i < m_highscores.Count; i++) 
        {
            m_highscores[i].text = FindObjectOfType<HighScores>().scores.scores[i].ToString();
        }
        m_currentButton = m_ArcadeButtons.Count - 1;

    }

    public void HighscoreBack()
    {
        m_highscorePanel.SetActive(false);
        m_mainPanel.SetActive(true);
        m_ArcadeButtons[0].Select();
        m_currentButton = 0;
        m_mainActive = false;
    }


}
