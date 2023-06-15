using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public GameObject m_sicknessWarning;
    public GameObject m_surroundingsWarning;

    public Transform m_startMenuPos;
    public GameObject m_arcadeMenu;
    public GameObject m_tutorialArea;
    public BallPodium m_tutorialBall;
    public List<GameObject> m_tutorialPages;

    public UnityAction<XRBaseInteractor> tes;

    
    public int CurrentScore = 0;

    
}
