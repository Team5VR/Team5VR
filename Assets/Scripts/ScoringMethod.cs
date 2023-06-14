using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoringMethod : MonoBehaviour
{
    // Public Point Value For Each "Hoop"
    [Range(1,6)] 
    public int PointValue = 1;

    //Allowing For Multiplying Points 
    public bool MultiplyPoints = false;

    //Combo Value
    public int Combo = 1;

    //Slight Pull Gravity
    public enum ForceType { Repulsion = -1, None = 0, Attraction = 1 }
    public ForceType m_Type;
    public Transform m_Pivot;
    public float m_Radius;
    public float m_StopRadius;
    public float m_Force;
    public LayerMask m_Layers;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {       
            int multiplyValue = (MultiplyPoints ? 2 : 1);
            
            FindObjectOfType<GameManager>().CurrentScore += (PointValue * Combo * multiplyValue);
            Debug.Log("Score: " + FindObjectOfType<GameManager>().CurrentScore);           
        }       
    }
    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(m_Pivot.position, m_Radius, m_Layers);

        float signal = (float)m_Type;

        foreach (var collider in colliders)
        {
            Rigidbody body = collider.GetComponent<Rigidbody>();
            if (body == null)
                continue;

            Vector3 direction = m_Pivot.position - body.position;

            float distance = direction.magnitude;

            direction = direction.normalized;

            if (distance < m_StopRadius)
                continue;

            float forceRate = (m_Force / distance);

            body.AddForce(direction * (forceRate / body.mass) * signal);
        }
    }
}

