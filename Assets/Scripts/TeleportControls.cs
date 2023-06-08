using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TeleportControls : MonoBehaviour
{
    public GameObject m_controllerGO;
    public GameObject m_teleporterGO;

    public InputActionReference m_teleportActivation;

    public UnityEvent m_onTeleportActivate;
    public UnityEvent m_onTeleportCancel;

    private void Start()
    {
        m_teleportActivation.action.performed += TeleportModeActivate;
        m_teleportActivation.action.canceled += TeleportModeCancel;
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
}
