using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class CreditScroll : MonoBehaviour
{
    bool isScrolling;
    float rotation;
    public float speed = 10.0f;
    void OnEnable()
    {
        Setup();
    }

    void Setup()
    {
        isScrolling = true;
        rotation = gameObject.GetComponent<Transform>().eulerAngles.x;
        Debug.Log("Parent rotation:" + rotation);
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
