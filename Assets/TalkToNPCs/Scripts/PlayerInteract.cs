using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {


    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
       
            IInteractable interactable = GetInteractableObject();
            if (interactable != null) {
                interactable.Interact(transform);
            }
        }
    }

    public IInteractable GetInteractableObject() {
        List<IInteractable> npcinteractable = new List<IInteractable>();
        float interactRange = 3f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray) {
            if (collider.TryGetComponent(out NPCInteractable interactable)) {
                npcinteractable.Add(interactable);
            }
        }

        IInteractable closestInteractable = null;
        foreach (IInteractable interactable in npcinteractable) {
            if (closestInteractable == null) {
                closestInteractable = interactable;
            } else {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) < 
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position)) {
                    // Closer
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }

}