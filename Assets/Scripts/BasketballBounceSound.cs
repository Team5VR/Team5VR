using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballBounceSound : MonoBehaviour
{
    public AudioClip bounceSound;

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            other.gameObject.GetComponent<AudioSource>().clip = bounceSound;
            other.gameObject.GetComponent<AudioSource>().Play();
            Debug.Log("PlaySound" + gameObject.name);
        }
    }

}
