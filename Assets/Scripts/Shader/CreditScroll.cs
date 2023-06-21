using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class CreditScroll : MonoBehaviour
{
    bool isScrolling;
    float rotation;
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
            Vector3 currentUIPosition = gameObject.transform.position;

            Vector3 incrementYPosition = new Vector3(currentUIPosition.y,
                currentUIPosition.y + 0.1f * Mathf.Sin(Mathf.Deg2Rad * rotation),
                currentUIPosition.z + 0.1f * Mathf.Cos(Mathf.Deg2Rad * rotation));

                gameObject.transform.position = incrementYPosition;
        }
    }
}
