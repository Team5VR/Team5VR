using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringMethod : MonoBehaviour
{
    // Public Point Value For Each "Hoop"
    [Range(1,6)] 
    public int PointValue = 1;

    // Current Score
    public int CurrentScore = 0;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            CurrentScore += PointValue;
            Debug.Log("Score: " + CurrentScore);
        }
    }
}
