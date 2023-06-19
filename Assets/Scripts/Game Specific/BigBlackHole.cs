using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBlackHole : MonoBehaviour
{
    //Particles
    //public ParticleSystem GoalParticles;

    //Slight Pull Gravity    
    public Transform m_pivot;
    public float m_radius;
    public float m_force;
    LayerMask m_layer;

    private void OnTriggerEnter(Collider other)
    {
        //Play Particles
        // GoalParticles.Play();                
    }
    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(m_pivot.position, m_radius, m_layer);

        foreach (var collider in colliders)
        {
            Rigidbody body = collider.GetComponent<Rigidbody>();
            if (body == null)
                continue;

            Vector3 direction = m_pivot.position - body.position;

            float distance = direction.magnitude;

            direction = direction.normalized;            

            float forceRate = (m_force / distance);

            body.AddForce(direction * forceRate);
        }
    }
}