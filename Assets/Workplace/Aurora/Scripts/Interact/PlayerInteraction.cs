using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour {

    public static Action OnHide;
    public static Action<string> OnInteract;

    [Header("References")]
    [SerializeField] private Transform target;

    [Header("Settings")]
    [SerializeField, Range(1.5f, 5f)] private float interactionRange = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private InputAction interact;

    private IInteractable current;

    private void OnEnable() => interact.Enable();
    private void OnDisable() => interact.Disable();

    private void Start() {

        if (target == null) target = Camera.main.transform;
    }
    private void Update() {

        Ray ray = new(target.position, target.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactableLayer)) {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable)) {

                if (current != interactable) {

                    current = interactable;
                    OnInteract?.Invoke(current.InteractionPrompt);
                }
            } else {
                current = null;
                OnHide?.Invoke();
            }
        } else {
            current = null;
            OnHide?.Invoke();
        }
        if (interact.WasPressedThisFrame() && current != null) {

            current.Interact();
            OnHide?.Invoke();
        }
    }
}