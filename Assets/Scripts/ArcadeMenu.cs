using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Content.Interaction;

public class ArcadeMenu : MonoBehaviour
{
    [SerializeField]
    List<Button> m_ArcadeButtons;
    bool m_change = true;
    int m_currentButton = 0;
    [SerializeField]
    GameObject m_leftHand;
    [SerializeField]
    GameObject m_rightHand;

    GameObject m_mainPanel;
    GameObject m_creditsPanel;
    GameObject m_highscorePanel;

    void Start()
    {
        m_ArcadeButtons[0].Select();
        m_mainPanel = GameObject.Find("arcadeMain");
        m_creditsPanel = GameObject.Find("arcadeCredits");
        m_highscorePanel = GameObject.Find("arcadeHighscore");
    }

    private void OnEnable()
    {
        m_ArcadeButtons[0].Select();
        m_leftHand.SetActive(true);
        m_rightHand.SetActive(true);
    }

    private void OnDisable() //hi harley
    {
        if (m_leftHand != null)//how ya doin
        {
            m_leftHand.SetActive(false);//<3 harry
            m_rightHand.SetActive(false);
        }
    }

    public void SwitchButtons(float value)
    {        
        if(value == 0)
        {
            m_change = true;
        }
        if (m_change && value != 0)
        {
            m_change = false;
            m_currentButton += (int)value;
            if (m_currentButton < 0)
            {
                m_currentButton = m_ArcadeButtons.Count - 1;
            }
            else if (m_currentButton > m_ArcadeButtons.Count - 1)
            {
                m_currentButton = 0;
            }
            m_ArcadeButtons[m_currentButton].Select();            
        }
    }

    public void PushArcadeButton()
    {
        m_ArcadeButtons[m_currentButton].onClick.Invoke();
    }

    public void Credits()
    {
        m_mainPanel.SetActive(false);
        m_creditsPanel.SetActive(false);
    }

    public void HighScore()
    {

    }
}
