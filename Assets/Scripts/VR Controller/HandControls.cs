using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class HandControls : MonoBehaviour
{
    public GameObject m_controllerGO;
    public GameObject m_teleporterGO;
    public GameObject m_pointerGO;

    public InputActionReference m_teleportActivation;
    public InputActionReference m_grabPointActivation;
    public InputActionReference m_UIPointerActivation;

    public UnityEvent m_onTeleportActivate;
    public UnityEvent m_onTeleportCancel;

    public UnityEvent m_onGrabPressed;
    public UnityEvent m_onGrabReleased;

    public UnityEvent m_onPointPressed;
    public UnityEvent m_onPointReleased;

    private void Start()
    {
        m_teleportActivation.action.performed += TeleportModeActivate;
        m_teleportActivation.action.canceled += TeleportModeCancel;
        m_grabPointActivation.action.performed += GrabLine;
        m_grabPointActivation.action.canceled += NoGrabLine;
        m_UIPointerActivation.action.performed += PointUILine;
        m_UIPointerActivation.action.canceled += StopPointUILine;
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

    private void PointUILine(InputAction.CallbackContext obj)
    {
        m_onPointPressed.Invoke();
    }

    private void StopPointUILine(InputAction.CallbackContext obj)
    {
        m_onPointReleased.Invoke();
    }
}
