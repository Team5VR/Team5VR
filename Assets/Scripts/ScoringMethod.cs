using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoringMethod : MonoBehaviour
{
    // Public Point Value For Each "Hoop"
    [Range(1,6)] 
    public int m_pointValue = 1;

    public bool m_isTutorialHoop;

    //Particles
    //public ParticleSystem m_goalParticles;

    //Target Audio
    public AudioSource m_scoringAudioSource;
    public AudioClip m_scoringAudioClip;

    Material m_material;
    [SerializeField]
    Color m_startColor;
    [SerializeField]
    Color m_goalColor; 
    [SerializeField]
    float m_goalTime;


    //Slight Pull Gravity
    public enum ForceType { Repulsion = -1, None = 0, Attraction = 1 }
    public ForceType m_Type;
    public Transform m_Pivot;
    public float m_Radius;
    public float m_StopRadius;
    public float m_Force;
    public LayerMask m_Layers;

    private void Start()
    {
        m_material = GetComponentInChildren<MeshRenderer>().material;
        m_material.SetColor("_EmissionColor", m_startColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (!m_isTutorialHoop)
            {
                // Add Points to Game Manager
                FindObjectOfType<GameManager>().UpdateScores(m_pointValue);                
                StartCoroutine(FindObjectOfType<BigBlackHole>().PullBall(other.gameObject));
                StartCoroutine(ScoreColours());
            }

            // Play Particles
            //m_goalParticles.Play();

            // Play Scoring Audio
            m_scoringAudioSource.PlayOneShot(m_scoringAudioClip);
        }       
    }

    private IEnumerator ScoreColours()
    {
        m_material.SetColor("_EmissionColor", m_goalColor);
        yield return new WaitForSeconds(m_goalTime);
        m_material.SetColor("_EmissionColor", m_startColor);
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

