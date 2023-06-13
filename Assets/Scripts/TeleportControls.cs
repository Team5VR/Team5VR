using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TeleportControls : MonoBehaviour
{
    public GameObject m_controllerGO;
    public GameObject m_teleporterGO;

    public InputActionReference m_teleportActivation;
    public InputActionReference m_grabPointActivation;

    public UnityEvent m_onTeleportActivate;
    public UnityEvent m_onTeleportCancel;

    public UnityEvent m_onGrabPressed;
    public UnityEvent m_onGrabReleased;

    private void Start()
    {
        m_teleportActivation.action.performed += TeleportModeActivate;
        m_teleportActivation.action.canceled += TeleportModeCancel;
        m_grabPointActivation.action.performed += GrabLine;
        m_grabPointActivation.action.canceled += NoGrabLine;
    }

    private void TeleportModeActivate(InputAction.CallbackContext obj)
    {
        m_onTeleportActivate.Invoke();
    }

    private void TeleportModeCancel(InputAction.CallbackContext obj)
    {
        Invoke("DeactivateTeleporter", 0.1f);
    }

    void DeactivateTeleporter() 
    {
        m_onTeleportCancel.Invoke(); 
    }

    private void GrabLine(InputAction.CallbackContext obj)
    {
        m_onGrabPressed.Invoke();
    }

    private void NoGrabLine(InputAction.CallbackContext obj)
    {
        m_onGrabReleased.Invoke();
    }
}
