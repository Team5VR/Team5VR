using UnityEngine;

public class BallPodium : MonoBehaviour
{
    [SerializeField]
    GameObject m_ball;
    [SerializeField]
    Transform m_despawnPoint;
    Vector3 m_ballStart;

    private void Start()
    {
        m_ballStart = m_ball.transform.position;
    }
    private void Update()
    {
        if(m_ball.transform.position.y <= m_despawnPoint.position.y)
        {
            m_ball.transform.SetPositionAndRotation(m_ballStart, m_ball.transform.rotation);
            m_ball.layer = LayerMask.GetMask("Ball");
        }
    }
}
