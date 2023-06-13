using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextOnOffScripts : MonoBehaviour
{
    public GameObject One;
    public GameObject Two;
    public GameObject Three;
    public GameObject Four;
    public GameObject Five;
    public GameObject Six;
    [SerializeField] float delay;
    // Start is called before the first frame update
    void Start()
    {
        StartFunc();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void StartFunc()
    {
        One.SetActive(true);
        Invoke(nameof(OneFunc), delay);
    }
    private void OneFunc()
    {
        One.SetActive(false);
        Two.SetActive(true);
        Invoke(nameof(TwoFunc), delay);
    }
    private void TwoFunc()
    {
        Two.SetActive(false);
        Three.SetActive(true);
        Invoke(nameof(ThreeFunc), delay);
    }
    private void ThreeFunc()
    {
        Three.SetActive(false);
        Four.SetActive(true);
        Invoke(nameof(FourFunc), delay);
    }
    private void FourFunc()
    {
        Four.SetActive(false);
        Five.SetActive(true);
        Invoke(nameof(FiveFunc), delay);
    }
    private void FiveFunc()
    {
        Five.SetActive(false);
        Six.SetActive(true);
        //Invoke(nameof(TwoFunc), delay);
    }
}
