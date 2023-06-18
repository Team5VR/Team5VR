using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    [SerializeField] InputActionReference m_grip;
    [SerializeField] InputActionReference m_trigger;

    Animator m_hand;

    private void Awake()
    {
        m_grip.action.performed += GripAnimation;
        m_trigger.action.performed += TriggerAnimation;

        m_hand = GetComponent<Animator>();
    }

    private void TriggerAnimation(InputAction.CallbackContext obj)
    {
        m_hand.SetFloat("Trigger", obj.ReadValue<float>());
    }

    private void GripAnimation(InputAction.CallbackContext obj)
    {
        m_hand.SetFloat("Grip", obj.ReadValue<float>());
    }
}
