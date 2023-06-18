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
    
    void Start()
    {
        m_ArcadeButtons[0].Select();       
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
}
