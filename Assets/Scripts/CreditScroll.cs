using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class CreditScroll : MonoBehaviour
{
    bool isScrolling;
    float rotation;
    public float speed = 10.0f;
    Vector3 startPosition;

    private void Start()
    {
        startPosition = gameObject.transform.localPosition;
    }

    void OnEnable()
    {
        Setup();
    }

    void Setup()
    {
        gameObject.transform.localPosition = startPosition;
        isScrolling = true;
        rotation = gameObject.GetComponent<Transform>().eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
     if(isScrolling)
        {            
            gameObject.transform.localPosition += Vector3.up * rotation * Time.deltaTime;
        }
    }
}
