using System;
using System.Collections;
using UnityEngine;

public class ScoringMethod : MonoBehaviour
{
    // Public Point Value For Each "Hoop"
    [Range(1,6)] 
    public int m_pointValue = 1;

    public bool m_isTutorialHoop;

    //Particles
    public ParticleSystem m_goalParticles;

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
            m_goalParticles.Play();
            Debug.Log("Particles YAY!");

            // Play Scoring Audio
            m_scoringAudioSource.PlayOneShot(m_scoringAudioClip);
            Debug.Log("Sound YAY!");
        }       
    }

    private IEnumerator ScoreColours()
    {
        m_material.SetColor("_EmissionColor", m_goalColor);
        yield return new WaitForSeconds(m_goalTime);
        m_material.SetColor("_EmissionColor", m_startColor);
    }
  
}

