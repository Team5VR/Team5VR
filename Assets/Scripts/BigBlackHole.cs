using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBlackHole : MonoBehaviour
{
    //Particles
    //public ParticleSystem GoalParticles;

    //Slight Pull Gravity    
    Transform m_centrePoint;
    public float m_radius;
    public float m_force;    
    public float m_velocityPercentage;
    void Start()
    {
        m_centrePoint = this.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Play Particles
        // GoalParticles.Play();                
    }
    public IEnumerator PullBall(GameObject deadBall)
    {
        deadBall.tag = "DeadBall";
        float distance = 2;        
        deadBall.GetComponent<Rigidbody>().velocity = deadBall.GetComponent<Rigidbody>().velocity * m_velocityPercentage;
        while (distance > 0.01f)
        {
            Vector3 direction = m_centrePoint.position - deadBall.transform.position;

            distance = Vector3.Distance(deadBall.transform.position, m_centrePoint.position);

            direction = direction.normalized;

            float forceRate = (m_force / distance);

            deadBall.GetComponent<Rigidbody>().AddForce(direction * forceRate);

            yield return null;
        }
        deadBall.tag = "Ball";
    }
}