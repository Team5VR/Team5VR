using UnityEngine;

public class BallPodium : MonoBehaviour
{
    [SerializeField]
    GameObject m_ball;    
    Transform m_despawnPoint;
    Vector3 m_ballStart;

    private void Start()
    {
        m_ballStart = m_ball.transform.position;
        m_despawnPoint = FindObjectOfType<BigBlackHole>().transform;
    }
    private void Update()
    {
        if((m_ball.transform.position.y <= m_despawnPoint.position.y - 20 && m_ball.CompareTag("Ball")) || 
            Vector3.Distance(m_ball.transform.position, m_despawnPoint.position) <= 0.01f)
        {            
            m_ball.GetComponent<Rigidbody>().isKinematic = true;
            m_ball.GetComponent<TrailRenderer>().enabled = false;
            m_ball.transform.SetPositionAndRotation(m_ballStart, m_ball.transform.rotation);            
            m_ball.GetComponent<TrailRenderer>().enabled = true;
            m_ball.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
