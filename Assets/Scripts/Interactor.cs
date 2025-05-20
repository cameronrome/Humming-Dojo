using UnityEngine;
using UnityEngine.InputSystem;

interface Interactable
{
    void Interact();
}

public class Interactor : MonoBehaviour
{
    [SerializeField] private float interactRadius = 2f;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Interact.Enable();
        inputActions.Player.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Player.Interact.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRadius);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out Interactable interactObj))
            {
                interactObj.Interact();
                break;
            }
        }
    }
}
