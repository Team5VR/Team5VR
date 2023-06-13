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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {       
            int multiplyValue = (MultiplyPoints ? 2 : 1);
            
            FindObjectOfType<GameManager>().CurrentScore += (PointValue * Combo * multiplyValue);
            Debug.Log("Score: " + FindObjectOfType<GameManager>().CurrentScore);           
        }       
    }
}
